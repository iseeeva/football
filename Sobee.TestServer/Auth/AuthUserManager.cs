using Sobee.Network;

namespace Sobee.TestServer.Auth
{
    public class AuthUserManager : SessionManager<AuthUser>
    {
        private static readonly Serilog.ILogger _log = Sobee.Common.Logging.Get<AuthUserManager>();
        private bool _isDisposed;

        private readonly AuthRoom _authRoom;

        public AuthUserManager(AuthRoom authRoom) : base(authRoom)
        {
            _authRoom = authRoom;
        }

        public bool TryAdd(SocketWrapper socketWrapper)
        {
            var authUser = new AuthUser(socketWrapper, _authRoom);

            if (TryAdd(authUser))
                return true;
            else
                authUser.Dispose();

            return false;
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
