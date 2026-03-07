using System.Numerics;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Ball
{
    [MessageAttribute(8756)]
    public class BallKickoffTxMessage : AnimationAbstractMessage
    {
        public readonly Vector3 BallVelocity;

        public BallKickoffTxMessage(BinaryReader reader) : base(reader)
        {
            this.BallVelocity = reader.method_20();
        }

        public BallKickoffTxMessage(Vector3 ballVelocity, AnimationType animationType) : base(animationType)
        {
            this.BallVelocity = ballVelocity;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_20(this.BallVelocity);
        }

        public override string ToString()
        {
            return "PlayerKickoff - " + this.BallVelocity.ToString();
        }
    }
}
