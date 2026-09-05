using Football.GameServer.Game;
using Football.GameServer.Messages.Auth;
using Football.Network;
using Football.Network.Messaging;

namespace Football.GameServer.Lobby
{
    public class LobbyUser : GameUser
    {
        public MatchClientAuthInfoRxMessage? AuthInformation;

        public LobbyUser(SocketWrapper userSocket, MessageCommunication communication)
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