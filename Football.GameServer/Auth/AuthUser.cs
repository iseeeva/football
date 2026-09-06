using Football.GameServer.Game;
using Football.GameServer.Messages.Auth;
using Football.Network;
using Football.Network.Messaging;

namespace Football.GameServer.Auth
{
    public class AuthUser : GameUser<AuthRoom, AuthUser>
    {
        public MatchClientAuthInfoRxMessage? AuthInformation;

        public AuthUser(SocketWrapper userSocket, MessageCommunication communication)
            : base(userSocket, communication)
        {

        }

        #region Dispose
        protected override void OnDispose()
        {
            AuthInformation = null;
            base.OnDispose();
        }
        #endregion
    }
}