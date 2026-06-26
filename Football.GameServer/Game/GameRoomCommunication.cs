using Football.GameServer.Messages;
using Football.Network.Messaging;

namespace Football.GameServer.Game
{
    public class GameRoomCommunication : MessageCommunication
    {
        public GameRoomCommunication() : base()
        {
            RegisterMessages("Football.GameServer.Messages");
            RegisterMessage<LatencyMessage>();
        }
    }
}