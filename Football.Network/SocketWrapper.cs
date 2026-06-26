using Football.Common;
using System.Net.Sockets;
namespace Football.Network
{
    public class SocketWrapper : Component
    {
        private readonly Socket _socket;
        public bool IsConnected => _socket != null && _socket.Connected;

        public static readonly int MAX_RECEIVE_SIZE = 32768;
        public static readonly int MAX_SEND_SIZE = 4096;

        private readonly object _socketLock = new();
        private readonly object _receiveLock = new();
        private bool _isReceiving = false;

        private readonly MemoryStream _sendBuffer = new(MAX_SEND_SIZE);
        private readonly byte[] _receiveBuffer = new byte[MAX_RECEIVE_SIZE];
        private readonly Queue<byte[]> _receiveQueue = new();
        private int _receiveBufferOffset;

        public long TotalSentBytes { get; private set; }
        public long TotalReceivedBytes { get; private set; }
        public long TotalPacketsSent { get; private set; }
        public long TotalPacketsReceived { get; private set; }

        public EventHandler? DisconnectHandler;
        public EventHandler<ConnectionErrorEvent>? ErrorHandler;

        #region Constructor
        public SocketWrapper(Socket socket)
        {
            // WARN: Receive'i durdurup yeniden calistirmayi denemeyi birak.
            // Client buna uygun sekilde mesaj gondermiyor ve sende takip edemezsin.
            // Aklima gelen en uygun yol socket baglandiktan itibaren surekli kanali acik tutmak.

            _socket = socket;
            _log.Information("remoteEndPoint:{ep}", _socket.RemoteEndPoint);
        }
        #endregion

        #region Lifecycle
        protected override void OnStart()
        {
            if (!IsConnected)
                return;

            FlushBuffers();
            BeginReceive(0);
        }

        protected override void OnStop()
        {

        }
        #endregion

        #region Send
        public virtual void EnqueuePacket(byte[] buffer, int offset, int length)
        {
            if (length <= 0) return;
            if (!IsConnected) return;

            lock (_sendBuffer)
            {
                if (_sendBuffer.Length + length + 4 > MAX_SEND_SIZE)
                {
                    _log.Warning("send buffer full, dropping packet. (pending:{pending}b)", _sendBuffer.Length);
                    return;
                }

                byte[] lengthPrefix = BitConverter.GetBytes(length);
                _sendBuffer.Write(lengthPrefix, 0, lengthPrefix.Length);
                _sendBuffer.Write(buffer, offset, length);
                TotalPacketsSent++;
            }
        }

        public long BeginSendBuffered()
        {
            if (_socket == null || !IsConnected) return 0;

            byte[] dataToSend;
            lock (_sendBuffer)
            {
                if (_sendBuffer.Length <= 0) return 0;
                dataToSend = _sendBuffer.ToArray();
                _sendBuffer.SetLength(0);
                _sendBuffer.Position = 0;
            }

            try
            {
                int totalSent = 0;
                while (totalSent < dataToSend.Length)
                {
                    int sent = _socket.Send(dataToSend, totalSent, dataToSend.Length - totalSent, SocketFlags.None);
                    if (sent <= 0) break;
                    totalSent += sent;
                    TotalSentBytes += sent;
                }
                return totalSent;
            }
            catch (SocketException ex)
            {
                OnSocketError(ex.SocketErrorCode);
                Disconnect();
            }
            catch (ObjectDisposedException)
            {
                OnDisconnected();
            }

            return 0;
        }

        public virtual void QueueSend(byte[] buffer, int offset, int length)
            => EnqueuePacket(buffer, offset, length);
        #endregion

        #region Receive
        private void BeginReceive(int offset)
        {
            lock (_receiveLock)
            {
                if (_isReceiving)
                {
                    _log.Warning("attempted start BeginReceive while receiving data.");
                    return;
                }

                _isReceiving = true;
            }

            if (_socket == null) return;

            try
            {
                _socket.BeginReceive(
                    _receiveBuffer, offset,
                    _receiveBuffer.Length - offset,
                    SocketFlags.None,
                    OnReceiveComplete,
                    _socket);
            }
            catch (SocketException ex)
            {
                lock (_receiveLock)
                    _isReceiving = false;
                OnSocketError(ex.SocketErrorCode);
                Disconnect();
            }
            catch (ObjectDisposedException)
            {
                lock (_receiveLock)
                    _isReceiving = false;
                OnDisconnected();
            }
        }

