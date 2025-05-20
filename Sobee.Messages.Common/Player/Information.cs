using Sobee.Messaging;

namespace Sobee.Messages.Common.Player
{
    [GAttribute0(29475)]
    public sealed class Information : Message
    {
        // Token: 0x060002F4 RID: 756 RVA: 0x0000467C File Offset: 0x0000287C
        public override bool vmethod_0()
        {
            return false;
        }

        // Token: 0x060002F5 RID: 757 RVA: 0x0000467F File Offset: 0x0000287F
        public long method_0()
        {
            return long_0;
        }

        // Token: 0x060002F6 RID: 758 RVA: 0x00004687 File Offset: 0x00002887
        public void method_1(long long_1)
        {
            long_0 = long_1;
        }

        // Token: 0x060002F7 RID: 759 RVA: 0x00004690 File Offset: 0x00002890
        public string method_2()
        {
            return string_0;
        }

        // Token: 0x060002F8 RID: 760 RVA: 0x00004698 File Offset: 0x00002898
        public void method_3(string string_6)
        {
            string_0 = string_6;
        }

        // Token: 0x060002F9 RID: 761 RVA: 0x000046A1 File Offset: 0x000028A1
        public Version method_4()
        {
            return version_0;
        }

        // Token: 0x060002FA RID: 762 RVA: 0x000046A9 File Offset: 0x000028A9
        public string method_5()
        {
            return string_1;
        }

        // Token: 0x060002FB RID: 763 RVA: 0x000046B1 File Offset: 0x000028B1
        public string method_6()
        {
            return string_2;
        }

        // Token: 0x060002FC RID: 764 RVA: 0x000046B9 File Offset: 0x000028B9
        public int method_7()
        {
            return int_1;
        }

        // Token: 0x060002FD RID: 765 RVA: 0x000046C1 File Offset: 0x000028C1
        public string method_8()
        {
            return string_3;
        }

        // Token: 0x060002FE RID: 766 RVA: 0x000046C9 File Offset: 0x000028C9
        public string method_9()
        {
            return string_4;
        }

        // Token: 0x060002FF RID: 767 RVA: 0x000046D1 File Offset: 0x000028D1
        public MatchEntry method_10()
        {
            return matchEntry;
        }

        // Token: 0x06000301 RID: 769 RVA: 0x000046E2 File Offset: 0x000028E2
        public string method_12()
        {
            return string_5;
        }

        // Token: 0x06000302 RID: 770 RVA: 0x0000D1C4 File Offset: 0x0000B3C4
        public Information(BinaryReader gclass315_0) : base(gclass315_0)
        {
            version_0 = gclass315_0.method_24();
            long_0 = gclass315_0.method_10();
            string_0 = gclass315_0.method_14();
            string_1 = gclass315_0.method_14();
            string_2 = gclass315_0.method_14();
            int_1 = gclass315_0.method_9();
            string_3 = gclass315_0.method_14();
            string_4 = gclass315_0.method_14();
            matchEntry = new MatchEntry(gclass315_0);
            string_5 = gclass315_0.method_14();
            bool_0 = gclass315_0.method_1();
        }

        // Token: 0x06000303 RID: 771 RVA: 0x0000D25C File Offset: 0x0000B45C
        public Information(Version version_1, long long_1, string string_6, string string_7, string string_8, int int_3, string string_9, string string_10, MatchEntry int_4, string string_11, bool bool_1)
        {
            version_0 = version_1;
            long_0 = long_1;
            string_0 = string_6;
            string_1 = string_7;
            string_2 = string_8;
            int_1 = int_3;
            string_3 = string_9;
            string_4 = string_10;
            matchEntry = int_4;
            string_5 = string_11;
            bool_0 = bool_1;
        }

        // Token: 0x06000304 RID: 772 RVA: 0x0000D2C4 File Offset: 0x0000B4C4
        public override void Deserialize(BinaryWriter gclass316_0)
        {
            base.Deserialize(gclass316_0);
            gclass316_0.method_23(version_0);
            gclass316_0.method_10(long_0);
            gclass316_0.method_14(string_0);
            gclass316_0.method_14(string_1);
            gclass316_0.method_14(string_2);
            gclass316_0.method_9(int_1);
            gclass316_0.method_14(string_3);
            gclass316_0.method_14(string_4);
            matchEntry.Deserialize(gclass316_0);
            gclass316_0.method_14(string_5);
            gclass316_0.method_1(bool_0);
        }

        // Token: 0x06000305 RID: 773 RVA: 0x0000D35C File Offset: 0x0000B55C
        public string ToString()
        {
            return string.Concat(new object[]
            {
            "Ver:",
            version_0,
            " MP: ",
            long_0,
            " Pass: ",
            string_0,
            " EntryID:",
            matchEntry,
            " WEB:",
            string_5,
            " Auto:",
            bool_0
            });
        }

        // Token: 0x0400064E RID: 1614
        private Version version_0;

        // Token: 0x0400064F RID: 1615
        private long long_0;

        // Token: 0x04000650 RID: 1616
        private string string_0;

        // Token: 0x04000651 RID: 1617
        private string string_1;

        // Token: 0x04000652 RID: 1618
        private string string_2;

        // Token: 0x04000653 RID: 1619
        private int int_1;

        // Token: 0x04000654 RID: 1620
        private string string_3;

        // Token: 0x04000655 RID: 1621
        private string string_4;

        // Token: 0x04000656 RID: 1622
        private readonly MatchEntry matchEntry;

        // Token: 0x04000657 RID: 1623
        private string string_5;

        // Token: 0x04000658 RID: 1624
        public bool bool_0;
    }
}
