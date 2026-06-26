using Football.Network.Messaging;
using Football.Serialization;

namespace Football.GameServer.Messages.Ball
{
    [Message(22585)]
    public class BallTackleRxMessage : Message
    {
        public BallTackleRxMessage(BinaryReader reader) : base(reader)
        {

        }

        public BallTackleRxMessage() : base()
        {

        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
        }
    }
}
