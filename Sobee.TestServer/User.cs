using Serilog;
using Sobee.Common;
using Sobee.Messaging;
using Sobee.Network;

namespace Sobee.TestServer
{
    public class User : Session
    {
        private static readonly ILogger _log = Logging.Get<User>();
        private bool _isDisposed;

        public User(SocketWrapper socket, Communication playerComm) : base(socket, playerComm)
        {
            SessionType = SessionType.User;
            _log.Debug("{id} initialized.", Id);
        }

        public override Task Update(double delta)
        {
            return base.Update(delta);
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
