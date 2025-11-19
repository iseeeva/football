using System.Numerics;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Ball
{

    [MessageAttribute(8756)]
    public class BallKickoffHit : AnimationMessageAbstract
    {
        public BallKickoffHit(BinaryReader gclass315_0) : base(gclass315_0)
        {
            this.Velocity = gclass315_0.method_20();
        }

        public BallKickoffHit(Vector3 velocity, AnimationType animationType) : base(animationType)
        {
            this.Velocity = velocity;
        }

        public override string ToString()
        {
            return "PlayerKickoff - " + this.Velocity.ToString();
        }

        public override void Serialize(BinaryWriter gclass316_0)
        {
            base.Serialize(gclass316_0);
            gclass316_0.method_20(this.Velocity);
        }

        public Vector3 Velocity;
    }
}
