using System.Numerics;
using Sobee.Network.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages
{

    // Token: 0x02000067 RID: 103
    [MessageAttribute(15051)]
    public class GClass167 : Message
    {
        // Token: 0x06000230 RID: 560 RVA: 0x00003F5D File Offset: 0x0000215D
        public float method_0()
        {
            return float_0;
        }

        // Token: 0x06000231 RID: 561 RVA: 0x00003F65 File Offset: 0x00002165
        public float method_1()
        {
            return float_1;
        }

        // Token: 0x06000232 RID: 562 RVA: 0x00003F6D File Offset: 0x0000216D
        public float method_2()
        {
            return float_2;
        }

        // Token: 0x06000233 RID: 563 RVA: 0x00003F75 File Offset: 0x00002175
        public float method_3()
        {
            return float_3;
        }

        // Token: 0x06000234 RID: 564 RVA: 0x00003F7D File Offset: 0x0000217D
        public Vector3 method_4()
        {
            return vector3_0;
        }

        // Token: 0x06000235 RID: 565 RVA: 0x00003F85 File Offset: 0x00002185
        public float method_5()
        {
            return float_4;
        }

        // Token: 0x06000236 RID: 566 RVA: 0x0000B6FC File Offset: 0x000098FC
        public GClass167()
        {
            // TODO: Remove hardcoded values when possible
            float_0 = 0;
            float_1 = 0;
            float_2 = 0;
            float_3 = 0;
            vector3_0 = new Vector3(0, 0, 0);
            float_4 = 0;
        }

        // Token: 0x06000237 RID: 567 RVA: 0x0000B760 File Offset: 0x00009960
        public GClass167(BinaryReader gclass315_0)
        {
            float_0 = gclass315_0.method_12();
            float_1 = gclass315_0.method_12();
            float_2 = gclass315_0.method_12();
            float_3 = gclass315_0.method_12();
            vector3_0 = gclass315_0.method_20();
            float_4 = gclass315_0.method_12();
        }

        // Token: 0x06000238 RID: 568 RVA: 0x0000B80C File Offset: 0x00009A0C
        public GClass167(float float_5, float float_6, float float_7, float float_8, Vector3 vector3_1, float float_9)
        {
            float_0 = float_5;
            float_1 = float_6;
            float_2 = float_7;
            float_3 = float_8;
            vector3_0 = vector3_1;
            float_4 = float_9;
        }

        // Token: 0x06000239 RID: 569 RVA: 0x0000B8A0 File Offset: 0x00009AA0
        public override void Serialize(BinaryWriter gclass316_0)
        {
            gclass316_0.method_12(float_0);
            gclass316_0.method_12(float_1);
            gclass316_0.method_12(float_2);
            gclass316_0.method_12(float_3);
            gclass316_0.method_20(vector3_0);
            gclass316_0.method_12(float_4);
        }

        // Token: 0x040005ED RID: 1517
        private float float_0 = 200f;

        // Token: 0x040005EE RID: 1518
        private float float_1 = 100f;

        // Token: 0x040005EF RID: 1519
        private float float_2 = 0.45f;

        // Token: 0x040005F0 RID: 1520
        private float float_3 = 0.75f;

        // Token: 0x040005F1 RID: 1521
        private Vector3 vector3_0 = new Vector3(0f, 0f, -981f);

        // Token: 0x040005F2 RID: 1522
        private float float_4 = 2f;
    }
}