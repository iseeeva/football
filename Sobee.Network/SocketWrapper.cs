using System.Net;
using System.Net.Sockets;
using Sobee.Common;

namespace Sobee.Network
{
    public class SocketWrapper : Component
    {
        public bool IsActive { get; private set; }
        private static readonly Serilog.ILogger _log = Logging.Get<SocketWrapper>();

        private readonly Socket _socket;
        private bool _isDisposed;

        public static readonly int MAX_SEND_SIZE = 32768;
        public static readonly int MAX_RECEIVE_SIZE = 4096;

        private readonly MemoryStream _sendBuffer = new(MAX_SEND_SIZE);
        private readonly Queue<byte[]> _receiveQueue = new();
        private readonly byte[] _receiveBuffer = new byte[MAX_RECEIVE_SIZE];
        private int _processedBytesInBuffer;

        public event EventHandler? Disconnected;
        public event EventHandler<ConnectionErrorEvent>? SocketError;
        public event EventHandler<ConnectionErrorEvent>? ConnectionError;
        public event EventHandler<ConnectionErrorEvent>? SendError;
        public event EventHandler<ConnectionErrorEvent>? ReceiveError;

        public long TotalBytesSent { get; private set; }
        public long TotalBytesReceived { get; private set; }
        public long TotalPacketsSent { get; private set; }
        public long TotalPacketsReceived { get; private set; }

        public bool IsConnected => _socket.Connected;
        public EndPoint? RemoteEndPoint => _socket.RemoteEndPoint;

        public SocketWrapper(Socket socket)
        {
            _socket = socket ?? throw new ArgumentNullException(nameof(socket));
            _log.Information("{SocketId} initialized. (source: {ip})", Id, _socket.RemoteEndPoint);
        }

        public virtual void Start()
        {
            if (IsConnected && !IsActive)
            {
                IsActive = true;
                BeginReceive(_processedBytesInBuffer);

                _log.Information("{SocketId} started.", Id);
            }
        }

        public virtual void Stop()
        {
            if (IsActive)
            {
                IsActive = false;
                _log.Information("{SocketId} stopped.", Id);
            }
        }

        public void EnqueueSend(byte[] data, int offset, int count)
        {
            if (!IsConnected) return;

            lock (_sendBuffer)
            {
                _sendBuffer.Write(BitConverter.GetBytes(count), 0, 4);
                _sendBuffer.Write(data, offset, count);
                TotalPacketsSent++;
            }
        }

        public int SendSync()
        {
            if (!IsConnected) return 0;

            lock (_sendBuffer)
            {
                try
                {
                    int length = (int)_sendBuffer.Length;
                    if (length == 0) return 0;

                    int sent = _socket.Send(_sendBuffer.GetBuffer(), 0, length, SocketFlags.None);
                    _sendBuffer.SetLength(0);

                    TotalBytesSent += sent;
                    return sent;
                }
                catch (SocketException ex)
                {
                    OnSendError(ex.SocketErrorCode);
                    OnSocketError(ex.SocketErrorCode);
                    Disconnect();
                }
                catch (ObjectDisposedException)
                {
                    OnDisconnected();
                }

                return 0;
            }
        }

        public byte[]? ReceiveSync()
        {
            if (!IsConnected) return null;

            try
            {
                if (!_socket.Poll(0, SelectMode.SelectRead) || _socket.Available < 4)
                    return null;

                _socket.Receive(_receiveBuffer, 4, SocketFlags.Peek);
                int packetSize = BitConverter.ToInt32(_receiveBuffer, 0);

                if (packetSize <= 0 || packetSize > _receiveBuffer.Length || _socket.Available < packetSize + 4)
                    return null;

                var packet = new byte[packetSize];
                _socket.Receive(_receiveBuffer, 4, SocketFlags.None);
                _socket.Receive(packet, packetSize, SocketFlags.None);

                TotalBytesReceived += packetSize + 4;
                TotalPacketsReceived++;
                return packet;
            }
            catch (SocketException ex)
            {
                OnReceiveError(ex.SocketErrorCode);
                OnSocketError(ex.SocketErrorCode);
                Disconnect();
            }
            catch (ObjectDisposedException)
            {
                OnDisconnected();
            }

            return null;
        }

