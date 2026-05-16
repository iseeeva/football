using Sobee.Network;

namespace Sobee.TestServer.Auth
{
    public class AuthUserManager<TUser> : SessionManager<AuthRoom, TUser>
      where TUser : AuthUser
    {
        public AuthUserManager()
        {

        }

        #region Users
        public override bool TryAdd(TUser session)
        {
            throw new NotImplementedException("TryAdd is not implemented. Use TryCreate instead.");
            //_log.Warning("TryAdd is not implemented. Use TryCreate instead.");
            //return false;
        }

        public virtual bool TryCreate(SocketWrapper socketWrapper)
        {
            if (Owner == null)
            {
                _log.Error("Owner is null. TryCreate method aborted.");
                return false;
            }

            var authUser = new AuthUser(socketWrapper, Owner.Communication);
            if (base.TryAdd((TUser)authUser))
                return true;

            authUser.Dispose();
            return false;
        }
        #endregion
    }
}