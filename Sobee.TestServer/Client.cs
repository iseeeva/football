using Serilog;
using Sobee.Common;
using Sobee.Messaging;
using Sobee.Network;

namespace Sobee.TestServer
{
    public class Client : Session
    {
        private static readonly ILogger _log = Logging.Get<Client>();
        private bool _isDisposed;

        public Client(SocketWrapper gclass297_1, MessageDispatch gclass292_1) : base(gclass297_1, gclass292_1)
        {
            _log.Debug("{id} initialized.", Id);
        }

        public void SendHeartbeat()
        {
            SendMessage(new HeartbeatMessage());
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
