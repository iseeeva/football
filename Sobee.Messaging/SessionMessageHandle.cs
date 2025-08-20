using Serilog;
using Sobee.Common;
using Sobee.Network;

namespace Sobee.Messaging
{
    public class SessionMessageHandle : SessionQueueHandle
    {
        private static readonly ILogger _log = Logging.Get<SessionMessageHandle>();
        private bool _isDisposed;

        private MessageDispatch? _dispatcher;

        private MessageHelper? _receiveHelper;
        private readonly MemoryStream _receiveStream = new MemoryStream(MaxReceivingSize);

        private MessageHelper? _sendHelper;
        private readonly MemoryStream _sendStream = new MemoryStream(MaxSendingSize);

        public SessionMessageHandle(Session session) : base(session)
        {
            _log.Debug("{id} initialized.", this.Id);
        }

        public override Task Update(double delta)
        {
            if (_isDisposed) return Task.CompletedTask;
            if (!Session.IsConnected) return Task.CompletedTask;

            try
            {
                _ = ProcessSendQueueAsync();
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
            ObjectDisposedException.ThrowIf(_isDisposed, this);

            try
            {
                if (_sendHelper == null)
                    throw new InvalidOperationException($"{nameof(_sendHelper)} is not initialized.");

                PrepareStream(_sendStream);
                _sendHelper.WriteMessage(message);
                EnqueueSendData(_sendStream.GetBuffer());
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
            ObjectDisposedException.ThrowIf(_isDisposed, this);
            _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
            _receiveHelper = new MessageHelper(_receiveStream, dispatcher.GetDispatcher(), dispatcher.GetMessageTypeToIdDelegate());
            _sendHelper = new MessageHelper(_sendStream, dispatcher.GetDispatcher(), dispatcher.GetMessageTypeToIdDelegate());
        }

        private void ProcessIncomingMessages()
        {
            ObjectDisposedException.ThrowIf(_isDisposed, this);

            byte[]? array;
            while ((array = DequeueReceiveData()) != null)
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

                    _dispatcher.DispatchToMessageEvent(new MessageDelegateArgs(_dispatcher.Owner, new MessageEventArgs(Session, message)));
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
            if (_isDisposed) return;
            _isDisposed = true;

            if (disposing)
            {
                _log.Debug("{id} disposing.", this.Id);

                _receiveHelper?.Dispose();
                _receiveStream.Dispose();
                _sendHelper?.Dispose();
                _sendStream.Dispose();
                //_dispatcher?.Dispose();

                _log.Debug("{id} disposed.", this.Id);
            }

            base.Dispose(disposing);
        }
    }
}
