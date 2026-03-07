using System.Numerics;
using Sobee.Network.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Ball
{
    [MessageAttribute(13196)]
    public class BallUpdateTxMessage : Message
    {
        public readonly Vector3 BallPosition;
        public readonly Vector3 BallVelocity;

        public BallUpdateTxMessage(BinaryReader reader) : base(reader)
        {
            this.BallPosition = reader.method_20();
            this.BallVelocity = reader.method_20();
        }

        public BallUpdateTxMessage(Vector3 ballPosition, Vector3 ballVelocity)
        {
            this.BallPosition = ballPosition;
            this.BallVelocity = ballVelocity;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_20(this.BallPosition);
            writer.method_20(this.BallVelocity);
        }
    }
}