        private void BeginReceive(int offset)
        {
            if (!IsActive || !IsConnected || _isDisposed)
                return;

            try
            {
                _socket.BeginReceive(_receiveBuffer, offset, _receiveBuffer.Length - offset,
                    SocketFlags.None, OnReceiveCallback, null);
            }
            catch (SocketException ex)
            {
                OnReceiveError(ex.SocketErrorCode);
                OnSocketError(ex.SocketErrorCode);
                Disconnect();
            }
            catch (ObjectDisposedException)
            {
                OnDisconnected();
            }
        }

        private void OnReceiveCallback(IAsyncResult ar)
        {
            if (!IsConnected || _isDisposed)
                return;

            try
            {
                int read = _socket.EndReceive(ar);
                if (read <= 0)
                {
                    Disconnect();
                    return;
                }

                TotalBytesReceived += read;
                _processedBytesInBuffer += read;

                while (_processedBytesInBuffer > 4)
                {
                    int packetSize = BitConverter.ToInt32(_receiveBuffer, 0);

                    if (packetSize <= 0 || packetSize > _receiveBuffer.Length)
                    {
                        OnConnectionError(Network.ConnectionError.BufferLengthTooLong);
                        Disconnect();
                        return;
                    }

                    if (_processedBytesInBuffer < (packetSize + 4))
                        break;

                    var packet = new byte[packetSize];
                    Array.Copy(_receiveBuffer, 4, packet, 0, packetSize);

                    _processedBytesInBuffer -= (packetSize + 4);
                    Array.Copy(_receiveBuffer, packetSize + 4, _receiveBuffer, 0, _processedBytesInBuffer);

                    lock (_receiveQueue) _receiveQueue.Enqueue(packet);

                    TotalPacketsReceived++;
                }

                BeginReceive(_processedBytesInBuffer);
            }
            catch (SocketException ex)
            {
                OnReceiveError(ex.SocketErrorCode);
                OnSocketError(ex.SocketErrorCode);
                Disconnect();
            }
            catch (ObjectDisposedException)
            {
                OnDisconnected();
            }
        }

        public void FlushQueue()
        {
            lock (_receiveQueue)
                _receiveQueue.Clear();
        }

        public byte[]? DequeueReceive()
        {
            lock (_receiveQueue)
                return _receiveQueue.Count > 0 ? _receiveQueue.Dequeue() : null;
        }

        public virtual void Disconnect()
        {
            if (_isDisposed || !IsConnected)
                return;

            Stop();

            try
            {
                if (_socket.Connected)
                    _socket.Shutdown(SocketShutdown.Both);

                _socket.Close();
            }
            catch { }

            _log.Information("{SocketId} disconnected.", Id);
            OnDisconnected();
        }

        private void OnDisconnected() => Disconnected?.Invoke(this, EventArgs.Empty);
        private void OnSocketError(SocketError error) => SocketError?.Invoke(this, new ConnectionErrorEvent(error));
        private void OnConnectionError(ConnectionError error) => ConnectionError?.Invoke(this, new ConnectionErrorEvent(error));
        private void OnSendError(SocketError error) => SendError?.Invoke(this, new ConnectionErrorEvent(error));
        private void OnReceiveError(SocketError error) => ReceiveError?.Invoke(this, new ConnectionErrorEvent(error));

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            if (disposing)
            {
                Stop();

                try
                {
                    if (IsConnected)
                        _socket.Shutdown(SocketShutdown.Both);

                    _socket.Close();
                    _socket.Dispose();
                }
                catch { }

                lock (_receiveQueue)
                    _receiveQueue.Clear();

                try { _sendBuffer.Dispose(); } catch { }

                _log.Information("{SocketId} disposed.", Id);
            }

            base.Dispose(disposing);
        }
    }
}
