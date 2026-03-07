using Sobee.Network.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Ball
{
    [MessageAttribute(43635)]
    public class BallPassRxMessage : Message
    {
        public readonly sbyte SquadNumber;

        public BallPassRxMessage(BinaryReader reader) : base(reader)
        {
            SquadNumber = reader.method_11();
        }

        public BallPassRxMessage(sbyte squadNumber)
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
