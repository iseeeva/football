using System.Numerics;
using Serilog;
using Sobee.Common;
using Sobee.Serialization.GameServer;
using Sobee.TestServer.Messages;

namespace Sobee.TestServer.Common
{
    public class MatchFieldPosition
    {
        private static readonly ILogger _log = Logging.Get<MatchFieldPosition>();

        public class Team
        {
            public List<Vector2> Positions { get; }
            public List<Vector2> Directions { get; }
            public List<AnimationType> Animations { get; }

            public Team(List<Vector2> positions, Vector2 defaultDirection, AnimationType defaultAnimation)
            {
                ArgumentNullException.ThrowIfNull(positions);
                var positionList = positions.ToList();
                Positions = positionList;
                Directions = positionList.Select(_ => defaultDirection).ToList();
                Animations = positionList.Select(_ => defaultAnimation).ToList();
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

        public static void ChangePosition(Room room, MatchFieldPositioning fieldType)
        {
            ArgumentNullException.ThrowIfNull(room);

            var positioning = CreatePosition(room.Information.ScenarioInfo.ScenarioType, fieldType);

            if (positioning is null)
            {
                _log.Error("Room [{RoomId}]: Positioning {FieldType} not found.", room.Id, fieldType);
                return;
            }

            room.Information.MatchState = Messages.Match.MatchStateType.Positioning;
            room.Information.FieldPositioning = fieldType;

            ApplyTeamPositions(room.Information.HomeTeam, positioning.Home);
            ApplyTeamPositions(room.Information.AwayTeam, positioning.Away);

            room.Clients.Broadcast(
                new Messages.PositioningCutscene(
                    fieldType,
                    positioning.Home.Positions, positioning.Away.Positions,
                    positioning.Home.Directions, positioning.Away.Directions,
                    positioning.Home.Animations, positioning.Away.Animations
                )
            );
            _log.Debug($"Room {room.Id} positioned for {fieldType}.");
        }

        private static void ApplyTeamPositions(List<Messages.Player.PlayerMatchInformation> team, Team teamPositions)
        {
            foreach (var player in team)
            {
                player.Position = teamPositions.Positions[player.SquadNumber];
                player.Direction = teamPositions.Directions[player.SquadNumber];
            }
        }

        #region Positions 
        public static MatchFieldPosition? CreatePosition(ScenarioType scenarioType, MatchFieldPositioning fieldType) => scenarioType switch
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
                new(-4160, 20),    // Goalkeeper
                new(-3140, -1900), // Left Full-back
                new(-3140, 1860),  // Right Full-back
                new(-3300, -20),   // Left Center-back
                new(-1960, 960),   // Right Center-back
                new(-1940, -720),  // Left Midfielder
                new(-1140, -2360), // Right Midfielder
                new(-1040, 80),    // Center Midfielder 1
                new(-140, -200),   // Center Midfielder 2
                new(-480, 1780),   // Left Forward
                new(-180, 260)     // Right Forward
            };

            var awayPositions = new List<Vector2>
            {
                new(4200, 20),    // Goalkeeper
                new(3140, 1900),  // Left Full-back
                new(3140, -1880), // Right Full-back
                new(3380, 20),    // Left Center-back
                new(2320, -840),  // Right Center-back
                new(2320, 780),   // Left Midfielder
                new(1260, 2320),  // Right Midfielder
                new(1060, 80),    // Center Midfielder 1
                new(220, 980),    // Center Midfielder 2
                new(260, -980),   // Left Forward
                new(1180, -2340)  // Right Forward
            };

            var direction = StartDirection(scenarioType);

            return new MatchFieldPosition(
                homePositions,
                awayPositions,
                new Vector2(direction, 0),
                new Vector2(-direction, 0)
            );
        }
        #endregion
    }
}
