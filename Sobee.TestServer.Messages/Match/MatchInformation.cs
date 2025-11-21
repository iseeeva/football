using System.Numerics;
using Sobee.Messaging;
using Sobee.Serialization;
using Sobee.Serialization.GameServer;
using Sobee.TestServer.Messages.Player;

namespace Sobee.TestServer.Messages.Match
{
    [MessageAttribute(12835)]
    public class MatchInformation : Message
    {
        public MatchActor Actor { get; private set; }

        public List<PlayerMatchInformation> HomeTeam { get; private set; }
        public List<PlayerMatchInformation> AwayTeam { get; private set; }
        public List<PlayerMatchInformation> HomeSpectator { get; private set; }
        public List<PlayerMatchInformation> AwaySpectator { get; private set; }

        public Vector3 BallPosition;
        public Vector3 BallVelocity;

        public MatchStateType MatchState;
        public MatchFieldPositioning FieldPositioning;

        public UserSessionRights SessionRights { get; private set; }
        public PhaseInfo PhaseInfo { get; private set; }
        public ScenarioInfo ScenarioInfo { get; private set; }
        public GClass171 Class171 { get; private set; }
        public GClass167 Class167 { get; private set; }
        public TeamIdInfo TeamIdInfo { get; private set; }
        public List<GClass163> List163 { get; private set; }
        public List<GClass172> List172 { get; private set; }

        public float TimeMultiplier { get; private set; }
        public double SomeDouble { get; private set; }

        public List<string> SomeStrings1 { get; private set; }
        public List<string> SomeStrings2 { get; private set; }

        public string SomeString1 { get; private set; }
        public string SomeString2 { get; private set; }

        public MatchInformation()
        {
            // TODO: Remove hardcoded values when possible
            HomeTeam = new List<PlayerMatchInformation>(11);
            AwayTeam = new List<PlayerMatchInformation>(11);
            HomeSpectator = new List<PlayerMatchInformation>(11);
            AwaySpectator = new List<PlayerMatchInformation>(11);

            Actor = new MatchActor(-1, -1, -1);
            BallPosition = Vector3.Zero;
            BallVelocity = Vector3.Zero;
            MatchState = MatchStateType.Positioning;
            FieldPositioning = MatchFieldPositioning.Kickoff;

            SessionRights = new UserSessionRights();
            PhaseInfo = new PhaseInfo(0);
            ScenarioInfo = new ScenarioInfo();
            Class171 = new GClass171();
            Class167 = new GClass167();
            TeamIdInfo = new TeamIdInfo();
            List163 = new List<GClass163>();
            List172 = new List<GClass172>();

            TimeMultiplier = 1f;
            SomeDouble = 0.0;
            SomeStrings1 = new List<string>();
            SomeStrings2 = new List<string>();

            SomeString1 = string.Empty;
            SomeString2 = string.Empty;
        }

        public MatchInformation(BinaryReader gclass315_0) : base(gclass315_0)
        {
            Actor = new MatchActor(gclass315_0);

            ushort num = gclass315_0.method_15();
            HomeTeam = new List<PlayerMatchInformation>(num);
            for (int i = 0; i < num; i++)
            {
                HomeTeam.Add((PlayerMatchInformation)gclass315_0.method_25());
            }

            num = gclass315_0.method_15();
            AwayTeam = new List<PlayerMatchInformation>(num);
            for (int j = 0; j < num; j++)
            {
                AwayTeam.Add((PlayerMatchInformation)gclass315_0.method_25());
            }

            num = gclass315_0.method_15();
            HomeSpectator = new List<PlayerMatchInformation>(num);
            for (int k = 0; k < num; k++)
            {
                HomeSpectator.Add((PlayerMatchInformation)gclass315_0.method_25());
            }

            num = gclass315_0.method_15();
            AwaySpectator = new List<PlayerMatchInformation>(num);
            for (int l = 0; l < num; l++)
            {
                AwaySpectator.Add((PlayerMatchInformation)gclass315_0.method_25());
            }

            BallPosition = gclass315_0.method_20();
            BallVelocity = gclass315_0.method_20();
            MatchState = (MatchStateType)gclass315_0.method_9();
            FieldPositioning = (MatchFieldPositioning)gclass315_0.method_9();
            PhaseInfo = (PhaseInfo)gclass315_0.method_25();
            ScenarioInfo = (ScenarioInfo)gclass315_0.method_25();
            Class167 = (GClass167)gclass315_0.method_25();
            TimeMultiplier = gclass315_0.method_12();
            SomeDouble = gclass315_0.method_7();
            num = gclass315_0.method_15();
            SomeStrings1 = new List<string>(num);
            for (int m = 0; m < num; m++)
            {
                SomeStrings1.Add(gclass315_0.method_14());
            }
            num = gclass315_0.method_15();
            SomeStrings2 = new List<string>(num);
            for (int n = 0; n < num; n++)
            {
                SomeStrings2.Add(gclass315_0.method_14());
            }
            TeamIdInfo = (TeamIdInfo)gclass315_0.method_25();
            num = gclass315_0.method_15();
            List163 = new List<GClass163>(num);
            for (int num2 = 0; num2 < num; num2++)
            {
                List163.Add((GClass163)gclass315_0.method_25());
            }
            num = gclass315_0.method_15();
            List172 = new List<GClass172>(num);
            for (int num3 = 0; num3 < num; num3++)
            {
                List172.Add((GClass172)gclass315_0.method_25());
            }
            SessionRights = (UserSessionRights)gclass315_0.method_25();
            Class171 = (GClass171)gclass315_0.method_25();
            SomeString1 = gclass315_0.method_14();
            SomeString2 = gclass315_0.method_14();
        }

