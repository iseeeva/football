using Sobee.Messaging;

namespace Sobee.TestServer.Common
{
    public class Communication : MessageDispatch
    {
        private bool _isDisposed;
        public event EventHandler<MessageEventArgs>? LatencyReceived;

        public Communication()
        {
            Initialize();
            RegisterMessageEvent(typeof(Messages.Latency), OnLatencyReceived);
        }

        private void Initialize()
        {
            // For TestServer only
            RegisterMessagesFromAssemblyName("Sobee.TestServer.Messages");
        }

        protected virtual void OnLatencyReceived(Session session, Message message)
        {
            if (session is Client connection)
            {
                connection.SendHeartbeat();
            }

            LatencyReceived?.Invoke(this, new MessageEventArgs(session, message));
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    // Release references to subscribers
                    LatencyReceived = null;
                }

                _isDisposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
