using Football.GameServer.Game;
using Football.GameServer.LobbyEvents;
using Football.GameServer.Messages.Auth;
using Football.Network;

namespace Football.GameServer.Lobby
{
    public class LobbyRoom : GameRoom<LobbyRoom, LobbyUser>
    {
        public LobbyUserManager<LobbyUser> Users => (LobbyUserManager<LobbyUser>)_sessions;
        public GameRoomCommunication Communication => _communication;

        #region Constructor
        public LobbyRoom()
        {
            // === Communication ===
            Communication.CommunicationType = SessionType.Authentication;

            // === Match Messages ===
            Communication.RegisterMessage<MatchClientAuthInfoRxMessage>();
            Communication.AddGlobalMessageHandler<MatchClientAuthInfoRxMessage>(MatchClientAuthEvent.AuthInformationReceived);
        }
        #endregion

        #region Session Manager
        protected override SessionManager<LobbyRoom, LobbyUser> CreateSessionManager()
            => new LobbyUserManager<LobbyUser>();
        #endregion
    }
}
