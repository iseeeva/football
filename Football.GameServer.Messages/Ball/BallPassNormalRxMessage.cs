using Football.Network.Messaging;
using Football.Serialization;

namespace Football.GameServer.Messages.Ball
{
    [Message(43635)]
    public class BallPassNormalRxMessage : Message
    {
        public readonly sbyte SquadNumber;

        public BallPassNormalRxMessage(BinaryReader reader) : base(reader)
        {
            SquadNumber = reader.method_11();
        }

        public BallPassNormalRxMessage(sbyte squadNumber)
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
