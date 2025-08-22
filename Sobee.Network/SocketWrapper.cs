using System.Net;
using System.Net.Sockets;
using Sobee.Common;

namespace Sobee.Network
{
    public class SocketWrapper : Component
    {
        private bool _isDisposed;
        private Socket _socket;

        public static readonly int MAX_SEND_SIZE = 32768;
        public static readonly int MAX_RECEIVE_SIZE = 4096;

        private readonly MemoryStream _sendBuffer = new(MAX_SEND_SIZE);
        private readonly Queue<byte[]> _receiveQueue = new();
        private readonly byte[] _receiveBuffer = new byte[MAX_RECEIVE_SIZE];
        private int _processedBytesInBuffer;

        public event EventHandler Disconnected;
        public event EventHandler<GEventArgs10> SocketError;
        public event EventHandler<GEventArgs10> ConnectionError;
        public event EventHandler<GEventArgs10> SendError;
        public event EventHandler<GEventArgs10> ReceiveError;

        public long TotalBytesSent { get; private set; }
        public long TotalBytesReceived { get; private set; }
        public long TotalPacketsSent { get; private set; }
        public long TotalPacketsReceived { get; private set; }

        public bool IsConnected => _socket?.Connected ?? false;

        public EndPoint? RemoteEndPoint => _socket?.RemoteEndPoint;

        public SocketWrapper() { }
        public SocketWrapper(Socket socket) => this._socket = socket;

        public void EnqueueSend(byte[] data, int offset, int count)
        {
            lock (_sendBuffer)
            {
                _sendBuffer.Write(BitConverter.GetBytes(count));
                _sendBuffer.Write(data, offset, count);
                TotalPacketsSent++;
            }
        }

        public int SendSync()
        {
            if (_socket == null) return 0;

            lock (_sendBuffer)
            {
                try
                {
                    if (_sendBuffer.Length == 0) return 0;
                    int sent = _socket.Send(_sendBuffer.GetBuffer(), 0, (int)_sendBuffer.Length, SocketFlags.None);
                    TotalBytesSent += sent;
                    _sendBuffer.SetLength(0);
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

        public byte[] ReceiveSync()
        {
            if (_socket == null || !_socket.Connected) { Disconnect(); return null; }
            try
            {
                if (!_socket.Poll(0, SelectMode.SelectRead) || _socket.Available < 4) return null;

                _socket.Receive(_receiveBuffer, 4, SocketFlags.Peek);
                int packetSize = BitConverter.ToInt32(_receiveBuffer, 0);

                if (packetSize <= 0 || packetSize > _receiveBuffer.Length || _socket.Available < 4 + packetSize) return null;

                var packet = new byte[packetSize];
                _socket.Receive(_receiveBuffer, 4, SocketFlags.None);
                _socket.Receive(packet, packetSize, SocketFlags.None);

                TotalBytesReceived += 4 + packetSize;
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

        public void BeginReceive() => BeginReceive(_processedBytesInBuffer);

        private void BeginReceive(int offset)
        {
            if (_socket == null) return;
            try
            {
                _socket.BeginReceive(_receiveBuffer, offset, _receiveBuffer.Length - offset, SocketFlags.None, OnReceiveCallback, null);
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
            try
            {
                int read = _socket.EndReceive(ar);
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

                    if (_processedBytesInBuffer < 4 + packetSize) break;

                    var packet = new byte[packetSize];
                    Array.Copy(_receiveBuffer, 4, packet, 0, packetSize);
                    Array.Copy(_receiveBuffer, 4 + packetSize, _receiveBuffer, 0, _processedBytesInBuffer -= 4 + packetSize);

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

        public byte[] DequeueReceive()
        {
            lock (_receiveQueue)
                return _receiveQueue.Count > 0 ? _receiveQueue.Dequeue() : null;
        }

        public virtual void Disconnect()
        {
            _socket?.Close();
            _socket = null;
            OnDisconnected();
        }

        private void OnDisconnected() => Disconnected?.Invoke(this, EventArgs.Empty);
        private void OnSocketError(SocketError error) => SocketError?.Invoke(this, new GEventArgs10(error));
        private void OnConnectionError(ConnectionError error) => ConnectionError?.Invoke(this, new GEventArgs10(error));
        private void OnSendError(SocketError error) => SendError?.Invoke(this, new GEventArgs10(error));
        private void OnReceiveError(SocketError error) => ReceiveError?.Invoke(this, new GEventArgs10(error));

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                if (disposing)
                {
                    Disconnect();
                }
            }

            base.Dispose(disposing);
        }
    }
}
