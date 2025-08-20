using System.Collections.Concurrent;
using System.Net.Sockets;
using Serilog;
using Sobee.Common;

namespace Sobee.Network
{
    public class SessionQueueHandle : Component
    {
        private static readonly ILogger _log = Logging.Get<SessionQueueHandle>();
        private bool _isDisposed;

        private readonly SemaphoreSlim _sendSemaphore = new(1, 1);
        private readonly CancellationTokenSource _cts = new();

        private readonly ConcurrentQueue<byte[]> _sendQueue = new();
        private readonly ConcurrentQueue<byte[]> _receiveQueue = new();

        private readonly byte[] _receiveBuffer = new byte[MaxReceivingSize];
        private int _receiveBufferOffset;

        protected readonly Session Session;

        public static int MaxSendingSize { get; private set; } = 32768;
        public static int MaxReceivingSize { get; private set; } = 4096;

        public long TotalReceive { get; private set; }
        public long TotalBytesReceive { get; private set; }
        public long TotalSent { get; private set; }
        public long TotalBytesSent { get; private set; }
        public long TotalQueued => _sendQueue.Count + _receiveQueue.Count;

        public SessionQueueHandle(Session session)
        {
            Session = session ?? throw new ArgumentNullException(nameof(session));
            _ = StartReceivingAsync(_cts.Token);

            _log.Debug("{id} initialized.", this.Id);
        }

        public override Task Update(double delta)
        {
            if (_isDisposed) return Task.CompletedTask;
            if (!Session.IsConnected) return Task.CompletedTask;

            _ = ProcessSendQueueAsync();
            return Task.CompletedTask;
        }


        protected async Task ProcessSendQueueAsync()
        {
            ObjectDisposedException.ThrowIf(_isDisposed, this);
            if (!Session.IsConnected) return;

            await _sendSemaphore.WaitAsync();

            try
            {
                while (_sendQueue.TryDequeue(out var message))
                {
                    await Session.Socket.SendAsync(message, SocketFlags.None);
                    TotalBytesSent += message.Length;
                    TotalSent++;
                }
            }
            catch (SocketException ex)
            {
                _log.Error(ex, "Socket send failed.");
            }
            finally
            {
                _sendSemaphore.Release();
            }
        }

        public void EnqueueSendData(byte[] data)
        {
            ObjectDisposedException.ThrowIf(_isDisposed, this);

            if (data == null || data.Length == 0)
            {
                _log.Warning("Attempted to enqueue empty or null send data.");
                return;
            }

            if (data.Length > MaxSendingSize)
            {
                _log.Warning("Send data size exceeds maximum allowed size.");
                return;
            }

            var header = BitConverter.GetBytes(data.Length);
            var message = new byte[header.Length + data.Length];
            Buffer.BlockCopy(header, 0, message, 0, header.Length);
            Buffer.BlockCopy(data, 0, message, header.Length, data.Length);

            _sendQueue.Enqueue(message);
        }

        public byte[]? DequeueSendData()
        {
            ObjectDisposedException.ThrowIf(_isDisposed, this);
            return _sendQueue.TryDequeue(out var message) ? message : null;
        }

        private async Task StartReceivingAsync(CancellationToken cancellationToken)
        {
            ObjectDisposedException.ThrowIf(_isDisposed, this);

            try
            {
                while (Session.IsConnected && !cancellationToken.IsCancellationRequested)
                {
                    var segment = new ArraySegment<byte>(_receiveBuffer, _receiveBufferOffset, MaxReceivingSize - _receiveBufferOffset);
                    int bytesRead = await Session.Socket.ReceiveAsync(segment, SocketFlags.None, cancellationToken);

                    if (bytesRead == 0) break;

                    _receiveBufferOffset += bytesRead;

                    while (_receiveBufferOffset >= 4)
                    {
                        int messageLength = BitConverter.ToInt32(_receiveBuffer, 0);

                        if (messageLength <= 0 || messageLength > MaxReceivingSize)
                        {
                            _log.Error("Received invalid message length: {length}", messageLength);
                            return;
                        }

                        int totalLength = 4 + messageLength;
                        if (_receiveBufferOffset < totalLength)
                            break;

                        var message = new byte[messageLength];
                        Buffer.BlockCopy(_receiveBuffer, 4, message, 0, messageLength);
                        Buffer.BlockCopy(_receiveBuffer, totalLength, _receiveBuffer, 0, _receiveBufferOffset - totalLength);

                        _receiveBufferOffset -= totalLength;
                        TotalReceive++;
                        TotalBytesReceive += message.Length;

                        _receiveQueue.Enqueue(message);
                    }
                }
            }
            catch (SocketException ex)
            {
                _log.Error(ex, "Socket receive failed.");
            }
            catch (OperationCanceledException)
            {
                _log.Debug("Receiving stopped on {id}.", this.Id);
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Unexpected receive error.");
            }
        }

        public void EnqueueReceiveData(byte[] data)
        {
            ObjectDisposedException.ThrowIf(_isDisposed, this);

            if (data == null || data.Length == 0)
            {
                _log.Warning("Attempted to enqueue empty or null received data.");
                return;
            }

            _receiveQueue.Enqueue(data);
        }

        public byte[]? DequeueReceiveData()
        {
            ObjectDisposedException.ThrowIf(_isDisposed, this);
            return _receiveQueue.TryDequeue(out var message) ? message : null;
        }

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed) return;
            _isDisposed = true;

            if (disposing)
            {
                _log.Debug("{id} disposing.", this.Id);

                _cts.Cancel();
                _cts.Dispose();

                _sendSemaphore.Dispose();
                Session.Dispose();

                _sendQueue.Clear();
                _receiveQueue.Clear();

                TotalSent = 0;
                TotalBytesSent = 0;
                TotalReceive = 0;
                TotalBytesReceive = 0;
                _receiveBufferOffset = 0;

                _log.Debug("{id} disposed.", this.Id);
            }

            base.Dispose(disposing);
        }
    }
}