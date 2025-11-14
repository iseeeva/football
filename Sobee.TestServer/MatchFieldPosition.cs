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

                Positions = new List<Vector2>(positions);
                Directions = Enumerable.Repeat(defaultDirection, MAX_TEAM_SIZE).ToList();
                Animations = Enumerable.Repeat(defaultAnimation, MAX_TEAM_SIZE).ToList();
            }
        }

        public Team Home { get; }
        public Team Away { get; }

        public MatchFieldPosition(
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

        public static void ChangePosition(MatchRoom room, MatchFieldPositioning fieldType)
        {
            ArgumentNullException.ThrowIfNull(room);

            var positioning = CreatePosition(room.MatchInformation.ScenarioInfo.ScenarioType, fieldType);
            if (positioning is null)
            {
                _log.Error("Room [{RoomId}]: Positioning {FieldType} not found.", room.Id, fieldType);
                return;
            }

            void ZeroUnused(List<Vector2> positions, List<Vector2> directions, int count)
            {
                for (int i = count; i < MAX_TEAM_SIZE; i++)
                {
                    positions[i] = Vector2.Zero;
                    directions[i] = Vector2.Zero;
                }
            }

            // Client 11 oyuncu bekliyor ama daha az oyuncu olabilir, bunun icin kalan pozisyonlari sifirladim.
            ZeroUnused(positioning.Home.Positions, positioning.Home.Directions, room.MatchInformation.HomeTeam.Count);
            ZeroUnused(positioning.Away.Positions, positioning.Away.Directions, room.MatchInformation.AwayTeam.Count);

            room.MatchInformation.MatchState = Messages.Match.MatchStateType.Positioning;
            room.MatchInformation.FieldPositioning = fieldType;

            void ApplyPositions(List<PlayerMatchInformation> team, Team positions)
            {
                for (int i = 0; i < team.Count; i++)
                {
                    team[i].Position = positions.Positions[i];
                    team[i].Direction = positions.Directions[i];
                }
            }

            // Pozisyonlari oyunculara uygula (server-side icin)
            ApplyPositions(room.MatchInformation.HomeTeam, positioning.Home);
            ApplyPositions(room.MatchInformation.AwayTeam, positioning.Away);

            room.Players.SendMessage(new PositioningCutscene(
                fieldType,
                positioning.Home.Positions,
                positioning.Away.Positions,
                positioning.Home.Directions,
                positioning.Away.Directions,
                positioning.Home.Animations,
                positioning.Away.Animations
            ));

            _log.Debug($"Room {room.Id} positioned for {fieldType}.");
        }

        public static MatchFieldPosition? CreatePosition(ScenarioType scenarioType, MatchFieldPositioning fieldType) =>
            scenarioType switch
            {
                ScenarioType.ScenarioMatch => fieldType switch
                {
                    MatchFieldPositioning.Kickoff => Create11v11(scenarioType),
                    _ => throw new ArgumentException($"Invalid field type {fieldType} for scenario {scenarioType}.")
                },
                _ => throw new ArgumentException($"Invalid scenario type {scenarioType}.")
            };

        public static MatchFieldPosition Create11v11(ScenarioType scenarioType)
        {
            var homePositions = new List<Vector2>
            {
                new(-4160, 20), new(-3140, -1900), new(-3140, 1860), new(-3300, -20),
                new(-1960, 960), new(-1940, -720), new(-1140, -2360), new(-1040, 80),
                new(-140, -200), new(-480, 1780), new Vector2(0, 0)
            };

            var awayPositions = new List<Vector2>
            {
                new(4200, 20), new(3140, 1900), new(3140, -1880), new(3380, 20),
                new(2320, -840), new(2320, 780), new(1260, 2320), new(1060, 80),
                new(220, 980), new(260, -980), new(1180, -2340)
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
