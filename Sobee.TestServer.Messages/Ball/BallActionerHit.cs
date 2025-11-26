using System.Numerics;
using Sobee.Network.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Ball
{
    [MessageAttribute(22132)]
    public class BallActionerHit : Message
    {
        public BallActionerHit(BinaryReader gclass315_0) : base(gclass315_0)
        {
            Strength = gclass315_0.method_12();
            Direction = gclass315_0.method_19();
            HitSubType = (HitSubType)gclass315_0.method_2();
        }

        // Token: 0x060001E6 RID: 486 RVA: 0x00003C74 File Offset: 0x00001E74
        public BallActionerHit(float strength, Vector2 direction, HitSubType hitSubType)
        {
            Strength = strength;
            Direction = direction;
            HitSubType = hitSubType;
        }

        // Token: 0x060001E7 RID: 487 RVA: 0x00003C91 File Offset: 0x00001E91
        public override void Serialize(BinaryWriter gclass316_0)
        {
            base.Serialize(gclass316_0);
            gclass316_0.method_12(Strength);
            gclass316_0.method_19(Direction);
            gclass316_0.method_2((byte)HitSubType);
        }

        // Token: 0x040005C1 RID: 1473
        public float Strength;

        // Token: 0x040005C2 RID: 1474
        public Vector2 Direction;

        // Token: 0x040005C3 RID: 1475
        public HitSubType HitSubType;
    }
}
