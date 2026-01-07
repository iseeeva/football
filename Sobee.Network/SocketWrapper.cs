using System.Net.Sockets;
using Sobee.Common;

namespace Sobee.Network
{
    public enum SocketState
    {
        Stopped,
        Running,
        Paused
    }

    public class SocketWrapper : Component
    {
        private static readonly Serilog.ILogger _log = Logging.Get<SocketWrapper>();
        private bool _isDisposed;

        public static readonly int MAX_SEND_SIZE = 32768;
        public static readonly int MAX_RECEIVE_SIZE = 4096;

        private readonly Socket _socket;
        public bool IsConnected => _socket != null && _socket.Connected;
        public bool IsRunning => _socket != null && _socketState == SocketState.Running;

        private readonly object _socketStateLock = new();
        private SocketState _socketState = SocketState.Stopped;
        private bool _isReceivePending = false;

        private readonly MemoryStream _sendBuffer = new MemoryStream(MAX_SEND_SIZE);
        private readonly byte[] _receiveBuffer = new byte[MAX_RECEIVE_SIZE];
        private readonly Queue<byte[]> _receiveQueue = new Queue<byte[]>();
        private int _receiveBufferOffset;

        public long TotalSentBytes { get; private set; }
        public long TotalReceivedBytes { get; private set; }
        public long TotalPacketsSent { get; private set; }
        public long TotalPacketsReceived { get; private set; }

        public EventHandler? DisconnectHandler;
        public EventHandler<ConnectionErrorEvent>? ErrorHandler;

        public SocketWrapper(Socket socket)
        {
            _socket = socket;
            _log.Information("{id} initialized. (socket:{socketIp})", Id, _socket.RemoteEndPoint);
        }

        public void Start()
        {
            lock (_socketStateLock)
            {
                if (_socketState == SocketState.Running)
                    return;

                _socketState = SocketState.Running;
                if (!_isReceivePending)
                    BeginReceiveAsync(_receiveBufferOffset);
            }

            _log.Information("{id} started.", Id, _socket.RemoteEndPoint);
        }

        public void Pause(bool isFlushNeeded = false)
        {
            lock (_socketStateLock)
            {
                if (_socketState == SocketState.Paused)
                    return;

                _socketState = SocketState.Paused;
                if (isFlushNeeded)
                    FlushBuffers();
            }

            _log.Information("{id} paused. (isFlushNeeded:{flushBool})", Id, isFlushNeeded);
        }

        public void Stop()
        {
            lock (_socketStateLock)
            {
                if (_socketState == SocketState.Stopped)
                    return;

                _socketState = SocketState.Stopped;
                Disconnect();
            }

            _log.Information("{id} stopped.", Id);
        }

        public virtual void QueueSend(byte[] buffer, int offset, int length)
        {
            lock (_sendBuffer)
            {
                _sendBuffer.Write(BitConverter.GetBytes(length), 0, 4);
                _sendBuffer.Write(buffer, offset, length);
                TotalPacketsSent++;
            }
        }

        public long BeginSendBuffered()
        {
            if (_socket == null || !IsConnected)
                return 0;

            byte[] dataToSend;
            lock (_sendBuffer)
            {
                if (_sendBuffer.Length == 0)
                    return 0;

                dataToSend = _sendBuffer.ToArray();
                _sendBuffer.SetLength(0);
                _sendBuffer.Position = 0;
            }

            try
            {
                _socket.BeginSend(dataToSend, 0, dataToSend.Length, SocketFlags.None, EndSendCallback, _socket);
                return dataToSend.Length;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return 0;
            }
        }

        private void EndSendCallback(IAsyncResult ar)
        {
            try { TotalSentBytes += ((Socket)ar.AsyncState!).EndSend(ar); }
            catch { Disconnect(); }
        }

