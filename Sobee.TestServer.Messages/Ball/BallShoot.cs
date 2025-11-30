using System.Numerics;
using Sobee.Network.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Ball
{
    [MessageAttribute(4386)]
    public class BallShoot : Message
    {
        public BallShoot(BinaryReader gclass315_0) : base(gclass315_0)
        {
            this.Strength = gclass315_0.method_12();
            this.Direction = GClass97.smethod_10_11_c(gclass315_0.method_12());
        }

        // Token: 0x060001DB RID: 475 RVA: 0x00003B90 File Offset: 0x00001D90
        public BallShoot(float strength, Vector2 direction)
        {
            this.Strength = strength;
            this.Direction = direction;
        }

        // Token: 0x060001DC RID: 476 RVA: 0x00003BA6 File Offset: 0x00001DA6
        public override void Serialize(BinaryWriter gclass316_0)
        {
            base.Serialize(gclass316_0);
            gclass316_0.method_12(this.Strength);
            gclass316_0.method_12(GClass97.smethod_15(Direction.Y, Direction.X));
        }

        // Token: 0x040005BD RID: 1469
        public readonly float Strength;

        // Token: 0x040005BE RID: 1470
        public readonly Vector2 Direction;
    }

}
