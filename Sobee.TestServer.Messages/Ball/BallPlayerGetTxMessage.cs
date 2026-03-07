using System.Numerics;
using Sobee.Network.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Ball
{
    [MessageAttribute(12883)]
    public class BallPlayerGetTxMessage : Message
    {
        public readonly sbyte SquadNumber;
        public readonly Vector2 Position;
        public readonly float Direction;
        public readonly float Speed;

        public Vector2 GetDirectionAxis()
        {
            return new Vector2(GClass97.smethod_11(this.Direction), GClass97.smethod_10(this.Direction));
        }

        public BallPlayerGetTxMessage(BinaryReader reader) : base(reader)
        {
            this.SquadNumber = reader.method_11();
            this.Position = reader.method_19();
            this.Direction = reader.method_12();
            this.Speed = reader.method_12();
        }

        public BallPlayerGetTxMessage(sbyte squadNumber, Vector2 position, Vector2 direction, float speed)
        {
            this.SquadNumber = squadNumber;
            this.Position = position;
            this.Direction = GClass97.smethod_15(direction.Y, direction.X);
            this.Speed = speed;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_11(this.SquadNumber);
            writer.method_19(this.Position);
            writer.method_12(this.Direction);
            writer.method_12(this.Speed);
        }

        public override string ToString()
        {
            return "PlayerGetBall - " + this.SquadNumber;
        }
    }
}
