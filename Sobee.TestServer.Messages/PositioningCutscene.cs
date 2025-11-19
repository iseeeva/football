using System.Numerics;
using Sobee.Messaging;
using Sobee.Serialization;
using Sobee.Serialization.GameServer;

namespace Sobee.TestServer.Messages
{
    [MessageAttribute(30461)]
    public class PositioningCutscene : Message
    {
        private static readonly int MAX_TEAM_SIZE = ScenarioInfo.MAX_TEAM_SIZE;

        public MatchFieldPositioning PositioningType { get; private set; }

        public List<Vector2> HomePositions { get; } = new List<Vector2>(MAX_TEAM_SIZE);
        public List<Vector2> AwayPositions { get; } = new List<Vector2>(MAX_TEAM_SIZE);

        public List<float> HomeDirections { get; } = new List<float>(MAX_TEAM_SIZE);
        public List<float> AwayDirections { get; } = new List<float>(MAX_TEAM_SIZE);

        public List<AnimationType> HomeAnimations { get; } = new List<AnimationType>(MAX_TEAM_SIZE);
        public List<AnimationType> AwayAnimations { get; } = new List<AnimationType>(MAX_TEAM_SIZE);

        public PositioningCutscene(BinaryReader reader) : base(reader)
        {
            PositioningType = (MatchFieldPositioning)reader.method_9();

            for (int i = 0; i < MAX_TEAM_SIZE; i++)
                HomePositions.Add(reader.method_19());

            for (int i = 0; i < MAX_TEAM_SIZE; i++)
                AwayPositions.Add(reader.method_19());

            for (int i = 0; i < MAX_TEAM_SIZE; i++)
                HomeDirections.Add(reader.method_12());

            for (int i = 0; i < MAX_TEAM_SIZE; i++)
                AwayDirections.Add(reader.method_12());

            for (int i = 0; i < MAX_TEAM_SIZE; i++)
                HomeAnimations.Add((AnimationType)reader.method_15());

            for (int i = 0; i < MAX_TEAM_SIZE; i++)
                AwayAnimations.Add((AnimationType)reader.method_15());
        }

        public PositioningCutscene(
            MatchFieldPositioning type,
            List<Vector2> homePositions,
            List<Vector2> awayPositions,
            List<Vector2> homeDirectionVectors,
            List<Vector2> awayDirectionVectors,
            List<AnimationType> homeAnimations,
            List<AnimationType> awayAnimations)
        {
            if (homePositions.Count != MAX_TEAM_SIZE ||
                awayPositions.Count != MAX_TEAM_SIZE ||
                homeDirectionVectors.Count != MAX_TEAM_SIZE ||
                awayDirectionVectors.Count != MAX_TEAM_SIZE ||
                homeAnimations.Count != MAX_TEAM_SIZE ||
                awayAnimations.Count != MAX_TEAM_SIZE)
            {
                throw new ArgumentException($"All lists must have {MAX_TEAM_SIZE} elements.");
            }

            PositioningType = type;

            HomePositions.AddRange(homePositions);
            AwayPositions.AddRange(awayPositions);
            HomeAnimations.AddRange(homeAnimations);
            AwayAnimations.AddRange(awayAnimations);

            foreach (var vec in homeDirectionVectors)
                HomeDirections.Add(GClass97.smethod_15(vec.Y, vec.X));

            foreach (var vec in awayDirectionVectors)
                AwayDirections.Add(GClass97.smethod_15(vec.Y, vec.X));
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);

            writer.method_9((int)PositioningType);

            foreach (var pos in HomePositions)
                writer.method_19(pos);

            foreach (var pos in AwayPositions)
                writer.method_19(pos);

            foreach (var dir in HomeDirections)
                writer.method_12(dir);

            foreach (var dir in AwayDirections)
                writer.method_12(dir);

            foreach (var anim in HomeAnimations)
                writer.method_15((ushort)anim);

            foreach (var anim in AwayAnimations)
                writer.method_15((ushort)anim);
        }

        public List<Vector2> GetHomeDirectionVectors()
        {
            var result = new List<Vector2>(MAX_TEAM_SIZE);
            foreach (var angle in HomeDirections)
                result.Add(new Vector2(GClass97.smethod_11(angle), GClass97.smethod_10(angle)));
            return result;
        }

        public List<Vector2> GetAwayDirectionVectors()
        {
            var result = new List<Vector2>(MAX_TEAM_SIZE);
            foreach (var angle in AwayDirections)
                result.Add(new Vector2(GClass97.smethod_11(angle), GClass97.smethod_10(angle)));
            return result;
        }

        public override string ToString() => $"Positioned - {PositioningType}";
    }
}
