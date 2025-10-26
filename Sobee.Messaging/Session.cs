using System.Runtime.Serialization;
using Serilog;
using Sobee.Common;
using Sobee.Network;
using Sobee.Serialization;

namespace Sobee.Messaging
{
    public class Session : Component
    {
        private static readonly ILogger _log = Logging.Get<Session>();
        private bool _isDisposed;

        public event UnhandledExceptionEventHandler? SerializationError;
        public event EventHandler<MessageEventArgs>? MessageReceived;
        public event EventHandler<MessageEventArgs>? MessageSent;

        public SocketWrapper? Socket;

        public bool IsActive { get; private set; }
        public bool IsConnected => Socket != null && Socket.IsConnected;

        private readonly MessageDispatch _dispatcher;
        private readonly MessageHelper _receiveHelper;
        private readonly MessageHelper _sendHelper;

        private readonly MemoryStream _receiveBufferStream = new(SocketWrapper.MAX_RECEIVE_SIZE);
        private readonly MemoryStream _sendBufferStream = new(SocketWrapper.MAX_SEND_SIZE);

        public SessionType SessionType { get; protected set; }

        public Session(SocketWrapper socket, MessageDispatch dispatcher)
        {
            Socket = socket ?? throw new ArgumentNullException(nameof(socket));
            _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));

            _sendHelper = new MessageHelper(_sendBufferStream, _dispatcher.GetDispatcher(), _dispatcher.GetMessageTypeToIdDelegate());
            _receiveHelper = new MessageHelper(_receiveBufferStream, _dispatcher.GetDispatcher(), _dispatcher.GetMessageTypeToIdDelegate());

            _log.Information("{thisId} initialized. (source: {socketId})", this.Id, Socket.Id);
        }

        public virtual void Start()
        {
            if (!IsConnected || IsActive)
                return;

            IsActive = true;
            Socket?.Start();

            _log.Information("{id} started.", Id);
        }

        public virtual void Stop()
        {
            if (!IsActive)
                return;

            IsActive = false;
            Socket?.Stop();

            _log.Information("{id} stopped.", Id);
        }

        public override Task Update(double delta)
        {
            if (_isDisposed || !IsActive || !IsConnected)
                return Task.CompletedTask;

            try
            {
                while (true)
                {
                    var packetData = Socket?.DequeueReceive();
                    if (packetData == null)
                        break;

                    _receiveBufferStream.Position = 0;
                    _receiveBufferStream.SetLength(0);
                    _receiveBufferStream.Write(packetData, 0, packetData.Length);
                    _receiveBufferStream.Position = 0;

                    var message = (Message)_receiveHelper.ReadMessage();
                    OnMessageReceived(this, message);
                    _dispatcher.DispatchToMessageEvent(new MessageEventArgs(this, message));
                }
            }
            catch (SerializationException ex)
            {
                OnSerializationError(ex);
                Disconnect();
            }
            catch (Exception ex)
            {
                _log.Error(ex, "{id} unexpected receive error", Id);
                Disconnect();
            }

            Socket?.SendSync();
            return Task.CompletedTask;
        }

        public virtual void Disconnect()
        {
            if (_isDisposed || !IsConnected)
                return;

            Stop();
            Socket?.Disconnect();

            _log.Information("{id} disconnected.", Id);
        }

        public virtual void SendMessage(Message message)
        {
            if (_isDisposed || !IsConnected)
                return;

            _sendBufferStream.Position = 0;
            _sendBufferStream.SetLength(0);
            _sendHelper.WriteMessage(message);

            Socket?.EnqueueSend(_sendBufferStream.GetBuffer(), 0, (int)_sendBufferStream.Length);
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
            if (_isDisposed)
                return;

            _isDisposed = true;

            if (disposing)
            {
                Stop();

                try
                {
                    // TODO: Socketi session ile dispose etmek riskli çünkü socket başka bir session içinde kullanılabilir.
                    // ORNEK: AuthUser, MatchUser'e geçerken aynı socketi kullanmak zorunda.

                    if (!IsConnected)
                    {
                        Socket?.Dispose();
                        _log.Information("{id} socket disposed. (isConnected:{isConnected})", Id, IsConnected);
                    }
                    else
                        _log.Warning("{id} socket NOT disposed! (isConnected:{isConnected})", Id, IsConnected);

                    Socket = null;

                    _receiveBufferStream.Dispose();
                    _sendBufferStream.Dispose();
                    // _dispatcher.Dispose(); // WARN: Dispatcher artık odalara bağlı!!!!
                }
                catch { }

                _log.Debug("{id} disposed.", Id);
            }

            base.Dispose(disposing);
        }
    }
}
