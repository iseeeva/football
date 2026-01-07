using System.Runtime.Serialization;
using Serilog;
using Sobee.Common;
using Sobee.Network.Messaging;
using Sobee.Serialization;

namespace Sobee.Network
{
    public class Session : Component
    {
        private static readonly ILogger _log = Logging.Get<Session>();
        private bool _isDisposed;

        public SocketWrapper? Socket;
        public bool IsConnected => Socket != null && Socket.IsConnected;
        public bool IsRunning => Socket != null && Socket.IsRunning;

        public SessionType SessionType { get; protected set; }

        private readonly MessageDispatch _dispatcher;
        private readonly MessageSerialization _receiveSerialization;
        private readonly MessageSerialization _sendSerialization;

        private readonly MemoryStream _receiveStream = new(SocketWrapper.MAX_RECEIVE_SIZE);
        private readonly MemoryStream _sendStream = new(SocketWrapper.MAX_SEND_SIZE);

        public event EventHandler<Message>? MessageReceivedHandler;
        public event EventHandler<Message>? MessageSendHandler;

        public Session(SocketWrapper socket, MessageDispatch dispatcher)
        {
            Socket = socket ?? throw new ArgumentNullException(nameof(socket));
            _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));

            _sendSerialization = new MessageSerialization(
                _sendStream,
                _dispatcher.DispatchToMessageConstructor(),
                _dispatcher.GetMessageIdFromType());

            _receiveSerialization = new MessageSerialization(
                _receiveStream,
                _dispatcher.DispatchToMessageConstructor(),
                _dispatcher.GetMessageIdFromType());

            _log.Information("{id} initialized. (socket:{socketId})", Id, Socket.Id);
        }

        public void Start()
        {
            if (IsRunning || !IsConnected)
                return;

            Socket!.Start();
            _log.Information("{id} started.", Id);
        }

        public void Pause(bool isFlushNeeded = false)
        {
            if (!IsRunning)
                return;

            Socket!.Pause(isFlushNeeded);
            _log.Information("{id} paused.", Id);
        }

        public void Stop()
        {
            if (!IsRunning)
                return;

            Socket!.Stop();
            _log.Information("{id} stopped.", Id);
        }

        public override void Update(double delta)
        {
            if (_isDisposed || !IsRunning || !IsConnected)
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
                    _dispatcher.DispatchToMessageEvent(
                        new MessageEventArgs(this, message)
                    );

                    MessageReceivedHandler?.Invoke(this, message);
                }
            }
            catch (SerializationException ex)
            {
                _log.Error(ex, "{id} serialization error", Id);
                Disconnect();
            }
            catch (Exception ex)
            {
                _log.Error(ex, "{id} unexpected session error", Id);
                Disconnect();
            }

            Socket?.BeginSendBuffered();
        }

        /// <summary> Belirli turdeki mesaji 1 kere bekler </summary>
        public void WaitForMessage<T>(Action<Session, T> handler) where T : Message
        {
            EventHandler<Message>? wrapper = null;
            wrapper = (sender, message) =>
            {
                if (message is not T msg)
                    return;

                MessageReceivedHandler -= wrapper;

                handler(this, msg);
            };

            MessageReceivedHandler += wrapper;
        }

        public void SendMessage(Message message)
        {
            if (_isDisposed || !IsConnected)
                return;

            _sendStream.Position = 0;
            _sendStream.SetLength(0);
            _sendSerialization.WriteMessage(message);

            Socket?.QueueSend(
                _sendStream.GetBuffer(),
                0,
                (int)_sendStream.Length
            );
        }

        public void Disconnect()
        {
            if (_isDisposed || !IsConnected)
                return;

            Stop();
            Socket?.Disconnect();

            _log.Information("{id} disconnected.", Id);
        }

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            if (disposing)
            {
                try
                {
                    // WARN: Socketi session ile dispose etmek riskli çünkü socket başka bir session içinde kullanılabilir.
                    // ORNEK: AuthUser, MatchPlayer'a geçerken aynı socketi kullanmak zorunda.
                    Pause(true);

                    if (!IsConnected)
                    {
                        Socket?.Dispose();
                        _log.Information("{id} socket disposed. (isConnected:{isConnected})", Id, IsConnected);
                    }
                    else
                        _log.Warning("{id} socket NOT disposed! (isConnected:{isConnected})", Id, IsConnected);

                    // WARN: Artik dispatcher session'a degil odaya bagli.
                    // Session, dispatcher dispose EDEMEZ.

                    // Socket
                    Socket = null;

                    // Serializations
                    _sendSerialization.Dispose();
                    _receiveSerialization.Dispose();

                    _log.Debug("{id} disposed.", Id);
                }
                catch (Exception ex)
                {
                    _log.Error(ex, "{id} dispose error", Id);
                }
            }

            base.Dispose(disposing);
        }
    }
}
