namespace Sobee.Serialization
{
    public class MessageHelper
    {
        // Token: 0x06000029 RID: 41 RVA: 0x000021A4 File Offset: 0x000003A4
        public MessageHelper(Stream stream_0, GDelegate1 gdelegate1_1, GDelegate2 gdelegate2_1)
        {
            gclass316_0 = new BinaryWriter(stream_0, gdelegate1_1, gdelegate2_1);
            gclass315_0 = new BinaryReader(stream_0, gdelegate1_1, gdelegate2_1);
            gdelegate1_0 = gdelegate1_1;
            gdelegate2_0 = gdelegate2_1;
        }

        // Token: 0x0600002A RID: 42 RVA: 0x00002C30 File Offset: 0x00000E30
        public void WriteMessage(IMessage ginterface7_0)
        {
            ushort num = gdelegate2_0(ginterface7_0.GetType());
            num ^= 7779;
            gclass316_0.method_15(num);
            ginterface7_0.Serialize(gclass316_0);
        }

        // Token: 0x0600002B RID: 43 RVA: 0x00002C70 File Offset: 0x00000E70
        public IMessage ReadMessage()
        {
            if (gclass315_0 == null)
            {
                throw new ArgumentException("Stream was not readable.");
            }
            ushort num = gclass315_0.method_15();
            num ^= 7779;
            return (IMessage)gdelegate1_0(num, gclass315_0);
        }

        // Token: 0x0600002C RID: 44 RVA: 0x000021D6 File Offset: 0x000003D6
        public MessageHelper Dispose()
        {
            gclass316_0.method_0();
            gclass315_0.method_0();
            return this;
        }

        // Token: 0x04000010 RID: 16
        private BinaryWriter gclass316_0;

        // Token: 0x04000011 RID: 17
        private BinaryReader gclass315_0;

        // Token: 0x04000012 RID: 18
        private GDelegate1 gdelegate1_0;

        // Token: 0x04000013 RID: 19
        private GDelegate2 gdelegate2_0;
    }
}