using Football.Network.Messaging;
using Football.Serialization;
using System.Numerics;

namespace Football.GameServer.Messages.Ball
{
    [Message(12883)]
    public class BallPlayerGetTxMessage : Message
    {
        public readonly sbyte SquadNumber;
        public readonly Vector2 PlayerPosition;
        public readonly Vector2 PlayerDirection;
        public readonly float PlayerSpeed;

        public BallPlayerGetTxMessage(BinaryReader reader) : base(reader)
        {
            SquadNumber = reader.method_11();
            PlayerPosition = reader.method_19();
            PlayerDirection = GClass97.smethod_10_11_c(reader.method_12());
            PlayerSpeed = reader.method_12();
        }

        public BallPlayerGetTxMessage(sbyte squadNumber, Vector2 position, Vector2 direction, float speed)
        {
            SquadNumber = squadNumber;
            PlayerPosition = position;
            PlayerDirection = direction;
            PlayerSpeed = speed;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_11(SquadNumber);
            writer.method_19(PlayerPosition);
            writer.method_12(GClass97.smethod_15(PlayerDirection.Y, PlayerDirection.X));
            writer.method_12(PlayerSpeed);
        }

        public override string ToString()
        {
            return "PlayerGetBall - " + SquadNumber;
        }
    }
}
