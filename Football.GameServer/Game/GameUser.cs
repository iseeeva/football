using Football.Network;
using Football.Network.Messaging;

namespace Football.GameServer.Game
{
    public class GameUser : Session
    {
        public GameUser(SocketWrapper socket, MessageCommunication communication)
            : base(socket, communication)
        {
            SessionType = SessionType.User;
        }
    }
}