using Football.Serialization;
using System.Numerics;

namespace Football.GameServer.Messages.Ball
{
    [Message(43683)]
    public class BallTackleSuccessTxMessage : AnimationMessage
    {
        public readonly sbyte SquadNumber;
        public readonly Vector2 PlayerPosition;
        public readonly Vector2 PlayerDirection;
        public readonly float PlayerSpeed; // or AnimationSpeed idk
        public readonly sbyte UnknownByte0;

        public BallTackleSuccessTxMessage(BinaryReader reader) : base(reader)
        {
            SquadNumber = reader.method_11();
            PlayerPosition = reader.method_19();
            PlayerDirection = GClass97.smethod_10_11_c(reader.method_12());
            PlayerSpeed = reader.method_12();
            UnknownByte0 = reader.method_11();
        }

        public BallTackleSuccessTxMessage(sbyte squadNumber, Vector2 position, Vector2 direction, sbyte sbyte_3, float speed, AnimationType animationType_1) : base(animationType_1)
        {
            SquadNumber = squadNumber;
            PlayerPosition = position;
            PlayerDirection = direction;
            UnknownByte0 = sbyte_3;
            PlayerSpeed = speed;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_11(SquadNumber);
            writer.method_19(PlayerPosition);
            writer.method_12(GClass97.smethod_15(PlayerDirection.Y, PlayerDirection.X));
            writer.method_12(PlayerSpeed);
            writer.method_11(UnknownByte0);
        }

        public override string ToString()
        {
            return "PlayerTackleSuccess - " + SquadNumber;
        }
    }
}
