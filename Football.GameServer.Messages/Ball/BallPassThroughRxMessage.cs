using Football.Network.Messaging;
using Football.Serialization;

namespace Football.GameServer.Messages.Ball
{
    [Message(39209)]
    public class BallPassThroughRxMessage : Message
    {
        public readonly sbyte SquadNumber;

        public BallPassThroughRxMessage(BinaryReader reader) : base(reader)
        {
            SquadNumber = reader.method_11();
        }

        public BallPassThroughRxMessage(sbyte squadNumber)
        {
            SquadNumber = squadNumber;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_11(SquadNumber);
        }
    }
}
