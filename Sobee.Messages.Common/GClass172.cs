using Sobee.Messaging;

namespace Sobee.Messages.Common
{

    // Token: 0x02000094 RID: 148
    [GAttribute0(13699)]
    public class GClass172 : Message
    {
        // Token: 0x060003AF RID: 943 RVA: 0x00005133 File Offset: 0x00003333
        public GClass172()
        {
        }

        // Token: 0x060003B0 RID: 944 RVA: 0x0000E248 File Offset: 0x0000C448
        public GClass172(BinaryReader gclass315_0)
        {
            string_0 = gclass315_0.method_14();
            double_0 = (double)gclass315_0.method_9();
            string_1 = gclass315_0.method_14();
            byte_0 = gclass315_0.method_2();
        }

        // Token: 0x060003B1 RID: 945 RVA: 0x0000514A File Offset: 0x0000334A
        public override void Deserialize(BinaryWriter gclass316_0)
        {
            gclass316_0.method_14(string_0);
            gclass316_0.method_9((int)double_0);
            gclass316_0.method_14(string_1);
            gclass316_0.method_2(byte_0);
        }

        // Token: 0x040006C1 RID: 1729
        public string string_0;

        // Token: 0x040006C2 RID: 1730
        public double double_0 = -1.0;

        // Token: 0x040006C3 RID: 1731
        public string string_1;

        // Token: 0x040006C4 RID: 1732
        public byte byte_0 = 1;
    }
}