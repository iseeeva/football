using Serilog;
using Sobee.Common;
using Sobee.Network;

namespace Sobee.TestServer
{
    public class User : Client
    {
        private static readonly ILogger _log = Logging.Get<User>();
        private bool _isDisposed;

        public User(SocketWrapper socket, Communication playerComm) : base(socket, playerComm)
        {
            SessionType = SessionType.User;
            _log.Debug("{id} initialized.", Id);
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
