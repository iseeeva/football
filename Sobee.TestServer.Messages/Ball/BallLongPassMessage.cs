using Sobee.Network.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Ball
{
    [MessageAttribute(17)]
    public class BallLongPassMessage : Message
    {
        public BallLongPassMessage(BinaryReader gclass315_0) : base(gclass315_0)
        {
            SquadNumber = gclass315_0.method_11();
        }

        public BallLongPassMessage(sbyte squadNumber)
        {
            SquadNumber = squadNumber;
        }

        public override void Serialize(BinaryWriter gclass316_0)
        {
            base.Serialize(gclass316_0);
            gclass316_0.method_11(SquadNumber);
        }

        public readonly sbyte SquadNumber;
    }
}
