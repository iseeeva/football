// Token: 0x02000066 RID: 102
namespace Sobee.Messages.Common
{
    [GAttribute0(12908)]
    public class GClass166 : Message
    {
        // Token: 0x0600020D RID: 525 RVA: 0x00003E2B File Offset: 0x0000202B
        public double method_0()
        {
            return MatchTime;
        }

        // Token: 0x0600020E RID: 526 RVA: 0x00003E33 File Offset: 0x00002033
        public void method_1(double double_1)
        {
            MatchTime = double_1;
        }

        // Token: 0x0600020F RID: 527 RVA: 0x00003E3C File Offset: 0x0000203C
        public byte method_2()
        {
            return ScoreAway;
        }

        // Token: 0x06000210 RID: 528 RVA: 0x00003E44 File Offset: 0x00002044
        public void method_3(byte byte_4)
        {
            ScoreAway = byte_4;
        }

        // Token: 0x06000211 RID: 529 RVA: 0x00003E4D File Offset: 0x0000204D
        public byte method_4()
        {
            return ScoreHome;
        }

        // Token: 0x06000212 RID: 530 RVA: 0x00003E55 File Offset: 0x00002055
        public void method_5(byte byte_4)
        {
            ScoreHome = byte_4;
        }

        // Token: 0x06000213 RID: 531 RVA: 0x00003E5E File Offset: 0x0000205E
        public MatchPhase method_6()
        {
            return MatchPhase;
        }

        // Token: 0x06000214 RID: 532 RVA: 0x00003E66 File Offset: 0x00002066
        public void method_7(MatchPhase matchPhase_1)
        {
            MatchPhase = matchPhase_1;
        }

        // Token: 0x06000215 RID: 533 RVA: 0x00003E6F File Offset: 0x0000206F
        public bool method_8()
        {
            return MatchPhase == MatchPhase.FirstHalf;
        }

        // Token: 0x06000216 RID: 534 RVA: 0x00003E7A File Offset: 0x0000207A
        public bool method_9()
        {
            return MatchPhase == MatchPhase.SecondHalf;
        }

        // Token: 0x06000217 RID: 535 RVA: 0x00003E85 File Offset: 0x00002085
        public bool[] method_10()
        {
            return bool_0;
        }

        // Token: 0x06000218 RID: 536 RVA: 0x00003E8D File Offset: 0x0000208D
        public bool[] method_11()
        {
            return bool_1;
        }

        // Token: 0x06000219 RID: 537 RVA: 0x00003E95 File Offset: 0x00002095
        public TimeSpan method_12()
        {
            return TimeSpan.FromSeconds(method_0());
        }

        // Token: 0x0600021A RID: 538 RVA: 0x00003EA2 File Offset: 0x000020A2
        public int method_13()
        {
            return int_0;
        }

        // Token: 0x0600021B RID: 539 RVA: 0x00003EAA File Offset: 0x000020AA
        public void method_14(int int_4)
        {
            int_0 = int_4;
        }

        // Token: 0x0600021C RID: 540 RVA: 0x00003EB3 File Offset: 0x000020B3
        public int method_15()
        {
            return int_1;
        }

        // Token: 0x0600021D RID: 541 RVA: 0x00003EBB File Offset: 0x000020BB
        public void method_16(int int_4)
        {
            int_1 = int_4;
        }

        // Token: 0x0600021E RID: 542 RVA: 0x00003EC4 File Offset: 0x000020C4
        public int method_17()
        {
            return int_2;
        }

        // Token: 0x0600021F RID: 543 RVA: 0x00003ECC File Offset: 0x000020CC
        public void method_18(int int_4)
        {
            int_2 = int_4;
        }

        // Token: 0x06000220 RID: 544 RVA: 0x00003ED5 File Offset: 0x000020D5
        public int method_19()
        {
            return int_3;
        }

        // Token: 0x06000221 RID: 545 RVA: 0x00003EDD File Offset: 0x000020DD
        public void method_20(int int_4)
        {
            int_3 = int_4;
        }

        // Token: 0x06000222 RID: 546 RVA: 0x00003EE6 File Offset: 0x000020E6
        public int method_21()
        {
            return (int)(MatchTime * 1000.0);
        }

        // Token: 0x06000223 RID: 547 RVA: 0x00003EF9 File Offset: 0x000020F9
        public byte method_22()
        {
            return byte_2;
        }

        // Token: 0x06000224 RID: 548 RVA: 0x00003F01 File Offset: 0x00002101
        public void method_23(byte byte_4)
        {
            byte_2 = byte_4;
        }

        // Token: 0x06000225 RID: 549 RVA: 0x00003F0A File Offset: 0x0000210A
        public byte method_24()
        {
            return byte_3;
        }

        // Token: 0x06000226 RID: 550 RVA: 0x00003F12 File Offset: 0x00002112
        public void method_25(byte byte_4)
        {
            byte_3 = byte_4;
        }

