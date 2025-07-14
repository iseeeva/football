using System.Collections.Concurrent;
using System.Net.Sockets;
using Serilog;
using Sobee.Common;
using Sobee.Network;

public class SessionQueueHandle : Component
{
    private readonly ILogger _log = Logging.Get<SessionQueueHandle>();
    private readonly SemaphoreSlim _sendSemaphore = new SemaphoreSlim(1, 1);
    private bool _isDisposed;

    private readonly Session _session;

    public static int MaxSendingSize { get; private set; } = 32768;
    public static int MaxReceivingSize { get; private set; } = 4096;

    private readonly ConcurrentQueue<byte[]> _sendQueue = new ConcurrentQueue<byte[]>();
    private readonly ConcurrentQueue<byte[]> _receiveQueue = new ConcurrentQueue<byte[]>();

    private readonly byte[] _receiveBuffer = new byte[MaxReceivingSize];
    private int _receiveBufferOffset;

    public long totalReceive { get; private set; }
    public long totalBytesReceive { get; private set; }
    public long totalSent { get; private set; }
    public long totalBytesSent { get; private set; }
    public long totalQueued => _sendQueue.Count + _receiveQueue.Count;

    public SessionQueueHandle(Session session)
    {
        try
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
            _ = StartReceivingAsync();

            _log.Debug("{id} initialized.", this.Id);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public override async Task Update()
    {
        try
        {
            await ProcessSendQueueAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    private async Task ProcessSendQueueAsync()
    {
        if (!_session.IsConnected) return;

        await _sendSemaphore.WaitAsync();

        try
        {
            while (_sendQueue.TryDequeue(out var message))
            {
                try
                {
                    await _session.Socket.SendAsync(new ArraySegment<byte>(message), SocketFlags.None);
                    totalBytesSent += message.Length;
                    totalSent++;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
        finally
        {
            _sendSemaphore.Release();
        }
    }

    public void EnqueueSendData(byte[] data)
    {
        if (data == null || data.Length == 0)
        {
            _log.Warning("Attempted to enqueue an empty or null message.");
            return;
        }

        var header = BitConverter.GetBytes(data.Length);
        var messageWithHeader = new byte[header.Length + data.Length];
        Buffer.BlockCopy(header, 0, messageWithHeader, 0, header.Length);
        Buffer.BlockCopy(data, 0, messageWithHeader, header.Length, data.Length);
        _sendQueue.Enqueue(messageWithHeader);
    }

    public byte[]? DequeueSendData()
    {
        return _sendQueue.TryDequeue(out var message) ? message : null;
    }

    private async Task StartReceivingAsync(CancellationToken cancellationToken = default)
    {
        if (!_session.IsConnected) return;

        try
        {
            while (_session.IsConnected && !cancellationToken.IsCancellationRequested)
            {
                int bytesRead = await _session.Socket.ReceiveAsync(
                    new ArraySegment<byte>(_receiveBuffer, _receiveBufferOffset, _receiveBuffer.Length - _receiveBufferOffset),
                    SocketFlags.None,
                    cancellationToken
                );

                if (bytesRead == 0)
                {
                    break;
                }

                _receiveBufferOffset += bytesRead;
                while (_receiveBufferOffset > 4)
                {
                    int messageLength = BitConverter.ToInt32(_receiveBuffer, 0);
                    if (messageLength > MaxReceivingSize || messageLength <= 0)
                    {
                        //throw new Exception("Message length out of range"));
                        return;
                    }

                    int totalLength = 4 + messageLength;
                    if (_receiveBufferOffset < totalLength) break;

                    byte[] message = new byte[messageLength];
                    Array.Copy(_receiveBuffer, 4, message, 0, messageLength);
                    Array.Copy(_receiveBuffer, totalLength, _receiveBuffer, 0, _receiveBufferOffset - totalLength);

                    _receiveBufferOffset -= totalLength;
                    totalBytesReceive += message.Length;
                    totalReceive++;

                    EnqueueReceiveData(message);
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public void EnqueueReceiveData(byte[] data)
    {
        if (data == null || data.Length == 0)
        {
            _log.Warning("Attempted to enqueue an empty or null received message.");
            return;
        }

        _receiveQueue.Enqueue(data);
    }

    public byte[]? DequeueReceiveData()
    {
        return _receiveQueue.TryDequeue(out var message) ? message : null;
    }

    protected override void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            _isDisposed = true;

            if (disposing)
            {
                _log.Debug("{id} disposing.", this.Id);

                _session.Dispose();
                _sendSemaphore.Dispose();

                _sendQueue.Clear();
                totalSent = 0;
                totalBytesSent = 0;

                _receiveQueue.Clear();
                _receiveBufferOffset = 0;
                totalReceive = 0;
                totalBytesReceive = 0;

                _log.Debug("{id} disposed.", this.Id);
            }
        }

        base.Dispose(disposing);
    }
}
