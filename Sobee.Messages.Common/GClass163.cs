namespace Sobee.Messages.Common
{

    // Token: 0x0200004F RID: 79
    [GAttribute0(13698)]
    public class GClass163 : Message
    {
        // Token: 0x0600017D RID: 381 RVA: 0x00003693 File Offset: 0x00001893
        public GClass163()
        {
        }

        // Token: 0x0600017E RID: 382 RVA: 0x00007B08 File Offset: 0x00005D08
        public GClass163(BinaryReader gclass315_0)
        {
            string_0 = gclass315_0.method_14();
            double_0 = (double)gclass315_0.method_9();
            int_0 = gclass315_0.method_9();
            byte_0 = gclass315_0.method_2();
        }

        // Token: 0x0600017F RID: 383 RVA: 0x000036A9 File Offset: 0x000018A9
        public override void Deserialize(BinaryWriter gclass316_0)
        {
            gclass316_0.method_14(string_0);
            gclass316_0.method_9((int)double_0);
            gclass316_0.method_9(int_0);
            gclass316_0.method_2(byte_0);
        }

        // Token: 0x040004FA RID: 1274
        public string string_0;

        // Token: 0x040004FB RID: 1275
        public double double_0;

        // Token: 0x040004FC RID: 1276
        public int int_0 = -1;

        // Token: 0x040004FD RID: 1277
        public byte byte_0 = 1;
    }
}