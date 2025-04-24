using System.Numerics;
using Sobee.Serialization.GameServer;

namespace Sobee.Messages.Common.Match
{
    // Token: 0x02000065 RID: 101
    [GAttribute0(12835)]
    public class Information : Message
    {
        // Token: 0x060001F2 RID: 498 RVA: 0x00003D6B File Offset: 0x00001F6B
        public GClass170 method_0()
        {
            return gclass170_0;
        }

        // Token: 0x060001F3 RID: 499 RVA: 0x00003D73 File Offset: 0x00001F73
        public GClass166 method_1()
        {
            return gclass166_0;
        }

        // Token: 0x060001F4 RID: 500 RVA: 0x00003D7B File Offset: 0x00001F7B
        public MatchFieldPositioning method_2()
        {
            return matchFieldPositioning_0;
        }

        // Token: 0x060001F5 RID: 501 RVA: 0x00003D83 File Offset: 0x00001F83
        public MatchStateType method_3()
        {
            return matchStateType_0;
        }

        // Token: 0x060001F6 RID: 502 RVA: 0x00003D8B File Offset: 0x00001F8B
        public IEnumerable<Player.MatchInformation> method_4()
        {
            return HomeTeam;
        }

        // Token: 0x060001F7 RID: 503 RVA: 0x00003D93 File Offset: 0x00001F93
        public IEnumerable<Player.MatchInformation> method_5()
        {
            return AwayTeam;
        }

        // Token: 0x060001F8 RID: 504 RVA: 0x00003D9B File Offset: 0x00001F9B
        public IEnumerable<Player.MatchInformation> method_6()
        {
            return HomeSpectator;
        }

        // Token: 0x060001F9 RID: 505 RVA: 0x00003DA3 File Offset: 0x00001FA3
        public IEnumerable<Player.MatchInformation> method_7()
        {
            return AwaySpectator;
        }

        // Token: 0x060001FA RID: 506 RVA: 0x00003DAB File Offset: 0x00001FAB
        public Vector3 method_8()
        {
            return BallPosition;
        }

        // Token: 0x060001FB RID: 507 RVA: 0x00003DB3 File Offset: 0x00001FB3
        public Vector3 method_9()
        {
            return BallVelocity;
        }

        // Token: 0x060001FC RID: 508 RVA: 0x00003DBB File Offset: 0x00001FBB
        public MatchActor GetActor()
        {
            return Actor;
        }

        // Token: 0x060001FE RID: 510 RVA: 0x00003DCB File Offset: 0x00001FCB
        public GClass167 method_12()
        {
            return gclass167_0;
        }

        // Token: 0x060001FF RID: 511 RVA: 0x00003DD3 File Offset: 0x00001FD3
        public double method_13()
        {
            return double_0;
        }

        // Token: 0x06000200 RID: 512 RVA: 0x00003DDB File Offset: 0x00001FDB
        public float method_14()
        {
            return float_0;
        }

        // Token: 0x06000201 RID: 513 RVA: 0x00003DE3 File Offset: 0x00001FE3
        public IEnumerable<string> method_15()
        {
            return ilist_4;
        }

        // Token: 0x06000202 RID: 514 RVA: 0x00003DEB File Offset: 0x00001FEB
        public IEnumerable<string> method_16()
        {
            return ilist_5;
        }

        // Token: 0x06000203 RID: 515 RVA: 0x00003DF3 File Offset: 0x00001FF3
        public GClass156 method_17()
        {
            return gclass156_0;
        }

        // Token: 0x06000204 RID: 516 RVA: 0x00003DFB File Offset: 0x00001FFB
        public List<GClass163> method_18()
        {
            return list_0;
        }

        // Token: 0x06000205 RID: 517 RVA: 0x00003E03 File Offset: 0x00002003
        public List<GClass172> method_19()
        {
            return list_1;
        }

        // Token: 0x06000206 RID: 518 RVA: 0x00003E0B File Offset: 0x0000200B
        public UserSessionRights method_20()
        {
            return userSessionRights_0;
        }

        // Token: 0x06000207 RID: 519 RVA: 0x00003E13 File Offset: 0x00002013
        public GClass171 method_21()
        {
            return gclass171_0;
        }

        // Token: 0x06000208 RID: 520 RVA: 0x00003E1B File Offset: 0x0000201B
        public string method_22()
        {
            return string_0;
        }

