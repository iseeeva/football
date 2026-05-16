using Sobee.Network;
using Sobee.Network.Messaging;

namespace Sobee.TestServer
{
    public class User : Session
    {
        public User(SocketWrapper socket, MessageCommunication communication)
            : base(socket, communication)
        {
            SessionType = SessionType.User;
        }
    }
}