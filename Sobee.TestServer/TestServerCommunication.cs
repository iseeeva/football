using Sobee.Common;
using Sobee.Network.Messaging;

namespace Sobee.TestServer
{
    public class TestServerCommunication : MessageCommunication
    {
        private static readonly Serilog.ILogger _log = Logging.Get<TestServerCommunication>();
        private bool _isDisposed;

        public TestServerCommunication() : base()
        {
            RegisterMessagesFromAssemblyName("Sobee.TestServer.Messages");
            RegisterMessageEvent<Messages.LatencyMessage>(OnReceivedMessage);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                if (disposing)
                {
                    _log.Debug("{id} disposed.", Id);
                }
            }

            base.Dispose(disposing);
        }
    }
}