        // Token: 0x06000209 RID: 521 RVA: 0x00003E23 File Offset: 0x00002023
        public string method_23()
        {
            return string_1;
        }

        // Token: 0x0600020A RID: 522 RVA: 0x0000ADE4 File Offset: 0x00008FE4
        public Information(BinaryReader gclass315_0) : base(gclass315_0)
        {
            Actor = new MatchActor(gclass315_0);

            ushort num = gclass315_0.method_15();
            HomeTeam = new List<Player.MatchInformation>(num);
            for (int i = 0; i < num; i++)
            {
                HomeTeam.Add((Player.MatchInformation)gclass315_0.method_25());
            }

            num = gclass315_0.method_15();
            AwayTeam = new List<Player.MatchInformation>(num);
            for (int j = 0; j < num; j++)
            {
                AwayTeam.Add((Player.MatchInformation)gclass315_0.method_25());
            }

            num = gclass315_0.method_15();
            HomeSpectator = new List<Player.MatchInformation>(num);
            for (int k = 0; k < num; k++)
            {
                HomeSpectator.Add((Player.MatchInformation)gclass315_0.method_25());
            }

            num = gclass315_0.method_15();
            AwaySpectator = new List<Player.MatchInformation>(num);
            for (int l = 0; l < num; l++)
            {
                AwaySpectator.Add((Player.MatchInformation)gclass315_0.method_25());
            }

            BallPosition = gclass315_0.method_20();
            BallVelocity = gclass315_0.method_20();
            matchStateType_0 = (MatchStateType)gclass315_0.method_9();
            matchFieldPositioning_0 = (MatchFieldPositioning)gclass315_0.method_9();
            gclass166_0 = (GClass166)gclass315_0.method_25();
            gclass170_0 = (GClass170)gclass315_0.method_25();
            gclass167_0 = (GClass167)gclass315_0.method_25();
            float_0 = gclass315_0.method_12();
            double_0 = gclass315_0.method_7();
            num = gclass315_0.method_15();
            ilist_4 = new List<string>(num);
            for (int m = 0; m < num; m++)
            {
                ilist_4.Add(gclass315_0.method_14());
            }
            num = gclass315_0.method_15();
            ilist_5 = new List<string>(num);
            for (int n = 0; n < num; n++)
            {
                ilist_5.Add(gclass315_0.method_14());
            }
            gclass156_0 = (GClass156)gclass315_0.method_25();
            num = gclass315_0.method_15();
            list_0 = new List<GClass163>(num);
            for (int num2 = 0; num2 < num; num2++)
            {
                list_0.Add((GClass163)gclass315_0.method_25());
            }
            num = gclass315_0.method_15();
            list_1 = new List<GClass172>(num);
            for (int num3 = 0; num3 < num; num3++)
            {
                list_1.Add((GClass172)gclass315_0.method_25());
            }
            userSessionRights_0 = (UserSessionRights)gclass315_0.method_25();
            gclass171_0 = (GClass171)gclass315_0.method_25();
            string_0 = gclass315_0.method_14();
            string_1 = gclass315_0.method_14();
        }

        // Token: 0x0600020B RID: 523 RVA: 0x0000B094 File Offset: 0x00009294
        public Information(
            IEnumerable<Player.MatchInformation> ienumerable_0,
            IEnumerable<Player.MatchInformation> ienumerable_1,
            IEnumerable<Player.MatchInformation> ienumerable_2,
            IEnumerable<Player.MatchInformation> ienumerable_3,
            MatchActor Actor,
            Vector3 vector3_2,
            Vector3 vector3_3,
            MatchStateType matchStateType_1,
            MatchFieldPositioning matchFieldPositioning_1,
            GClass166 gclass166_1,
            GClass170 gclass170_1,
            GClass167 gclass167_1,
            double double_1,
            float float_1,
            IEnumerable<string> ienumerable_4,
            IEnumerable<string> ienumerable_5, GClass156 gclass156_1,
            IEnumerable<GClass163> ienumerable_6, IEnumerable<GClass172> ienumerable_7,
            UserSessionRights userSessionRights_1,
            GClass171 gclass171_1,
            string string_2,
            string string_3
            )
        {
            this.Actor = Actor;
            HomeTeam = new List<Player.MatchInformation>(ienumerable_0);
            AwayTeam = new List<Player.MatchInformation>(ienumerable_1);
            HomeSpectator = new List<Player.MatchInformation>(ienumerable_2);
            AwaySpectator = new List<Player.MatchInformation>(ienumerable_3);
            BallPosition = vector3_2;
            BallVelocity = vector3_3;
            matchStateType_0 = matchStateType_1;
            matchFieldPositioning_0 = matchFieldPositioning_1;
            gclass166_0 = gclass166_1;
            gclass170_0 = gclass170_1;
            gclass167_0 = gclass167_1;
            double_0 = double_1;
            float_0 = float_1;
            ilist_4 = new List<string>(ienumerable_4);
            ilist_5 = new List<string>(ienumerable_5);
            gclass156_0 = gclass156_1;
            list_0 = new List<GClass163>(ienumerable_6);
            list_1 = new List<GClass172>(ienumerable_7);
            userSessionRights_0 = userSessionRights_1;
            gclass171_0 = gclass171_1;
            string_0 = string_2;
            string_1 = string_3;
        }

