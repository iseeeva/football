using System.Numerics;
using Serilog;
using Sobee.Common;
using Sobee.Serialization.GameServer;
using Sobee.TestServer.Match;
using Sobee.TestServer.Messages;
using Sobee.TestServer.Messages.Match;
using Sobee.TestServer.Messages.Player;

namespace Sobee.TestServer.Helpers
{
    public class PositioningHelper
    {
        private static readonly ILogger _log = Logging.Get<PositioningHelper>();
        private static readonly int MAX_TEAM_SIZE = ScenarioInfo.MAX_TEAM_SIZE;

        public class Team
        {
            public List<Vector2> Positions { get; }
            public List<Vector2> Directions { get; }
            public List<AnimationType> Animations { get; }

            public Team(List<Vector2> positions, Vector2 defaultDirection, AnimationType defaultAnimation)
            {
                if (positions.Count != MAX_TEAM_SIZE)
                    throw new ArgumentException($"Positions list must have {MAX_TEAM_SIZE} elements.");

                Positions = new(positions);
                Directions = Enumerable.Repeat(defaultDirection, MAX_TEAM_SIZE).ToList();
                Animations = Enumerable.Repeat(defaultAnimation, MAX_TEAM_SIZE).ToList();
            }
        }

        public Team Home { get; }
        public Team Away { get; }

        public PositioningHelper(
            List<Vector2> homePositions,
            List<Vector2> awayPositions,
            Vector2 homeDirection,
            Vector2 awayDirection,
            AnimationType homeAnimation = AnimationType.WaitIdle1,
            AnimationType awayAnimation = AnimationType.WaitIdle1)
        {
            Home = new Team(homePositions, homeDirection, homeAnimation);
            Away = new Team(awayPositions, awayDirection, awayAnimation);
        }

        public static float StartDirection(ScenarioType scenarioType) => scenarioType switch
        {
            ScenarioType.ScenarioMatch1v1 => 0.33f,
            ScenarioType.ScenarioMatch2v2 or ScenarioType.ScenarioMatch3v3 => 0.66f,
            ScenarioType.ScenarioMatch6v6 or ScenarioType.ScenarioMatch => 1f,
            _ => 0f
        };

        public static void ChangePosition(MatchRoom matchRoom, MatchFieldPositioning fieldType)
        {
            ArgumentNullException.ThrowIfNull(matchRoom);

            var pos = CreatePosition(matchRoom, fieldType);
            if (pos == null)
            {
                _log.Error("Room [{RoomId}]: Positioning {FieldType} not found.", matchRoom.Id, fieldType);
                return;
            }

            _log.Debug("Room {RoomId} positioned for {FieldType}.", matchRoom.Id, fieldType);
        }

        private static void ApplyPositions(List<PlayerMatchInformation> players, Team team)
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].Position = team.Positions[i];
                players[i].Direction = team.Directions[i];
            }
        }

        private static void ZeroUnused(List<Vector2> pos, List<Vector2> dir, int count)
        {
            for (int i = count; i < MAX_TEAM_SIZE; i++)
            {
                pos[i] = Vector2.Zero;
                dir[i] = Vector2.Zero;
            }
        }

        public static PositioningHelper? CreatePosition(MatchRoom room, MatchFieldPositioning fieldType)
        {
            var scenario = room.MatchInformation.ScenarioInfo.ScenarioType;

            return scenario switch
            {
                ScenarioType.ScenarioMatch => fieldType switch
                {
                    MatchFieldPositioning.Kickoff => CreateKickoff(room, fieldType),
                    _ => throw new ArgumentException($"Invalid field type {fieldType} for scenario {scenario}.")
                },
                _ => throw new ArgumentException($"Invalid scenario type {scenario}.")
            };
        }

        public static PositioningHelper CreateKickoff(MatchRoom matchRoom, MatchFieldPositioning type)
        {
            var matchInfo = matchRoom.MatchInformation;
            var matchPhaseInfo = matchInfo.PhaseInfo;

            var pos = Create11v11(matchInfo.ScenarioInfo.ScenarioType);
            var startingTeam = matchPhaseInfo.IsFirstHalf() ? matchInfo.HomeTeam : matchInfo.AwayTeam;

            var actionerIndex = startingTeam.Count - 1;
            var actionerPlayerInformation = startingTeam[actionerIndex];

            if (matchPhaseInfo.IsFirstHalf())
                pos.Home.Positions[actionerIndex] = Vector2.Zero;
            else
            {
                pos.Away.Positions[actionerIndex] = Vector2.Zero;
                pos = new PositioningHelper(
                    pos.Away.Positions,
                    pos.Home.Positions,
                    pos.Away.Directions[0],
                    pos.Home.Directions[0],
                    pos.Away.Animations[0],
                    pos.Home.Animations[0]
                );
            }

            ZeroUnused(pos.Home.Positions, pos.Home.Directions, matchInfo.HomeTeam.Count);
            ZeroUnused(pos.Away.Positions, pos.Away.Directions, matchInfo.AwayTeam.Count);

            matchInfo.MatchState = MatchStateType.Positioning;
            matchInfo.FieldPositioning = type;

            ApplyPositions(matchInfo.HomeTeam, pos.Home);
            ApplyPositions(matchInfo.AwayTeam, pos.Away);

            matchRoom.Players.SendMessage(new PositioningCutscene(
                type,
                pos.Home.Positions,
                pos.Away.Positions,
                pos.Home.Directions,
                pos.Away.Directions,
                pos.Home.Animations,
                pos.Away.Animations
            ));

            BallHelper.GetBall(matchRoom, actionerPlayerInformation.SquadNumber, 0f);
            return pos;
        }

        public static PositioningHelper Create11v11(ScenarioType scenarioType)
        {
            var home = new List<Vector2>
            {
                new(-4160, 20), new(-3140, -1900), new(-3140, 1860), new(-3300, -20),
                new(-1960, 960), new(-1940, -720), new(-1140, -2360), new(-1040, 80),
                new(-140, -200), new(-480, 1780), new(-180, 260)
            };

            var away = new List<Vector2>
            {
                new(4200, 20), new(3140, 1900), new(3140, -1880), new(3380, 20),
                new(2320, -840), new(2320, 780), new(1260, 2320), new(1060, 80),
                new(220, 980), new(260, -980), new(1180, -2340)
            };

            float dir = StartDirection(scenarioType);

            return new PositioningHelper(
                home,
                away,
                new Vector2(dir, 0),
                new Vector2(-dir, 0)
            );
        }
    }
}
