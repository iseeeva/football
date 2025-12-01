using System.Numerics;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Ball
{
    // Token: 0x02000037 RID: 55
    [MessageAttribute(17253)]
    public class BallShootHitMessage : AnimationMessageAbstract
    {
        // Token: 0x06000095 RID: 149 RVA: 0x00006324 File Offset: 0x00004524
        public BallShootHitMessage(BinaryReader gclass315_0) : base(gclass315_0)
        {
            this.SquadNumber = gclass315_0.method_11();
            this.Position = gclass315_0.method_19();
            this.Direction = gclass315_0.method_12();
            this.Velocity = gclass315_0.method_20();
            this.Speed = gclass315_0.method_12();
        }

        // Token: 0x06000096 RID: 150 RVA: 0x00006374 File Offset: 0x00004574
        public BallShootHitMessage(sbyte squadNumber, Vector2 position, Vector2 direction, Vector3 velocity, float speed, AnimationType animationType_1) : base(animationType_1)
        {
            this.SquadNumber = squadNumber;
            this.Position = position;
            this.Direction = GClass97.smethod_15(direction.Y, direction.X);
            this.Velocity = velocity;
            this.Speed = speed;
        }

        // Token: 0x06000097 RID: 151 RVA: 0x000063C0 File Offset: 0x000045C0
        public override string ToString()
        {
            return string.Concat(new object[]
            {
            "PlayerShoot - ",
            this.SquadNumber,
            " - ",
            this.Velocity.ToString()
            });
        }

        // Token: 0x06000098 RID: 152 RVA: 0x00006408 File Offset: 0x00004608
        public override void Serialize(BinaryWriter gclass316_0)
        {
            base.Serialize(gclass316_0);
            gclass316_0.method_11(this.SquadNumber);
            gclass316_0.method_19(this.Position);
            gclass316_0.method_12(this.Direction);
            gclass316_0.method_20(this.Velocity);
            gclass316_0.method_12(this.Speed);
        }

        // Token: 0x04000475 RID: 1141
        public readonly sbyte SquadNumber;

        // Token: 0x04000476 RID: 1142
        public readonly Vector2 Position;

        // Token: 0x04000477 RID: 1143
        public readonly float Direction;

        // Token: 0x04000478 RID: 1144
        public readonly Vector3 Velocity;

        // Token: 0x04000479 RID: 1145
        public readonly float Speed;
    }

}
