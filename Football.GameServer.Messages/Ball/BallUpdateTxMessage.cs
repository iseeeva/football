using Football.Network.Messaging;
using Football.Serialization;
using System.Numerics;

namespace Football.GameServer.Messages.Ball
{
    [Message(13196)]
    public class BallUpdateTxMessage : Message
    {
        public readonly Vector3 BallPosition;
        public readonly Vector3 BallVelocity;

        public BallUpdateTxMessage(BinaryReader reader) : base(reader)
        {
            BallPosition = reader.method_20();
            BallVelocity = reader.method_20();
        }

        public BallUpdateTxMessage(Vector3 ballPosition, Vector3 ballVelocity)
        {
            BallPosition = ballPosition;
            BallVelocity = ballVelocity;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_20(BallPosition);
            writer.method_20(BallVelocity);
        }
    }
}
