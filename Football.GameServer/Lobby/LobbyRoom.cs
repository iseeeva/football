using Football.GameServer.Game;
using Football.GameServer.LobbyEvents;
using Football.GameServer.Messages.Auth;
using Football.Network;
using Football.ServerBrowser.Messages;

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

            // === Browser Messages ===
            Communication.RegisterMessage<BrowserLoginAuthMessage>();
            Communication.AddGlobalMessageHandler<BrowserLoginAuthMessage>(BrowserClientAuthEvent.LoginAuthRequestReceived);

            Communication.RegisterMessage<BrowserRegisterAuthMessage>();
            Communication.AddGlobalMessageHandler<BrowserRegisterAuthMessage>(BrowserClientAuthEvent.RegisterAuthRequestReceived);

            Communication.RegisterMessage<BrowserStatusResponseMessage>();

            Communication.RegisterMessage<BrowserServerListRequestMessage>();
            Communication.RegisterMessage<BrowserServerListResponseMessage>();
            Communication.AddGlobalMessageHandler<BrowserServerListRequestMessage>(BrowserClientInfoEvent.ServerListRequestReceived);

            Communication.RegisterMessage<BrowserUserDetailsRequestMessage>();
            Communication.RegisterMessage<BrowserUserDetailsResponseMessage>();
            Communication.AddGlobalMessageHandler<BrowserUserDetailsRequestMessage>(BrowserClientInfoEvent.UserDetailsRequestReceived);

            Communication.RegisterMessage<BrowserCreateMatchRequestMessage>();
            Communication.RegisterMessage<BrowserServerListItem>();
            Communication.AddGlobalMessageHandler<BrowserCreateMatchRequestMessage>(BrowserClientInfoEvent.BrowserCreateMatchRequestReceived);

        }
        #endregion

        #region Session Manager
        protected override SessionManager<LobbyRoom, LobbyUser> CreateSessionManager()
            => new LobbyUserManager<LobbyUser>();
        #endregion
    }
}
