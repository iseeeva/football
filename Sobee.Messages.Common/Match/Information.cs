using System.Numerics;
using Sobee.Serialization.GameServer;

namespace Sobee.Messages.Common.Match
{
    [GAttribute0(12835)]
    public class Information : Message
    {
        public GClass170 method_0()
        {
            return gclass170_0;
        }

        public GClass166 method_1()
        {
            return gclass166_0;
        }

        public MatchFieldPositioning method_2()
        {
            return matchFieldPositioning_0;
        }

        public MatchStateType method_3()
        {
            return matchStateType_0;
        }

        public IEnumerable<Player.MatchInformation> method_4()
        {
            return HomeTeam;
        }

        public IEnumerable<Player.MatchInformation> method_5()
        {
            return AwayTeam;
        }

        public IEnumerable<Player.MatchInformation> method_6()
        {
            return HomeSpectator;
        }

        public IEnumerable<Player.MatchInformation> method_7()
        {
            return AwaySpectator;
        }

        public Vector3 method_8()
        {
            return BallPosition;
        }

        public Vector3 method_9()
        {
            return BallVelocity;
        }

        public MatchActor GetActor()
        {
            return Actor;
        }

        public GClass167 method_12()
        {
            return gclass167_0;
        }

        public double method_13()
        {
            return double_0;
        }

        public float method_14()
        {
            return float_0;
        }

        public IEnumerable<string> method_15()
        {
            return ilist_4;
        }

        public IEnumerable<string> method_16()
        {
            return ilist_5;
        }

        public GClass156 method_17()
        {
            return gclass156_0;
        }

        public List<GClass163> method_18()
        {
            return list_0;
        }

        public List<GClass172> method_19()
        {
            return list_1;
        }

        public UserSessionRights method_20()
        {
            return userSessionRights_0;
        }

        public GClass171 method_21()
        {
            return gclass171_0;
        }

        public string method_22()
        {
            return string_0;
        }

        public string method_23()
        {
            return string_1;
        }

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
            var gg1 = new GClass166(0);
            gg1.Clear();

            var homeTeam = Enumerable.Range(1, 5).Select(i => new Player.MatchInformation(
                i, i, $"Player{i}", StadiumSitting.HomePlayer,
                (sbyte)MatchEntry.ToSquad(i, true), new Vector2(0, 0),
                new Vector3(0, 0, 0), 0f, MatchCard.None, "<XMLData><Script></Script></XMLData>")).ToList();

            var awayTeam = Enumerable.Range(12, 5).Select(i => new Player.MatchInformation(
                i, i, $"Player{i}", StadiumSitting.AwayPlayer,
                (sbyte)MatchEntry.ToSquad(i, true), new Vector2(0, 0),
                new Vector3(0, 0, 0), 0f, MatchCard.None, "<XMLData><Script></Script></XMLData>")).ToList();

            return new Information(
                homeTeam, awayTeam, Enumerable.Empty<Player.MatchInformation>(),
                Enumerable.Empty<Player.MatchInformation>(), new MatchActor(1, 1, 1),
                new Vector3(0, 0, 0), new Vector3(0, 0, 0), MatchStateType.Positioning,
                MatchFieldPositioning.Kickoff, gg1, GClass170.Default(),
                GClass167.Default(), 0.0, 0f, Enumerable.Empty<string>(),
                Enumerable.Empty<string>(), GClass156.Default(), new List<GClass163>(),
                new List<GClass172>(), new UserSessionRights(), new GClass171(),
                string.Empty, string.Empty);
        }

        private MatchActor Actor;

        private IList<Player.MatchInformation> HomeTeam;

        private IList<Player.MatchInformation> AwayTeam;

        private IList<Player.MatchInformation> HomeSpectator;

        private IList<Player.MatchInformation> AwaySpectator;

        private Vector3 BallPosition;

        private Vector3 BallVelocity;

        private MatchStateType matchStateType_0;

        private MatchFieldPositioning matchFieldPositioning_0;

        private GClass166 gclass166_0;

        private GClass170 gclass170_0;

        private GClass167 gclass167_0;

        private float float_0;

        private double double_0;

        private IList<string> ilist_4;

        private IList<string> ilist_5;

        private GClass156 gclass156_0;

        private List<GClass163> list_0;

        private List<GClass172> list_1;

        private UserSessionRights userSessionRights_0;

        private GClass171 gclass171_0;

        private string string_0;

        private string string_1;
    }
}
