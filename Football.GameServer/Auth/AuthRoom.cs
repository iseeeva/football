using Football.GameServer.AuthEvents;
using Football.GameServer.Game;
using Football.GameServer.Messages.Auth;
using Football.Network;

namespace Football.GameServer.Auth
{
    public class AuthRoom : GameRoom<AuthRoom, AuthUser>
    {
        public AuthUserManager<AuthUser> Users => (AuthUserManager<AuthUser>)_sessions;
        public GameRoomCommunication Communication => _communication;

        #region Constructor
        public AuthRoom()
        {
            // === Communication ===
            Communication.CommunicationType = SessionType.Authentication;

            // === Match Messages ===
            Communication.RegisterMessage<MatchClientAuthInfoRxMessage>();
            Communication.AddGlobalMessageHandler<MatchClientAuthInfoRxMessage>(AuthMatchClientEvent.AuthInformationReceived);
        }
        #endregion

        #region Session Manager
        protected override SessionManager<AuthRoom, AuthUser> CreateSessionManager()
            => new AuthUserManager<AuthUser>();
        #endregion
    }
}