        // Token: 0x06000227 RID: 551 RVA: 0x00003F1B File Offset: 0x0000211B
        public short method_26()
        {
            return short_0;
        }

        // Token: 0x06000228 RID: 552 RVA: 0x00003F23 File Offset: 0x00002123
        public void method_27(short short_2)
        {
            short_0 = short_2;
        }

        // Token: 0x06000229 RID: 553 RVA: 0x00003F2C File Offset: 0x0000212C
        public short method_28()
        {
            return short_1;
        }

        // Token: 0x0600022A RID: 554 RVA: 0x00003F34 File Offset: 0x00002134
        public void method_29(short short_2)
        {
            short_1 = short_2;
        }

        // Token: 0x0600022B RID: 555 RVA: 0x00003F3D File Offset: 0x0000213D
        public GClass166(int int_4)
        {
            bool_0 = new bool[int_4];
            bool_1 = new bool[int_4];
        }

        // Token: 0x0600022C RID: 556 RVA: 0x0000B458 File Offset: 0x00009658
        public GClass166(BinaryReader gclass315_0)
        {
            ScoreHome = gclass315_0.method_2();
            ScoreAway = gclass315_0.method_2();
            MatchTime = gclass315_0.method_7();
            MatchPhase = (MatchPhase)gclass315_0.method_9();
            ushort num = gclass315_0.method_15();
            bool_0 = new bool[num];
            for (int i = 0; i < num; i++)
            {
                bool_0[i] = gclass315_0.method_1();
            }
            num = gclass315_0.method_15();
            bool_1 = new bool[num];
            for (int j = 0; j < num; j++)
            {
                bool_1[j] = gclass315_0.method_1();
            }
            byte_2 = gclass315_0.method_2();
            byte_3 = gclass315_0.method_2();
            short_0 = gclass315_0.method_8();
            short_1 = gclass315_0.method_8();
        }

        // Token: 0x0600022D RID: 557 RVA: 0x0000B528 File Offset: 0x00009728
        public string method_30()
        {
            int num = (int)method_0() / 60;
            int num2 = (int)method_0() % 60;
            int num3 = num / 10;
            num %= 10;
            int num4 = num2 / 10;
            num2 %= 10;
            return string.Format("{0:D}{1:D}:{2:D}{3:D}", [num3, num, num4, num2]);
        }

        // Token: 0x0600022E RID: 558 RVA: 0x0000B59C File Offset: 0x0000979C
        public override void Deserialize(BinaryWriter gclass316_0)
        {
            gclass316_0.method_2(ScoreHome);
            gclass316_0.method_2(ScoreAway);
            gclass316_0.method_7(MatchTime);
            gclass316_0.method_9((int)MatchPhase);
            gclass316_0.method_15((ushort)bool_0.Length);
            for (int i = 0; i < bool_0.Length; i++)
            {
                gclass316_0.method_1(bool_0[i]);
            }
            gclass316_0.method_15((ushort)bool_1.Length);
            for (int j = 0; j < bool_1.Length; j++)
            {
                gclass316_0.method_1(bool_1[j]);
            }
            gclass316_0.method_2(byte_2);
            gclass316_0.method_2(byte_3);
            gclass316_0.method_8(short_0);
            gclass316_0.method_8(short_1);
        }

        // Token: 0x0600022F RID: 559 RVA: 0x0000B66C File Offset: 0x0000986C
        public void Clear()
        {
            ScoreHome = 0;
            ScoreAway = 0;
            MatchTime = 0.0;
            MatchPhase = MatchPhase.FirstHalf;
            int_0 = 0;
            int_1 = 0;
            int_2 = 0;
            int_3 = 0;
            bool_0 = new bool[bool_0.Length];
            bool_1 = new bool[bool_1.Length];
            byte_2 = 0;
            byte_3 = 0;
            short_0 = 0;
            short_1 = 0;
        }

        // Token: 0x040005DF RID: 1503
        private byte ScoreHome;

        // Token: 0x040005E0 RID: 1504
        private byte ScoreAway;

        // Token: 0x040005E1 RID: 1505
        private double MatchTime;

        // Token: 0x040005E2 RID: 1506
        private MatchPhase MatchPhase;

        // Token: 0x040005E3 RID: 1507
        private bool[] bool_0;

        // Token: 0x040005E4 RID: 1508
        private bool[] bool_1;

        // Token: 0x040005E5 RID: 1509
        private int int_0;

        // Token: 0x040005E6 RID: 1510
        private int int_1;

        // Token: 0x040005E7 RID: 1511
        private int int_2;

        // Token: 0x040005E8 RID: 1512
        private int int_3;

        // Token: 0x040005E9 RID: 1513
        private byte byte_2;

        // Token: 0x040005EA RID: 1514
        private byte byte_3;

        // Token: 0x040005EB RID: 1515
        private short short_0;

        // Token: 0x040005EC RID: 1516
        private short short_1;
    }
}