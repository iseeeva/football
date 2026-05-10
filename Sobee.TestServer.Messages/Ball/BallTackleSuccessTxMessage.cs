using Sobee.Serialization;
using System.Numerics;

namespace Sobee.TestServer.Messages.Ball
{
    [MessageAttribute(43683)]
    public class BallTackleSuccessTxMessage : AnimationMessage
    {
        public readonly sbyte SquadNumber;
        public readonly Vector2 PlayerPosition;
        public readonly Vector2 PlayerDirection;
        public readonly float PlayerSpeed; // or AnimationSpeed idk
        public readonly sbyte UnknownByte0;

        public BallTackleSuccessTxMessage(BinaryReader reader) : base(reader)
        {
            this.SquadNumber = reader.method_11();
            this.PlayerPosition = reader.method_19();
            this.PlayerDirection = GClass97.smethod_10_11_c(reader.method_12());
            this.PlayerSpeed = reader.method_12();
            this.UnknownByte0 = reader.method_11();
        }

        public BallTackleSuccessTxMessage(sbyte squadNumber, Vector2 position, Vector2 direction, sbyte sbyte_3, float speed, AnimationType animationType_1) : base(animationType_1)
        {
            this.SquadNumber = squadNumber;
            this.PlayerPosition = position;
            this.PlayerDirection = direction;
            this.UnknownByte0 = sbyte_3;
            this.PlayerSpeed = speed;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_11(this.SquadNumber);
            writer.method_19(this.PlayerPosition);
            writer.method_12(GClass97.smethod_15(PlayerDirection.Y, PlayerDirection.X));
            writer.method_12(this.PlayerSpeed);
            writer.method_11(this.UnknownByte0);
        }

        public override string ToString()
        {
            return "PlayerTackleSuccess - " + this.SquadNumber;
        }
    }
}
