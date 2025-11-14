using Sobee.Network;

namespace Sobee.TestServer.Auth
{
    public class AuthUserManager : SessionManager<AuthUser>
    {
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
    }
}