        // Token: 0x0600020C RID: 524 RVA: 0x0000B194 File Offset: 0x00009394
        public override void Deserialize(BinaryWriter gclass316_0)
        {
            base.Deserialize(gclass316_0);
            Actor.Deserialize(gclass316_0);
            gclass316_0.method_15((ushort)HomeTeam.Count);
            for (int i = 0; i < HomeTeam.Count; i++)
            {
                gclass316_0.method_25(HomeTeam[i]);
            }
            gclass316_0.method_15((ushort)AwayTeam.Count);
            for (int j = 0; j < AwayTeam.Count; j++)
            {
                gclass316_0.method_25(AwayTeam[j]);
            }
            gclass316_0.method_15((ushort)HomeSpectator.Count);
            for (int k = 0; k < HomeSpectator.Count; k++)
            {
                gclass316_0.method_25(HomeSpectator[k]);
            }
            gclass316_0.method_15((ushort)AwaySpectator.Count);
            for (int l = 0; l < AwaySpectator.Count; l++)
            {
                gclass316_0.method_25(AwaySpectator[l]);
            }
            gclass316_0.method_20(BallPosition);
            gclass316_0.method_20(BallVelocity);
            gclass316_0.method_9((int)matchStateType_0);
            gclass316_0.method_9((int)matchFieldPositioning_0);
            gclass316_0.method_25(gclass166_0);
            gclass316_0.method_25(gclass170_0);
            gclass316_0.method_25(gclass167_0);
            gclass316_0.method_12(float_0);
            gclass316_0.method_7(double_0);
            gclass316_0.method_15((ushort)ilist_4.Count);
            for (int m = 0; m < ilist_4.Count; m++)
            {
                gclass316_0.method_14(ilist_4[m]);
            }
            gclass316_0.method_15((ushort)ilist_5.Count);
            for (int n = 0; n < ilist_5.Count; n++)
            {
                gclass316_0.method_14(ilist_5[n]);
            }
            gclass316_0.method_25(gclass156_0);
            gclass316_0.method_15((ushort)list_0.Count);
            for (int num = 0; num < list_0.Count; num++)
            {
                gclass316_0.method_25(list_0[num]);
            }
            gclass316_0.method_15((ushort)list_1.Count);
            for (int num2 = 0; num2 < list_1.Count; num2++)
            {
                gclass316_0.method_25(list_1[num2]);
            }
            gclass316_0.method_25(userSessionRights_0);
            gclass316_0.method_25(gclass171_0);
            gclass316_0.method_14(string_0);
            gclass316_0.method_14(string_1);
        }

