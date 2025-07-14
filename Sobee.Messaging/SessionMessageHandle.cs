using Serilog;
using Sobee.Common;
using Sobee.Network;

namespace Sobee.Messaging
{
    public class SessionMessageHandle : Component
    {
        private readonly ILogger _log = Logging.Get<SessionMessageHandle>();
        private bool _isDisposed;

        private MessageDispatch? _dispatcher;

        private readonly SessionQueueHandle _queue;
        private readonly Session _session;

        private MessageHelper? _receiveHelper;
        private readonly MemoryStream _receiveStream = new MemoryStream(SessionQueueHandle.MaxReceivingSize);

        private MessageHelper? _sendHelper;
        private readonly MemoryStream _sendStream = new MemoryStream(SessionQueueHandle.MaxSendingSize);

        public SessionMessageHandle(SessionQueueHandle queueHandle, Session session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
            _queue = queueHandle;

            _log.Debug("{id} initialized.", this.Id);
        }

        public override Task Update()
        {
            if (!_session.IsConnected) return Task.CompletedTask;

            try
            {
                ProcessIncomingMessages();
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Error during update.");
                this.Dispose();
                throw;
            }

            return Task.CompletedTask;
        }

        public virtual void SendMessage(Message message)
        {
            ArgumentNullException.ThrowIfNull(message);

            try
            {
                if (_sendHelper == null)
                    throw new InvalidOperationException($"{nameof(_sendHelper)} is not initialized.");

                PrepareStream(_sendStream);
                _sendHelper.WriteMessage(message);
                _queue.EnqueueSendData(_sendStream.GetBuffer());
                message.byteLength = (int)_sendStream.Length;
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Failed to send message.");
                throw;
            }
        }

        public virtual void SetDispatchSource(MessageDispatch dispatcher)
        {
            _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
            _receiveHelper = new MessageHelper(_receiveStream, dispatcher.GetDispatcher(), dispatcher.GetMessageTypeToIdDelegate());
            _sendHelper = new MessageHelper(_sendStream, dispatcher.GetDispatcher(), dispatcher.GetMessageTypeToIdDelegate());
        }

        private void ProcessIncomingMessages()
        {
            byte[]? array;
            while ((array = _queue.DequeueReceiveData()) != null)
            {
                try
                {
                    if (_receiveHelper == null)
                        throw new InvalidOperationException($"{nameof(_receiveHelper)} is not initialized.");

                    PrepareStream(_receiveStream, array);
                    var message = (Message)_receiveHelper.ReadMessage();
                    message.byteLength = array.Length;

                    if (_dispatcher == null)
                        throw new InvalidOperationException($"{nameof(_dispatcher)} is not initialized.");

                    _dispatcher.DispatchToMessageEvent(new MessageDelegateArgs(_dispatcher.Owner, new MessageEventArgs(_session, message)));
                }
                catch (Exception ex)
                {
                    _log.Error(ex, "Failed to process incoming message.");
                    throw;
                }
            }
        }

        private static void PrepareStream(MemoryStream stream, byte[]? data = null)
        {
            stream.Position = 0L;
            stream.SetLength(0L);
            if (data != null)
            {
                stream.Write(data, 0, data.Length);
                stream.Position = 0L;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                if (disposing)
                {
                    _log.Debug("{id} disposing.", this.Id);

                    _receiveHelper?.Dispose();
                    _receiveStream.Dispose();
                    _sendHelper?.Dispose();
                    _sendStream.Dispose();

                    //_dispatcher?.Dispose();
                    _queue.Dispose();
                    _session.Dispose();

                    _log.Debug("{id} disposed.", this.Id);
                }
            }

            base.Dispose(disposing);
        }
    }
}
