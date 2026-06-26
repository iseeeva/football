using Football.Network.Messaging;
using Football.Serialization;

namespace Football.GameServer.Messages.Player
{
    [Message(3)]
    public class PlayerHeartbeatRxMessage : Message
    {
        public override bool vmethod_0()
        {
            return false;
        }

        public PlayerHeartbeatRxMessage()
        {

        }

        public PlayerHeartbeatRxMessage(BinaryReader reader) : base(reader)
        {

        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
        }
    }
}