        private void OnReceiveComplete(IAsyncResult ar)
        {
            try
            {
                Socket socket = (Socket)ar.AsyncState!;
                int received = socket.EndReceive(ar);

                lock (_receiveLock)
                    _isReceiving = false;

                if (received <= 0)
                {
                    Disconnect();
                    return;
                }

                int nextOffset;
                lock (_receiveLock)
                {
                    TotalReceivedBytes += received;
                    _receiveBufferOffset += received;

                    while (_receiveBufferOffset > 4)
                    {
                        int packetLength = BitConverter.ToInt32(_receiveBuffer, 0);

                        if (packetLength <= 0 || packetLength > _receiveBuffer.Length)
                        {
                            _log.Warning("invalid packetLength={packetLength}, dump={dump}",
                            packetLength, BitConverter.ToString(_receiveBuffer, 0, Math.Min(16, _receiveBufferOffset)));
                            OnConnectionError(ConnectionError.BufferLengthTooLong);
                            Disconnect();
                            return;
                        }

                        int fullLength = 4 + packetLength;
                        if (fullLength > _receiveBuffer.Length)
                        {
                            OnConnectionError(ConnectionError.BufferLengthTooLong);
                            Disconnect();
                            return;
                        }

                        if (_receiveBufferOffset < fullLength) break;

                        _receiveBufferOffset -= fullLength;

                        byte[] packet = new byte[packetLength];
                        Array.Copy(_receiveBuffer, 4, packet, 0, packetLength);
                        Array.Copy(_receiveBuffer, fullLength, _receiveBuffer, 0, _receiveBufferOffset);

                        lock (_receiveQueue)
                        {
                            _receiveQueue.Enqueue(packet);
                            TotalPacketsReceived++;
                        }
                    }

                    nextOffset = _receiveBufferOffset;
                }

                BeginReceive(nextOffset);
            }
            catch (SocketException ex)
            {
                lock (_receiveLock)
                    _isReceiving = false;
                OnSocketError(ex.SocketErrorCode);
                Disconnect();
            }
            catch (ObjectDisposedException)
            {
                lock (_receiveLock)
                    _isReceiving = false;
                OnDisconnected();
            }
        }

        public byte[]? DequeuePacket()
        {
            lock (_receiveQueue)
                return _receiveQueue.Count == 0 ? null : _receiveQueue.Dequeue();
        }
        #endregion

        #region Disconnect
        public void Disconnect()
        {
            lock (_socketLock)
            {
                if (!IsConnected) return;
                if (_socket != null && _socket.Connected)
                {
                    try { _socket.Shutdown(SocketShutdown.Both); } catch { }
                    _socket.Close();
                }
            }

            FlushBuffers();
            _log.Information("disconnected.");
            OnDisconnected();
        }

        public void FlushBuffers()
        {
            lock (_sendBuffer)
            {
                _sendBuffer.SetLength(0);
                _sendBuffer.Position = 0;
            }

            lock (_receiveQueue)
                _receiveQueue.Clear();

            lock (_receiveLock)
            {
                Array.Clear(_receiveBuffer, 0, _receiveBuffer.Length);
                _receiveBufferOffset = 0;
            }
        }

        protected virtual void OnDisconnected() => DisconnectHandler?.Invoke(this, EventArgs.Empty);
        protected virtual void OnSocketError(SocketError error) => ErrorHandler?.Invoke(this, new ConnectionErrorEvent(error));
        protected virtual void OnConnectionError(ConnectionError error) => ErrorHandler?.Invoke(this, new ConnectionErrorEvent(error));
        #endregion

        #region Dispose
        protected override void OnDispose()
        {
            try
            {
                lock (_socketLock)
                {
                    if (_socket != null)
                    {
                        if (_socket.Connected)
                            try { _socket.Shutdown(SocketShutdown.Both); } catch { }

                        _socket.Close();
                        _socket.Dispose();
                    }
                }

                FlushBuffers();
            }
            catch (Exception ex)
            {
                _log.Error(ex, "error during disposal.");
            }
        }
        #endregion
    }
}