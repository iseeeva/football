using Football.GameServer.Messages.Match;
using Football.Network.Messaging;
using Football.Serialization;

namespace Football.GameServer.Messages
{

    // Token: 0x02000093 RID: 147
    [Message(21588)]
    public class GClass171 : Message
    {
        // Token: 0x060003A3 RID: 931 RVA: 0x000050FD File Offset: 0x000032FD
        public List<GClass162> method_0()
        {
            return list_0;
        }

        // Token: 0x060003A4 RID: 932 RVA: 0x00005105 File Offset: 0x00003305
        public GClass171()
        {
        }

        // Token: 0x060003A5 RID: 933 RVA: 0x0000DEE8 File Offset: 0x0000C0E8
        public GClass171(BinaryReader gclass315_0)
        {
            int num = gclass315_0.method_9();
            for (int i = 0; i < num; i++)
            {
                method_1((GClass162)gclass315_0.method_25());
            }
        }

        // Token: 0x060003A6 RID: 934 RVA: 0x0000DF2C File Offset: 0x0000C12C
        public override void Serialize(BinaryWriter gclass316_0)
        {
            gclass316_0.method_9(list_0.Count);
            for (int i = 0; i < list_0.Count; i++)
            {
                gclass316_0.method_25(list_0[i]);
            }
        }

        // Token: 0x060003A7 RID: 935 RVA: 0x00005118 File Offset: 0x00003318
        public void method_1(GClass162 gclass162_0)
        {
            list_0.Add(gclass162_0);
        }

        // Token: 0x060003A8 RID: 936 RVA: 0x0000DF74 File Offset: 0x0000C174
        public List<GClass162> method_2()
        {
            List<GClass162> list = new List<GClass162>();
            for (int i = 0; i < list_0.Count; i++)
            {
                if (list_0[i].method_0() < 11 && list_0[i].method_2() == MatchUIEvent.Goal)
                {
                    list.Add(list_0[i]);
                }
                if (list_0[i].method_0() > 10 && list_0[i].method_2() == MatchUIEvent.OwnGoal)
                {
                    list.Add(list_0[i]);
                }
            }
            return list;
        }

        // Token: 0x060003A9 RID: 937 RVA: 0x0000E018 File Offset: 0x0000C218
        public List<GClass162> method_3()
        {
            List<GClass162> list = new List<GClass162>();
            for (int i = 0; i < list_0.Count; i++)
            {
                if (list_0[i].method_0() > 10 && list_0[i].method_2() == MatchUIEvent.Goal)
                {
                    list.Add(list_0[i]);
                }
                if (list_0[i].method_0() < 11 && list_0[i].method_2() == MatchUIEvent.OwnGoal)
                {
                    list.Add(list_0[i]);
                }
            }
            return list;
        }

        // Token: 0x060003AA RID: 938 RVA: 0x0000E0BC File Offset: 0x0000C2BC
        public List<GClass162> method_4(MatchUIEvent matchUIEventType_0)
        {
            List<GClass162> list = new List<GClass162>();
            for (int i = 0; i < list_0.Count; i++)
            {
                if (list_0[i].method_2() == matchUIEventType_0)
                {
                    list.Add(list_0[i]);
                }
            }
            return list;
        }

        // Token: 0x060003AB RID: 939 RVA: 0x0000E10C File Offset: 0x0000C30C
        public List<GClass162> method_5(MatchUIEvent matchUIEventType_0)
        {
            List<GClass162> list = new List<GClass162>();
            for (int i = 0; i < list_0.Count; i++)
            {
                if (list_0[i].method_2() == matchUIEventType_0 && list_0[i].method_0() < 11)
                {
                    list.Add(list_0[i]);
                }
            }
            return list;
        }

        // Token: 0x060003AC RID: 940 RVA: 0x0000E174 File Offset: 0x0000C374
        public List<GClass162> method_6(MatchUIEvent matchUIEventType_0)
        {
            List<GClass162> list = new List<GClass162>();
            for (int i = 0; i < list_0.Count; i++)
            {
                if (list_0[i].method_2() == matchUIEventType_0 && list_0[i].method_0() > 10)
                {
                    list.Add(list_0[i]);
                }
            }
            return list;
        }

        // Token: 0x060003AD RID: 941 RVA: 0x0000E1DC File Offset: 0x0000C3DC
        public List<GClass162> method_7(MatchUIEvent matchUIEventType_0, string string_0)
        {
            List<GClass162> list = new List<GClass162>();
            for (int i = 0; i < list_0.Count; i++)
            {
                if (list_0[i].method_2() == matchUIEventType_0 && list_0[i].method_1() == string_0)
                {
                    list.Add(list_0[i]);
                }
            }
            return list;
        }

        // Token: 0x060003AE RID: 942 RVA: 0x00005126 File Offset: 0x00003326
        public void method_8()
        {
            list_0.Clear();
        }

        // Token: 0x040006C0 RID: 1728
        private List<GClass162> list_0 = new List<GClass162>();
    }
}