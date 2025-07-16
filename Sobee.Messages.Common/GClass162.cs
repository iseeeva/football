using Sobee.Messaging;

namespace Sobee.Messages.Common
{

    // Token: 0x02000046 RID: 70
    [GAttribute0(8387)]
    public class GClass162 : Message
    {
        // Token: 0x0600014F RID: 335 RVA: 0x000032DA File Offset: 0x000014DA
        public sbyte method_0()
        {
            return sbyte_0;
        }

        // Token: 0x06000150 RID: 336 RVA: 0x000032E2 File Offset: 0x000014E2
        public string method_1()
        {
            return string_0;
        }

        // Token: 0x06000151 RID: 337 RVA: 0x000032EA File Offset: 0x000014EA
        public MatchUIEventType method_2()
        {
            return matchUIEventType_0;
        }

        // Token: 0x06000152 RID: 338 RVA: 0x000032F2 File Offset: 0x000014F2
        public float method_3()
        {
            return float_0;
        }

        // Token: 0x06000153 RID: 339 RVA: 0x000032FA File Offset: 0x000014FA
        public GClass162(BinaryReader gclass315_0)
        {
            string_0 = gclass315_0.method_14();
            matchUIEventType_0 = (MatchUIEventType)gclass315_0.method_2();
            sbyte_0 = gclass315_0.method_11();
            float_0 = gclass315_0.method_12();
        }

        // Token: 0x06000154 RID: 340 RVA: 0x00003332 File Offset: 0x00001532
        public GClass162(double double_0, MatchUIEventType matchUIEventType_1, sbyte sbyte_1, string string_1)
        {
            matchUIEventType_0 = matchUIEventType_1;
            string_0 = string_1;
            sbyte_0 = sbyte_1;
            float_0 = (float)double_0;
        }

        // Token: 0x06000155 RID: 341 RVA: 0x00003358 File Offset: 0x00001558
        public override void Serialize(BinaryWriter gclass316_0)
        {
            gclass316_0.method_14(string_0);
            gclass316_0.method_2((byte)matchUIEventType_0);
            gclass316_0.method_11(sbyte_0);
            gclass316_0.method_12(float_0);
        }

        // Token: 0x040004E5 RID: 1253
        private string string_0;

        // Token: 0x040004E6 RID: 1254
        private MatchUIEventType matchUIEventType_0;

        // Token: 0x040004E7 RID: 1255
        private sbyte sbyte_0;

        // Token: 0x040004E8 RID: 1256
        private float float_0;
    }
}