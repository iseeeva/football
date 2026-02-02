using System.Numerics;
using Sobee.TestServer.Messages;

namespace Sobee.TestServer.MatchHelpers
{
    public class MatchFormationHelper
    {
        private static readonly int MAX_TEAM_SIZE = ScenarioInfo.MAX_TEAM_SIZE;

        public class Team
        {
            public List<Vector2> Positions { get; }
            public List<Vector2> Directions { get; }
            public List<AnimationType> Animations { get; }

            public Team(List<Vector2> positions, Vector2 defaultDirection, AnimationType defaultAnimation)
            {
                if (positions.Count > MAX_TEAM_SIZE)
                    throw new ArgumentException($"Positions list must have <= {MAX_TEAM_SIZE}");

                Positions = new(positions);
                Directions = Enumerable.Repeat(defaultDirection, MAX_TEAM_SIZE).ToList();
                Animations = Enumerable.Repeat(defaultAnimation, MAX_TEAM_SIZE).ToList();
            }
        }

        public Team Home { get; }
        public Team Away { get; }

        public MatchFormationHelper(
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

        public static MatchFormationHelper GetFormationForScenario(ScenarioType scenarioType) => scenarioType switch
        {
            ScenarioType.ScenarioMatch => Create11v11(scenarioType),
            _ => throw new ArgumentException($"No formation defined for scenario type {scenarioType}.")
        };


        public static float GetStartDirection(ScenarioType scenarioType) => scenarioType switch
        {
            ScenarioType.ScenarioMatch1v1 => 0.33f,
            ScenarioType.ScenarioMatch2v2 or ScenarioType.ScenarioMatch3v3 => 0.66f,
            ScenarioType.ScenarioMatch6v6 or ScenarioType.ScenarioMatch => 1f,
            _ => throw new ArgumentException($"No start direction defined for scenario type {scenarioType}.")
        };

        public static MatchFormationHelper Create11v11(ScenarioType scenarioType)
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

            float dir = GetStartDirection(scenarioType);

            return new MatchFormationHelper(
                home,
                away,
                new Vector2(dir, 0),
                new Vector2(-dir, 0)
            );
        }

        public static MatchFormationHelper SwapFormationSides(MatchFormationHelper src)
        {
            return new MatchFormationHelper(
                src.Away.Positions,
                src.Home.Positions,
                src.Away.Directions[0],
                src.Home.Directions[0],
                src.Away.Animations[0],
                src.Home.Animations[0]
            );
        }
    }
}
