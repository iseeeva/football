using System.Numerics;
using Serilog;
using Sobee.Common;
using Sobee.Serialization.GameServer;
using Sobee.TestServer.Match;
using Sobee.TestServer.Messages;
using Sobee.TestServer.Messages.Player;

namespace Sobee.TestServer
{
    public class MatchFieldPosition
    {
        private static readonly ILogger _log = Logging.Get<MatchFieldPosition>();
        private static readonly int MaxTeamSize = ScenarioInfo.MAX_TEAM_SIZE;

        public class Team
        {
            public Vector2[] Positions { get; }
            public Vector2[] Directions { get; }
            public AnimationType[] Animations { get; }

            public Team(Vector2[] positions, Vector2 defaultDirection, AnimationType defaultAnimation)
            {
                if (positions.Length != MaxTeamSize)
                    throw new ArgumentException($"Positions array must have {MaxTeamSize} elements.");

                Positions = new Vector2[MaxTeamSize];
                Array.Copy(positions, Positions, MaxTeamSize);

                Directions = new Vector2[MaxTeamSize];
                for (int i = 0; i < MaxTeamSize; i++)
                    Directions[i] = defaultDirection;

                Animations = new AnimationType[MaxTeamSize];
                for (int i = 0; i < MaxTeamSize; i++)
                    Animations[i] = defaultAnimation;
            }
        }

        public Team Home { get; }
        public Team Away { get; }

        public MatchFieldPosition(
            IList<Vector2> homePositions,
            IList<Vector2> awayPositions,
            Vector2 homeDirection,
            Vector2 awayDirection,
            AnimationType homeAnimation = AnimationType.WaitIdle1,
            AnimationType awayAnimation = AnimationType.WaitIdle1)
        {
            if (homePositions.Count != MaxTeamSize ||
                awayPositions.Count != MaxTeamSize)
            {
                throw new ArgumentException($"Home/Away position lists must each have {MaxTeamSize} elements.");
            }

            Home = new Team(homePositions.ToArray(), homeDirection, homeAnimation);
            Away = new Team(awayPositions.ToArray(), awayDirection, awayAnimation);
        }

        public static float StartDirection(ScenarioType scenarioType) => scenarioType switch
        {
            ScenarioType.ScenarioMatch1v1 => 0.33f,
            ScenarioType.ScenarioMatch2v2 or ScenarioType.ScenarioMatch3v3 => 0.66f,
            ScenarioType.ScenarioMatch6v6 or ScenarioType.ScenarioMatch => 1f,
            _ => 0f
        };

        public static void ChangePosition(MatchRoom room, MatchFieldPositioning fieldType)
        {
            if (room is null) throw new ArgumentNullException(nameof(room));

            var positioning = CreatePosition(room.MatchInformation.ScenarioInfo.ScenarioType, fieldType);

            if (positioning is null)
            {
                _log.Error("Room [{RoomId}]: Positioning {FieldType} not found.", room.Id, fieldType);
                return;
            }

            room.MatchInformation.MatchState = Messages.Match.MatchStateType.Positioning;
            room.MatchInformation.FieldPositioning = fieldType;

            ApplyTeamPositions(room.MatchInformation.HomeTeam, positioning.Home);
            ApplyTeamPositions(room.MatchInformation.AwayTeam, positioning.Away);

            room.Players.SendMessage(
                new PositioningCutscene(
                    fieldType,
                    positioning.Home.Positions,
                    positioning.Away.Positions,
                    positioning.Home.Directions,
                    positioning.Away.Directions,
                    positioning.Home.Animations,
                    positioning.Away.Animations
                )
            );

            _log.Debug($"Room {room.Id} positioned for {fieldType}.");
        }

        private static void ApplyTeamPositions(List<PlayerMatchInformation> team, Team teamPositions)
        {
            for (int i = 0; i < team.Count; i++)
            {
                team[i].Position = teamPositions.Positions[i];
                team[i].Direction = teamPositions.Directions[i];
            }
        }

        public static MatchFieldPosition? CreatePosition(ScenarioType scenarioType, MatchFieldPositioning fieldType) =>
            scenarioType switch
            {
                ScenarioType.ScenarioMatch => fieldType switch
                {
                    MatchFieldPositioning.Kickoff => Create11v11(scenarioType),
                    _ => throw new ArgumentException($"Invalid field type {fieldType} for scenario {scenarioType}.")
                },
                ScenarioType.ScenarioMatch1v1 => fieldType switch
                {
                    MatchFieldPositioning.Kickoff => Create11v11(scenarioType),
                    _ => throw new ArgumentException($"Invalid field type {fieldType} for scenario {scenarioType}.")
                },
                _ => throw new ArgumentException($"Invalid scenario type {scenarioType}.")
            };

        public static MatchFieldPosition Create11v11(ScenarioType scenarioType)
        {
            var homePositions = new[]
            {
                new Vector2(-4160, 20),
                new Vector2(-3140, -1900),
                new Vector2(-3140, 1860),
                new Vector2(-3300, -20),
                new Vector2(-1960, 960),
                new Vector2(-1940, -720),
                new Vector2(-1140, -2360),
                new Vector2(-1040, 80),
                new Vector2(-140, -200),
                new Vector2(-480, 1780),
                new Vector2(0, 0)
            };

            var awayPositions = new[]
            {
                new Vector2(4200, 20),
                new Vector2(3140, 1900),
                new Vector2(3140, -1880),
                new Vector2(3380, 20),
                new Vector2(2320, -840),
                new Vector2(2320, 780),
                new Vector2(1260, 2320),
                new Vector2(1060, 80),
                new Vector2(220, 980),
                new Vector2(260, -980),
                new Vector2(1180, -2340)
            };

            float dir = StartDirection(scenarioType);

            return new MatchFieldPosition(
                homePositions,
                awayPositions,
                new Vector2(dir, 0),
                new Vector2(-dir, 0)
            );
        }
    }
}
