using Sobee.Network.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Ball
{
    [MessageAttribute(22585)]
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
