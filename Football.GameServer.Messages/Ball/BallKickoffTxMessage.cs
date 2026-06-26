using Football.Serialization;
using System.Numerics;

namespace Football.GameServer.Messages.Ball
{
    [Message(8756)]
    public class BallKickoffTxMessage : AnimationMessage
    {
        public readonly Vector3 BallVelocity;

        public BallKickoffTxMessage(BinaryReader reader) : base(reader)
        {
            BallVelocity = reader.method_20();
        }

        public BallKickoffTxMessage(Vector3 ballVelocity, AnimationType animationType) : base(animationType)
        {
            BallVelocity = ballVelocity;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_20(BallVelocity);
        }

        public override string ToString()
        {
            return "PlayerKickoff - " + BallVelocity.ToString();
        }
    }
}
