using Football.Common;
using Football.Network.Messaging;
using Football.Serialization;
using System.Runtime.Serialization;

namespace Football.Network
{
    public class Session : ComponentManager<IComponent>
    {
        public SocketWrapper? Socket;
        public bool IsConnected => Socket != null && Socket.IsConnected;

        public SessionType SessionType { get; protected set; }
        private readonly MessageCommunication _communication;

        private readonly MemoryStream _receiveStream = new(SocketWrapper.MAX_RECEIVE_SIZE);
        private readonly MessageSerialization _receiveSerialization;

        private readonly MemoryStream _sendStream = new(SocketWrapper.MAX_SEND_SIZE);
        private readonly MessageSerialization _sendSerialization;

        public event EventHandler<Message>? MessageReceivedHandler;
        public event EventHandler<Message>? MessageSendHandler;

        #region Constructor
        public Session(SocketWrapper socket, MessageCommunication communication)
        {
            Socket = socket ?? throw new ArgumentNullException(nameof(socket));
            AddComponent(socket);

            ArgumentNullException.ThrowIfNull(communication);
            _communication = communication;

            _sendSerialization = new MessageSerialization(
                _sendStream,
                _communication.GetMessageConstructor(),
                _communication.GetMessageIdFromType());

            _receiveSerialization = new MessageSerialization(
                _receiveStream,
                _communication.GetMessageConstructor(),
                _communication.GetMessageIdFromType());
        }
        #endregion

        #region Lifecycle
        protected override void OnUpdate(double delta)
        {
            if (!IsConnected)
                return;

            try
            {
                while (Socket?.DequeuePacket() is byte[] packet)
                {
                    _receiveStream.Position = 0;
                    _receiveStream.SetLength(0);
                    _receiveStream.Write(packet);
                    _receiveStream.Position = 0;

                    var message = (Message)_receiveSerialization.ReadMessage();
                    _communication.DispatchToMessageEvent(new MessageEventArgs(this, message));

                    MessageReceivedHandler?.Invoke(this, message);
                }
            }
            catch (SerializationException ex)
            {
                _log.Error(ex, "serialization error");
                Disconnect();
            }
            catch (Exception ex)
            {
                _log.Error(ex, "unexpected session error");
                Disconnect();
            }

            Socket?.BeginSendBuffered();
        }
        #endregion

        #region Messaging
        public void SendMessage(Message message)
        {
            if (!IsConnected)
                return;

            _sendStream.Position = 0;
            _sendStream.SetLength(0);
            _sendSerialization.WriteMessage(message);

            byte[] data = new byte[_sendStream.Length];
            Buffer.BlockCopy(_sendStream.GetBuffer(), 0, data, 0, data.Length);

            Socket?.QueueSend(data, 0, data.Length);
            MessageSendHandler?.Invoke(this, message);
        }
        #endregion

        #region Disconnect / Dispose
        public void Disconnect()
        {
            if (!IsConnected)
                return;

            Stop();
            Socket?.Disconnect();
        }

        protected override void OnDispose()
        {
            try
            {
                Stop();
                _communication.RemoveSessionMessageHandlers(this);

                if (Socket != null)
                {
                    RemoveComponent(Socket.Id, IsConnected);
                    _log.Warning("socket {status} disposed! (isConnected:{isConnected})", IsConnected ? "NOT" : "", IsConnected);
                }

                Socket = null;

                _sendSerialization.Dispose();
                _receiveSerialization.Dispose();
            }
            catch (Exception ex)
            {
                _log.Error(ex, "dispose error");
            }

            base.OnDispose();
        }
        #endregion
    }
}