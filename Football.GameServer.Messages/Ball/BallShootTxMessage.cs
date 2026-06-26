using Football.Serialization;
using System.Numerics;

namespace Football.GameServer.Messages.Ball
{
    [Message(17253)]
    public class BallShootTxMessage : AnimationMessage
    {
        public readonly sbyte SquadNumber;
        public readonly Vector2 PlayerPosition;
        public readonly Vector2 PlayerDirection;
        public readonly Vector3 BallVelocity;
        public readonly float PlayerSpeed;

        public BallShootTxMessage(BinaryReader reader) : base(reader)
        {
            SquadNumber = reader.method_11();
            PlayerPosition = reader.method_19();
            PlayerDirection = GClass97.smethod_10_11_c(reader.method_12());
            BallVelocity = reader.method_20();
            PlayerSpeed = reader.method_12();
        }

        public BallShootTxMessage(sbyte squadNumber, Vector2 playerPos, Vector2 playerDir, Vector3 ballVelocity, float playerSpeed, AnimationType animationType) : base(animationType)
        {
            SquadNumber = squadNumber;
            PlayerPosition = playerPos;
            PlayerDirection = playerDir;
            BallVelocity = ballVelocity;
            PlayerSpeed = playerSpeed;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_11(SquadNumber);
            writer.method_19(PlayerPosition);
            writer.method_12(GClass97.smethod_15(PlayerDirection.Y, PlayerDirection.X));
            writer.method_20(BallVelocity);
            writer.method_12(PlayerSpeed);
        }

        public override string ToString()
        {
            return string.Concat(new object[]
            {
            "PlayerShoot - ",
            SquadNumber,
            " - ",
            BallVelocity.ToString()
            });
        }
    }
}
