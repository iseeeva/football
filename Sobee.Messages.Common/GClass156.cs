using Sobee.Messaging;

namespace Sobee.Messages.Common
{

    // Token: 0x02000025 RID: 37
    [GAttribute0(4932)]
    public class GClass156 : Message
    {
        // Token: 0x06000032 RID: 50 RVA: 0x00002431 File Offset: 0x00000631
        public int method_0()
        {
            return int_0;
        }

        // Token: 0x06000033 RID: 51 RVA: 0x00002439 File Offset: 0x00000639
        public int method_1()
        {
            return int_1;
        }

        // Token: 0x06000034 RID: 52 RVA: 0x00002441 File Offset: 0x00000641
        public int method_2()
        {
            return int_2;
        }

        // Token: 0x06000035 RID: 53 RVA: 0x00002449 File Offset: 0x00000649
        public int method_3()
        {
            return int_3;
        }

        // Token: 0x06000036 RID: 54 RVA: 0x00002451 File Offset: 0x00000651
        public int method_4()
        {
            return int_4;
        }

        // Token: 0x06000037 RID: 55 RVA: 0x00002459 File Offset: 0x00000659
        public int method_5()
        {
            return int_5;
        }

        public GClass156()
        {

        }

        // Token: 0x06000038 RID: 56 RVA: 0x000061D8 File Offset: 0x000043D8
        public GClass156(BinaryReader gclass315_0)
        {
            int_0 = gclass315_0.method_9();
            int_1 = gclass315_0.method_9();
            int_2 = gclass315_0.method_9();
            int_3 = gclass315_0.method_9();
            int_4 = gclass315_0.method_9();
            int_5 = gclass315_0.method_9();
        }

        // Token: 0x06000039 RID: 57 RVA: 0x00002461 File Offset: 0x00000661
        public GClass156(int int_6, int int_7, int int_8, int int_9, int int_10, int int_11)
        {
            int_0 = int_6;
            int_1 = int_8;
            int_2 = int_7;
            int_3 = int_9;
            int_4 = int_10;
            int_5 = int_11;
        }

        // Token: 0x0600003A RID: 58 RVA: 0x00006234 File Offset: 0x00004434
        public override void Deserialize(BinaryWriter gclass316_0)
        {
            gclass316_0.method_9(int_0);
            gclass316_0.method_9(int_1);
            gclass316_0.method_9(int_2);
            gclass316_0.method_9(int_3);
            gclass316_0.method_9(int_4);
            gclass316_0.method_9(int_5);
        }

        public static GClass156 Default()
        {
            return new GClass156()
            {
                int_0 = 0,
                int_1 = 0,
                int_2 = 0,
                int_3 = 0,
                int_4 = 0,
                int_5 = 0,
            };
        }

        // Token: 0x04000450 RID: 1104
        private int int_0;

        // Token: 0x04000451 RID: 1105
        private int int_1;

        // Token: 0x04000452 RID: 1106
        private int int_2;

        // Token: 0x04000453 RID: 1107
        private int int_3;

        // Token: 0x04000454 RID: 1108
        private int int_4;

        // Token: 0x04000455 RID: 1109
        private int int_5;
    }
}