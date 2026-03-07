using Serilog;
using Sobee.Common;
using Sobee.Network;
using Sobee.Network.Messaging;
using Sobee.TestServer.Messages.Auth;

namespace Sobee.TestServer.Auth
{
    public class AuthUser : User
    {
        private static readonly ILogger _log = Logging.Get<AuthUser>();
        private bool _isDisposed;

        public AuthInformationRxMessage? AuthInformation;

        public AuthUser(
            SocketWrapper userSocket,
            MessageCommunication communication
        ) : base(userSocket, communication)
        {
            SessionType = SessionType.User;
            _log.Debug("{id} initialized.", Id);
        }

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            if (disposing)
            {
                AuthInformation = null;
                _log.Debug("{id} disposed.", Id);
            }

            base.Dispose(disposing);
        }
    }
}
