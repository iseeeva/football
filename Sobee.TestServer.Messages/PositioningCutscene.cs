using System.Numerics;
using Sobee.Messaging;
using Sobee.Serialization.GameServer;

namespace Sobee.TestServer.Messages
{
    [GAttribute0(30461)]
    public class PositioningCutscene : Message
    {
        private static readonly int MAX_TEAM_SIZE = ScenarioInfo.MAX_TEAM_SIZE;

        public MatchFieldPositioning PositioningType { get; private set; }

        public Vector2[] HomePositions { get; } = new Vector2[MAX_TEAM_SIZE];
        public Vector2[] AwayPositions { get; } = new Vector2[MAX_TEAM_SIZE];

        public float[] HomeDirections { get; } = new float[MAX_TEAM_SIZE];
        public float[] AwayDirections { get; } = new float[MAX_TEAM_SIZE];

        public AnimationType[] HomeAnimations { get; } = new AnimationType[MAX_TEAM_SIZE];
        public AnimationType[] AwayAnimations { get; } = new AnimationType[MAX_TEAM_SIZE];

        // ---------------------------------------------------------
        // BINARY READER CONSTRUCTOR
        // ---------------------------------------------------------
        public PositioningCutscene(BinaryReader reader) : base(reader)
        {
            PositioningType = (MatchFieldPositioning)reader.method_9();

            for (int i = 0; i < MAX_TEAM_SIZE; i++)
                HomePositions[i] = reader.method_19();

            for (int i = 0; i < MAX_TEAM_SIZE; i++)
                AwayPositions[i] = reader.method_19();

            for (int i = 0; i < MAX_TEAM_SIZE; i++)
                HomeDirections[i] = reader.method_12();

            for (int i = 0; i < MAX_TEAM_SIZE; i++)
                AwayDirections[i] = reader.method_12();

            for (int i = 0; i < MAX_TEAM_SIZE; i++)
                HomeAnimations[i] = (AnimationType)reader.method_15();

            for (int i = 0; i < MAX_TEAM_SIZE; i++)
                AwayAnimations[i] = (AnimationType)reader.method_15();
        }

        // ---------------------------------------------------------
        // CUSTOM CONSTRUCTOR (ARRAYS ONLY)
        // ---------------------------------------------------------
        public PositioningCutscene(
            MatchFieldPositioning type,
            Vector2[] homePositions,
            Vector2[] awayPositions,
            Vector2[] homeDirectionVectors,
            Vector2[] awayDirectionVectors,
            AnimationType[] homeAnimations,
            AnimationType[] awayAnimations)
        {
            if (homePositions.Length != MAX_TEAM_SIZE ||
                awayPositions.Length != MAX_TEAM_SIZE ||
                homeDirectionVectors.Length != MAX_TEAM_SIZE ||
                awayDirectionVectors.Length != MAX_TEAM_SIZE ||
                homeAnimations.Length != MAX_TEAM_SIZE ||
                awayAnimations.Length != MAX_TEAM_SIZE)
            {
                throw new ArgumentException($"All arrays must have {MAX_TEAM_SIZE} elements.");
            }

            PositioningType = type;

            Array.Copy(homePositions, HomePositions, MAX_TEAM_SIZE);
            Array.Copy(awayPositions, AwayPositions, MAX_TEAM_SIZE);
            Array.Copy(homeAnimations, HomeAnimations, MAX_TEAM_SIZE);
            Array.Copy(awayAnimations, AwayAnimations, MAX_TEAM_SIZE);

            for (int i = 0; i < MAX_TEAM_SIZE; i++)
                HomeDirections[i] = GClass97.smethod_15(homeDirectionVectors[i].Y, homeDirectionVectors[i].X);

            for (int i = 0; i < MAX_TEAM_SIZE; i++)
                AwayDirections[i] = GClass97.smethod_15(awayDirectionVectors[i].Y, awayDirectionVectors[i].X);
        }

        // ---------------------------------------------------------
        // SERIALIZATION
        // ---------------------------------------------------------
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

        // ---------------------------------------------------------
        // GET DIRECTION VECTORS
        // ---------------------------------------------------------
        public Vector2[] GetHomeDirectionVectors()
        {
            var result = new Vector2[MAX_TEAM_SIZE];
            for (int i = 0; i < MAX_TEAM_SIZE; i++)
                result[i] = new Vector2(GClass97.smethod_11(HomeDirections[i]),
                                        GClass97.smethod_10(HomeDirections[i]));
            return result;
        }

        public Vector2[] GetAwayDirectionVectors()
        {
            var result = new Vector2[MAX_TEAM_SIZE];
            for (int i = 0; i < MAX_TEAM_SIZE; i++)
                result[i] = new Vector2(GClass97.smethod_11(AwayDirections[i]),
                                        GClass97.smethod_10(AwayDirections[i]));
            return result;
        }

        public override string ToString() => $"Positioned - {PositioningType}";
    }
}