        public static Information testMethod()
        {
            //  ************
            //  Scenario:       Messages.Scenario.Type.ScenarioMatch,
            //  State:          Messages.Match.State.Type.Positioning,
            //  Positioning:    Types.Enums.Positioning.Kickoff,
            //  Teams: {
            //      Home: { Color: 5, Name: { Full: 'Gecici Takim 1', Short: 'G1' }, Size: 11 },
            //      Away: { Color: 1, Name: { Full: 'Gecici Takim 2', Short: 'G2' }, Size: 11 },
            //  },

            var gg1 = new GClass166(0);
            gg1.Clear();

            List<Player.MatchInformation> test = new List<Player.MatchInformation>();
            List<Player.MatchInformation> test2 = new List<Player.MatchInformation>();
            for (int i = 0; i < 11; i++)
            {
                test.Add(new Player.MatchInformation(
                    i, // MatchID
                    i, // PlayerID
                    "Player" + i, // PlayerName
                    StadiumSitting.HomePlayer, // StadiumSitting
                    (sbyte)MatchEntry.ToSquad(i + 1), // SquadNumber
                    new Vector2(0, 0), // Vector2
                    new Vector3(0, 0, 0), // Vector3
                    0f, // float_0
                    MatchCard.None, // CardStatus
                    string.Empty // string_3
                ));

                test2.Add(new Player.MatchInformation(
                    12 + i, // MatchID
                    12 + i, // PlayerID
                    "Player" + i, // PlayerName
                     StadiumSitting.AwayPlayer, // StadiumSitting
                    (sbyte)MatchEntry.ToSquad(12 + (i + 1)), // SquadNumber
                    new Vector2(0, 0), // Vector2
                    new Vector3(0, 0, 0), // Vector3
                    0f, // float_0
                    MatchCard.None, // CardStatus
                    string.Empty // string_3
                ));
            }

            return new Information(
                test,   // Home team players
                test2,   // Away team players
                Enumerable.Empty<Player.MatchInformation>(),   // Home spectators
                Enumerable.Empty<Player.MatchInformation>(),   // Away spectators
                new MatchActor(1, 1, 1),                    // Actor
                new Vector3(0, 0, 0),                          // Ball position
                new Vector3(0, 0, 0),                          // Ball velocity
                MatchStateType.Positioning,                    // Match state (default)
                MatchFieldPositioning.Kickoff,                 // Field positioning (default)
                gg1,                                           // gclass166
                GClass170.Default(),                           // gclass170
                GClass167.Default(),                           // gclass167
                0.0,                                           // double_0 (some numeric data)
                0f,                                            // float_0 (maybe time or duration)
                Enumerable.Empty<string>(),                    // ilist_4 (string data 1)
                Enumerable.Empty<string>(),                    // ilist_5 (string data 2)
                GClass156.Default(),                           // gclass156
                new List<GClass163>(),                         // list_0
                new List<GClass172>(),                         // list_1
                new UserSessionRights(),                       // user session rights
                new GClass171(),                               // gclass171
                string.Empty,                                  // string_0
                string.Empty                                   // string_1
            );
        }

        // Token: 0x040005C6 RID: 1478
        private MatchActor Actor;

        // Token: 0x040005C9 RID: 1481
        private IList<Player.MatchInformation> HomeTeam;

        // Token: 0x040005CA RID: 1482
        private IList<Player.MatchInformation> AwayTeam;

        // Token: 0x040005CB RID: 1483
        private IList<Player.MatchInformation> HomeSpectator;

        // Token: 0x040005CC RID: 1484
        private IList<Player.MatchInformation> AwaySpectator;

        // Token: 0x040005CD RID: 1485
        private Vector3 BallPosition;

        // Token: 0x040005CE RID: 1486
        private Vector3 BallVelocity;

        // Token: 0x040005CF RID: 1487
        private MatchStateType matchStateType_0;

        // Token: 0x040005D0 RID: 1488
        private MatchFieldPositioning matchFieldPositioning_0;

        // Token: 0x040005D1 RID: 1489
        private GClass166 gclass166_0;

        // Token: 0x040005D2 RID: 1490
        private GClass170 gclass170_0;

        // Token: 0x040005D3 RID: 1491
        private GClass167 gclass167_0;

        // Token: 0x040005D4 RID: 1492
        private float float_0;

        // Token: 0x040005D5 RID: 1493
        private double double_0;

        // Token: 0x040005D6 RID: 1494
        private IList<string> ilist_4;

        // Token: 0x040005D7 RID: 1495
        private IList<string> ilist_5;

        // Token: 0x040005D8 RID: 1496
        private GClass156 gclass156_0;

        // Token: 0x040005D9 RID: 1497
        private List<GClass163> list_0;

        // Token: 0x040005DA RID: 1498
        private List<GClass172> list_1;

        // Token: 0x040005DB RID: 1499
        private UserSessionRights userSessionRights_0;

        // Token: 0x040005DC RID: 1500
        private GClass171 gclass171_0;

        // Token: 0x040005DD RID: 1501
        private string string_0;

        // Token: 0x040005DE RID: 1502
        private string string_1;
    }
}
