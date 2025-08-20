using System.Numerics;
using Sobee.Messaging;
using Sobee.Serialization.GameServer;

namespace Sobee.TestServer.Messages
{
    [GAttribute0(30461)]
    public class PositioningCutscene : Message
    {

        public MatchFieldPositioning PositioningType { get; private set; }

        public List<Vector2> HomePositions { get; } = new List<Vector2>(ScenarioInfo.MAX_TEAM_SIZE);
        public List<Vector2> AwayPositions { get; } = new List<Vector2>(ScenarioInfo.MAX_TEAM_SIZE);

        public List<float> HomeDirections { get; } = new List<float>(ScenarioInfo.MAX_TEAM_SIZE);
        public List<float> AwayDirections { get; } = new List<float>(ScenarioInfo.MAX_TEAM_SIZE);

        public List<AnimationType> HomeAnimations { get; } = new List<AnimationType>(ScenarioInfo.MAX_TEAM_SIZE);
        public List<AnimationType> AwayAnimations { get; } = new List<AnimationType>(ScenarioInfo.MAX_TEAM_SIZE);

        public PositioningCutscene(BinaryReader reader) : base(reader)
        {
            PositioningType = (MatchFieldPositioning)reader.method_9();

            for (int i = 0; i < HomePositions.Capacity; i++)
                HomePositions.Add(reader.method_19());

            for (int i = 0; i < AwayPositions.Capacity; i++)
                AwayPositions.Add(reader.method_19());

            for (int i = 0; i < HomeDirections.Capacity; i++)
                HomeDirections.Add(reader.method_12());

            for (int i = 0; i < AwayDirections.Capacity; i++)
                AwayDirections.Add(reader.method_12());

            for (int i = 0; i < HomeAnimations.Capacity; i++)
                HomeAnimations.Add((AnimationType)reader.method_15());

            for (int i = 0; i < AwayAnimations.Capacity; i++)
                AwayAnimations.Add((AnimationType)reader.method_15());
        }

        public PositioningCutscene(
            MatchFieldPositioning type,
            IEnumerable<Vector2> homePositions,
            IEnumerable<Vector2> awayPositions,
            IEnumerable<Vector2> homeDirectionVectors,
            IEnumerable<Vector2> awayDirectionVectors,
            IEnumerable<AnimationType> homeAnimations,
            IEnumerable<AnimationType> awayAnimations)
        {
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

        public List<Vector2> GetHomeDirections()
        {
            var result = new List<Vector2>(HomeDirections.Count);
            foreach (var angle in HomeDirections)
                result.Add(new Vector2(GClass97.smethod_11(angle), GClass97.smethod_10(angle)));
            return result;
        }

        public List<Vector2> GetAwayDirections()
        {
            var result = new List<Vector2>(AwayDirections.Count);
            foreach (var angle in AwayDirections)
                result.Add(new Vector2(GClass97.smethod_11(angle), GClass97.smethod_10(angle)));
            return result;
        }

        public override string ToString() => $"Positioned - {PositioningType}";
    }
}
