using System.Numerics;
using Sobee.Network.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Ball
{
    [MessageAttribute(12883)]
    public class BallGet : Message
    {
        public Vector2 GetDirectionAxis()
        {
            return new Vector2(GClass97.smethod_11(this.Direction), GClass97.smethod_10(this.Direction));
        }

        public BallGet(BinaryReader gclass315_0) : base(gclass315_0)
        {
            this.SquadNumber = gclass315_0.method_11();
            this.Position = gclass315_0.method_19();
            this.Direction = gclass315_0.method_12();
            this.Speed = gclass315_0.method_12();
        }

        public BallGet(sbyte squadNumber, Vector2 position, Vector2 direction, float speed)
        {
            this.SquadNumber = squadNumber;
            this.Position = position;
            this.Direction = GClass97.smethod_15(direction.Y, direction.X);
            this.Speed = speed;
        }

        public override string ToString()
        {
            return "PlayerGetBall - " + this.SquadNumber;
        }

        public override void Serialize(BinaryWriter gclass316_0)
        {
            base.Serialize(gclass316_0);
            gclass316_0.method_11(this.SquadNumber);
            gclass316_0.method_19(this.Position);
            gclass316_0.method_12(this.Direction);
            gclass316_0.method_12(this.Speed);
        }

        public sbyte SquadNumber;

        public Vector2 Position;

        public float Direction;

        public float Speed;
    }
}
