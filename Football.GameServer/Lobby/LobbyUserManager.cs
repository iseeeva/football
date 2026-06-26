using Football.Network;

namespace Football.GameServer.Lobby
{
    public class LobbyUserManager<TUser> : SessionManager<LobbyRoom, TUser>
      where TUser : LobbyUser
    {
        public LobbyUserManager()
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

            var authUser = new LobbyUser(socketWrapper, Owner.Communication);
            if (base.TryAdd((TUser)authUser))
                return true;

            authUser.Dispose();
            return false;
        }
        #endregion
    }
}