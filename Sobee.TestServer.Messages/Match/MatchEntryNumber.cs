using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Match
{
    public class MatchEntryNumber : ISerialize
    {
        private int _value;
        public int Value
        {
            get => _value;
            set
            {
                if (IsValidEntryNumber(value))
                    _value = value;
                else
                    throw new ArgumentOutOfRangeException(nameof(value), "Entry number not in valid range.");
            }
        }

        public static implicit operator int(MatchEntryNumber entryNumber)
            => entryNumber.Value;

        public MatchEntryNumber(int entryNumber)
        {
            Value = entryNumber;
        }

        public MatchEntryNumber(BinaryReader reader)
        {
            ArgumentNullException.ThrowIfNull(reader);
            Value = reader.method_9();
        }

        public static bool IsValidEntryNumber(int entryNumber)
        {
            return
                Math.Abs(entryNumber) % 1 == 0 &&
                entryNumber >= 1 &&
                entryNumber <= ScenarioInfo.MAX_TEAM_SIZE * 2;
        }

        public void Serialize(BinaryWriter writer)
        {
            ArgumentNullException.ThrowIfNull(writer);
            writer.method_9(Value);
        }
    }
}
