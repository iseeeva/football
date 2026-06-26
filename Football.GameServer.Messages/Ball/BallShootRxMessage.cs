using Football.Network.Messaging;
using Football.Serialization;
using System.Numerics;

namespace Football.GameServer.Messages.Ball
{
    [Message(4386)]
    public class BallShootRxMessage : Message
    {
        public readonly float ShootStrength;
        public readonly Vector2 ShootDirection;

        public BallShootRxMessage(BinaryReader reader) : base(reader)
        {
            ShootStrength = reader.method_12();
            ShootDirection = GClass97.smethod_10_11_c(reader.method_12());
        }

        public BallShootRxMessage(float shootStrength, Vector2 shootDirection)
        {
            ShootStrength = shootStrength;
            ShootDirection = shootDirection;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_12(ShootStrength);
            writer.method_12(GClass97.smethod_15(ShootDirection.Y, ShootDirection.X));
        }
    }
}
