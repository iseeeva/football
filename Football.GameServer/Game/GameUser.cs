using Football.Network;
using Football.Network.Messaging;

namespace Football.GameServer.Game
{
    public class GameUser<TRoom, TSession> : Session
        where TRoom : GameRoom<TRoom, TSession>
        where TSession : GameUser<TRoom, TSession>
    {
        public GameUser(SocketWrapper socket, MessageCommunication communication)
            : base(socket, communication)
        {
            SessionType = SessionType.User;
        }
    }
}