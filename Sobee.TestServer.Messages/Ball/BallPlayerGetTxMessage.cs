using Sobee.Network.Messaging;
using Sobee.Serialization;
using System.Numerics;

namespace Sobee.TestServer.Messages.Ball
{
    [MessageAttribute(12883)]
    public class BallPlayerGetTxMessage : Message
    {
        public readonly sbyte SquadNumber;
        public readonly Vector2 PlayerPosition;
        public readonly Vector2 PlayerDirection;
        public readonly float PlayerSpeed;

        public BallPlayerGetTxMessage(BinaryReader reader) : base(reader)
        {
            this.SquadNumber = reader.method_11();
            this.PlayerPosition = reader.method_19();
            this.PlayerDirection = GClass97.smethod_10_11_c(reader.method_12());
            this.PlayerSpeed = reader.method_12();
        }

        public BallPlayerGetTxMessage(sbyte squadNumber, Vector2 position, Vector2 direction, float speed)
        {
            this.SquadNumber = squadNumber;
            this.PlayerPosition = position;
            this.PlayerDirection = direction;
            this.PlayerSpeed = speed;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_11(this.SquadNumber);
            writer.method_19(this.PlayerPosition);
            writer.method_12(GClass97.smethod_15(PlayerDirection.Y, PlayerDirection.X));
            writer.method_12(this.PlayerSpeed);
        }

        public override string ToString()
        {
            return "PlayerGetBall - " + this.SquadNumber;
        }
    }
}
