using System.Runtime.Serialization;
using Sobee.Common;
using Sobee.Network;
using Sobee.Serialization;

namespace Sobee.Messaging
{
    public class Session : Component
    {
        public event UnhandledExceptionEventHandler SerializationError;
        public event EventHandler<MessageEventArgs> MessageReceived;
        public event EventHandler<MessageEventArgs> MessageSent;

        public bool IsActive { get; private set; }
        private bool _isDisposed;

        private readonly SocketWrapper _socket;
        private readonly MessageDispatch _dispatcher;
        private readonly MessageHelper _receiveHelper;
        private readonly MessageHelper _sendHelper;

        private static readonly MemoryStream _receiveBufferStream = new MemoryStream(SocketWrapper.MAX_RECEIVE_SIZE);
        private readonly MemoryStream _sendBufferStream = new MemoryStream(SocketWrapper.MAX_SEND_SIZE);

        public SessionType SessionType => _dispatcher.SessionType;

        public Session(SocketWrapper socket, MessageDispatch dispatcher)
        {
            _socket = socket ?? throw new ArgumentNullException(nameof(socket));
            _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));

            _sendHelper = new MessageHelper(_sendBufferStream, _dispatcher.GetDispatcher(), _dispatcher.GetMessageTypeToIdDelegate());
            _receiveHelper = new MessageHelper(_receiveBufferStream, _dispatcher.GetDispatcher(), _dispatcher.GetMessageTypeToIdDelegate());
        }

        public virtual void Start()
        {
            if (_socket.IsConnected)
            {
                _socket.BeginReceive();
                IsActive = true;
            }
        }

        public override Task Update(double delta)
        {
            if (_socket.IsConnected)
            {
                Message receivedMessage = null;
                try
                {
                    while (true)
                    {
                        var packetData = _socket.DequeueReceive();
                        if (packetData == null) break;

                        _receiveBufferStream.Position = 0;
                        _receiveBufferStream.SetLength(0);
                        _receiveBufferStream.Write(packetData, 0, packetData.Length);
                        _receiveBufferStream.Position = 0;

                        receivedMessage = (Message)_receiveHelper.ReadMessage();
                        OnMessageReceived(this, receivedMessage);
                        _dispatcher.DispatchToMessageEvent(new MessageEventArgs(this, receivedMessage));
                    }
                }
                catch (SerializationException ex)
                {
                    OnSerializationError(ex);
                    Disconnect();
                }
                catch
                {
                    Disconnect();
                }
            }

            _socket.SendSync();
            return Task.CompletedTask;
        }

        public void Disconnect()
        {
            if (IsActive)
            {
                _socket.Disconnect();
                IsActive = false;
            }
        }

        public virtual void SendMessage(Message message)
        {
            _sendBufferStream.Position = 0;
            _sendBufferStream.SetLength(0);
            _sendHelper.WriteMessage(message);
            _socket.EnqueueSend(_sendBufferStream.GetBuffer(), 0, (int)_sendBufferStream.Length);
            OnMessageSent(this, message);
        }

        protected virtual void OnSerializationError(SerializationException ex)
        {
            SerializationError?.Invoke(this, new UnhandledExceptionEventArgs(ex, false));
        }

        protected virtual void OnMessageReceived(object sender, Message message)
        {
            MessageReceived?.Invoke(this, new MessageEventArgs(this, message));
        }

        protected virtual void OnMessageSent(object sender, Message message)
        {
            MessageSent?.Invoke(this, new MessageEventArgs(this, message));
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                if (disposing)
                {
                    _receiveBufferStream?.Dispose();
                    _sendBufferStream?.Dispose();
                    _dispatcher?.Dispose();
                    Disconnect();
                }
            }

            base.Dispose(disposing);
        }
    }
}
