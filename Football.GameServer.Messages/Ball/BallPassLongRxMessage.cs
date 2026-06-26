using Football.Network.Messaging;
using Football.Serialization;

namespace Football.GameServer.Messages.Ball
{
    [Message(17)]
    public class BallPassLongRxMessage : Message
    {
        public readonly sbyte SquadNumber;

        public BallPassLongRxMessage(BinaryReader reader) : base(reader)
        {
            SquadNumber = reader.method_11();
        }

        public BallPassLongRxMessage(sbyte squadNumber)
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
