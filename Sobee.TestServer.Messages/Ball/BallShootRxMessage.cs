using System.Numerics;
using Sobee.Network.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Ball
{
    [MessageAttribute(4386)]
    public class BallShootRxMessage : Message
    {
        public readonly float ShootStrength;
        public readonly Vector2 ShootDirection;

        public BallShootRxMessage(BinaryReader reader) : base(reader)
        {
            this.ShootStrength = reader.method_12();
            this.ShootDirection = GClass97.smethod_10_11_c(reader.method_12());
        }

        public BallShootRxMessage(float shootStrength, Vector2 shootDirection)
        {
            this.ShootStrength = shootStrength;
            this.ShootDirection = shootDirection;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_12(this.ShootStrength);
            writer.method_12(GClass97.smethod_15(ShootDirection.Y, ShootDirection.X));
        }
    }
}