        public MatchInformation(
            IEnumerable<PlayerMatchInformation> ienumerable_0,
            IEnumerable<PlayerMatchInformation> ienumerable_1,
            IEnumerable<PlayerMatchInformation> ienumerable_2,
            IEnumerable<PlayerMatchInformation> ienumerable_3,
            MatchActor Actor,
            Vector3 vector3_2,
            Vector3 vector3_3,
            MatchStateType matchStateType_1,
            MatchFieldPositioning matchFieldPositioning_1,
            PhaseInfo gclass166_1,
            ScenarioInfo gclass170_1,
            GClass167 gclass167_1,
            double double_1,
            float float_1,
            IEnumerable<string> ienumerable_4,
            IEnumerable<string> ienumerable_5, TeamIdInfo gclass156_1,
            IEnumerable<GClass163> ienumerable_6, IEnumerable<GClass172> ienumerable_7,
            UserSessionRights userSessionRights_1,
            GClass171 gclass171_1,
            string string_2,
            string string_3
            )
        {
            this.Actor = Actor;
            HomeTeam = new List<PlayerMatchInformation>(ienumerable_0);
            AwayTeam = new List<PlayerMatchInformation>(ienumerable_1);
            HomeSpectator = new List<PlayerMatchInformation>(ienumerable_2);
            AwaySpectator = new List<PlayerMatchInformation>(ienumerable_3);
            BallPosition = vector3_2;
            BallVelocity = vector3_3;
            MatchState = matchStateType_1;
            FieldPositioning = matchFieldPositioning_1;
            PhaseInfo = gclass166_1;
            ScenarioInfo = gclass170_1;
            Class167 = gclass167_1;
            SomeDouble = double_1;
            TimeMultiplier = float_1;
            SomeStrings1 = new List<string>(ienumerable_4);
            SomeStrings2 = new List<string>(ienumerable_5);
            TeamIdInfo = gclass156_1;
            List163 = new List<GClass163>(ienumerable_6);
            List172 = new List<GClass172>(ienumerable_7);
            SessionRights = userSessionRights_1;
            Class171 = gclass171_1;
            SomeString1 = string_2;
            SomeString2 = string_3;
        }

        public override void Serialize(BinaryWriter gclass316_0)
        {
            base.Serialize(gclass316_0);
            Actor.Serialize(gclass316_0);
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
            gclass316_0.method_9((int)MatchState);
            gclass316_0.method_9((int)FieldPositioning);
            gclass316_0.method_25(PhaseInfo);
            gclass316_0.method_25(ScenarioInfo);
            gclass316_0.method_25(Class167);
            gclass316_0.method_12(TimeMultiplier);
            gclass316_0.method_7(SomeDouble);
            gclass316_0.method_15((ushort)SomeStrings1.Count);
            for (int m = 0; m < SomeStrings1.Count; m++)
            {
                gclass316_0.method_14(SomeStrings1[m]);
            }
            gclass316_0.method_15((ushort)SomeStrings2.Count);
            for (int n = 0; n < SomeStrings2.Count; n++)
            {
                gclass316_0.method_14(SomeStrings2[n]);
            }
            gclass316_0.method_25(TeamIdInfo);
            gclass316_0.method_15((ushort)List163.Count);
            for (int num = 0; num < List163.Count; num++)
            {
                gclass316_0.method_25(List163[num]);
            }
            gclass316_0.method_15((ushort)List172.Count);
            for (int num2 = 0; num2 < List172.Count; num2++)
            {
                gclass316_0.method_25(List172[num2]);
            }
            gclass316_0.method_25(SessionRights);
            gclass316_0.method_25(Class171);
            gclass316_0.method_14(SomeString1);
            gclass316_0.method_14(SomeString2);
        }

        #region Player Helpers
        public List<PlayerMatchInformation> GetPlayers()
        {
            return [.. HomeTeam, .. AwayTeam];
        }

        public List<PlayerMatchInformation> GetSpectators()
        {
            return [.. HomeSpectator, .. AwaySpectator];
        }

        public List<PlayerMatchInformation> GetAllPlayers()
        {
            return [.. HomeTeam, .. AwayTeam, .. HomeSpectator, .. AwaySpectator];
        }

        public PlayerMatchInformation? GetPlayer(int squadNumber)
        {
            foreach (var player in GetAllPlayers())
            {
                if (player.SquadNumber == squadNumber)
                    return player;
            }
            return null;
        }

        public PlayerMatchInformation? GetPlayer(Guid playerId)
        {
            foreach (var player in GetAllPlayers())
            {
                if (player.PlayerId == playerId)
                    return player;
            }

            return null;
        }

        public List<PlayerMatchInformation> GetTeam(StadiumSitting stadiumSitting)
        {
            return stadiumSitting switch
            {
                StadiumSitting.HomePlayer => HomeTeam,
                StadiumSitting.AwayPlayer => AwayTeam,
                StadiumSitting.HomeSpectator => HomeSpectator,
                StadiumSitting.AwaySpectator => AwaySpectator,
                _ => throw new ArgumentException($"Invalid stadium sitting: {stadiumSitting}"),
            };
        }

        public bool HasPlayer(Func<PlayerMatchInformation, bool> predicate)
        {
            foreach (var player in GetAllPlayers())
            {
                if (predicate(player))
                    return true;
            }

            return false;
        }
        #endregion
    }
}
