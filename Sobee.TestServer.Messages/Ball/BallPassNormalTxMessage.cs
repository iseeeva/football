using System.Numerics;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Ball
{
    [MessageAttribute(30069)]
    public class BallPassNormalTxMessage : AnimationAbstractMessage
    {
        public readonly sbyte SquadNumber;
        public readonly Vector2 PlayerPosition;
        public readonly Vector2 PlayerDirection;
        public readonly Vector3 BallVelocity;
        public readonly float PlayerSpeed;

        public BallPassNormalTxMessage(BinaryReader reader) : base(reader)
        {
            this.SquadNumber = reader.method_11();
            this.PlayerPosition = reader.method_19();
            this.PlayerDirection = GClass97.smethod_10_11_c(reader.method_12());
            this.BallVelocity = reader.method_20();
            this.PlayerSpeed = reader.method_12();
        }

        public BallPassNormalTxMessage(sbyte squadNumber, Vector2 playerPos, Vector2 playerDir, Vector3 ballVelocity, float playerSpeed, AnimationType animationType) : base(animationType)
        {
            this.SquadNumber = squadNumber;
            this.PlayerPosition = playerPos;
            this.PlayerDirection = playerDir;
            this.BallVelocity = ballVelocity;
            this.PlayerSpeed = playerSpeed;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_11(this.SquadNumber);
            writer.method_19(this.PlayerPosition);
            writer.method_12(GClass97.smethod_15(PlayerDirection.Y, PlayerDirection.X));
            writer.method_20(this.BallVelocity);
            writer.method_12(this.PlayerSpeed);
        }

        public override string ToString()
        {
            return string.Concat(new object[]
            {
            "PlayerPass - ",
            this.SquadNumber,
            " - ",
            this.BallVelocity.ToString()
            });
        }
    }
}
