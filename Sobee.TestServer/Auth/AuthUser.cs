using Sobee.Network;
using Sobee.Network.Messaging;
using Sobee.TestServer.Messages.Auth;

namespace Sobee.TestServer.Auth
{
    public class AuthUser : User
    {
        public AuthInformationRxMessage? AuthInformation;

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