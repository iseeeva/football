using Sobee.Network;
using Sobee.TestServer.AuthEvents;

namespace Sobee.TestServer.Auth
{
    public class AuthRoom : Room<AuthRoom, AuthUser>
    {
        public AuthUserManager<AuthUser> Users => (AuthUserManager<AuthUser>)_sessions;
        public RoomCommunication Communication => _communication;

        #region Constructor
        public AuthRoom()
        {
            // === Communication ===
            Communication.CommunicationType = SessionType.Authentication;

            // === User Messages ===
            Communication.RegisterMessage<Messages.Auth.AuthInformationRxMessage>();
            Communication.AddGlobalMessageHandler<Messages.Auth.AuthInformationRxMessage>(AuthClientEvent.AuthInformationReceived);
        }
        #endregion

        #region Session Manager
        protected override SessionManager<AuthRoom, AuthUser> CreateSessionManager()
            => new AuthUserManager<AuthUser>();
        #endregion
    }
}