        private void BeginReceiveAsync(int offset)
        {
            if (_socket == null || _isDisposed)
                return;

            lock (_socketStateLock)
            {
                if (_socketState == SocketState.Stopped)
                    return;

                _isReceivePending = true;
            }

            try
            {
                _socket.BeginReceive(_receiveBuffer, offset, _receiveBuffer.Length - offset, SocketFlags.None, EndReceiveCallback, _socket);
            }
            catch (Exception ex)
            {
                lock (_socketStateLock)
                    _isReceivePending = false;

                HandleException(ex);
            }
        }

        private void EndReceiveCallback(IAsyncResult ar)
        {
            lock (_socketStateLock)
                _isReceivePending = false;

            try
            {
                int received = ((Socket)ar.AsyncState!).EndReceive(ar);
                if (received <= 0)
                {
                    Disconnect();
                    return;
                }

                _receiveBufferOffset += received;
                TotalReceivedBytes += received;

                ProcessBuffer();
                BeginReceiveAsync(_receiveBufferOffset);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ProcessBuffer()
        {
            while (_receiveBufferOffset > 4)
            {
                int packetLength = BitConverter.ToInt32(_receiveBuffer, 0);
                if (packetLength <= 0 || packetLength > MAX_RECEIVE_SIZE)
                {
                    OnConnectionError(ConnectionError.BufferLengthTooLong);
                    Disconnect(); return;
                }

                int fullLength = 4 + packetLength;
                if (_receiveBufferOffset < fullLength) break;

                bool isRunning;
                lock (_socketStateLock)
                {
                    isRunning =
                        (_socketState == SocketState.Running);
                }

                // Sadece socket calisiyorsa (SocketState.Running) paketi kuyruga ekleyecek.
                if (isRunning)
                {
                    byte[] packet = new byte[packetLength];
                    Array.Copy(_receiveBuffer, 4, packet, 0, packetLength);
                    lock (_receiveQueue) { _receiveQueue.Enqueue(packet); TotalPacketsReceived++; }
                }

                _receiveBufferOffset -= fullLength;
                if (_receiveBufferOffset > 0)
                    Array.Copy(_receiveBuffer, fullLength, _receiveBuffer, 0, _receiveBufferOffset);
            }
        }

        public byte[]? DequeuePacket()
        {
            lock (_receiveQueue)
                return _receiveQueue.Count == 0 ?
                    null :
                    _receiveQueue.Dequeue();
        }

        public void Disconnect()
        {
            lock (_socketStateLock)
            {
                if (!IsConnected)
                    return;

                // stop, socketStatei stopped yapıyor ama ya disconnecti cagiran stop degilse?
                _socketState = SocketState.Stopped;

                if (_socket != null && _socket.Connected)
                    _socket.Close();

                FlushBuffers();

                _log.Information("{id} disconnected.", Id);
                OnDisconnected();
            }
        }

        protected virtual void OnDisconnected() => DisconnectHandler?.Invoke(this, EventArgs.Empty);
        protected virtual void OnSocketError(SocketError error) => ErrorHandler?.Invoke(this, new ConnectionErrorEvent(error));
        protected virtual void OnConnectionError(ConnectionError error) => ErrorHandler?.Invoke(this, new ConnectionErrorEvent(error));

        public void FlushBuffers()
        {
            lock (_sendBuffer)
            {
                _sendBuffer.SetLength(0);
                _sendBuffer.Position = 0;
            }

            lock (_receiveQueue)
                _receiveQueue.Clear();
        }

        private void HandleException(Exception ex)
        {
            if (ex is SocketException se)
                OnSocketError(se.SocketErrorCode);

            Disconnect();
        }

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            lock (_socketStateLock)
            {
                _socketState = SocketState.Stopped;
            }

            _isDisposed = true;

            if (disposing)
            {
                try
                {
                    if (_socket != null)
                    {
                        if (_socket.Connected)
                            _socket.Shutdown(SocketShutdown.Both);

                        _socket.Close();
                        _socket.Dispose();
                    }

                    FlushBuffers();
                    _log.Information("{id} disposed.", Id);
                }
                catch (Exception ex)
                {
                    _log.Error(ex, "{id} error during disposal", Id);
                }
            }

            base.Dispose(disposing);
        }
    }
}