using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Match
{
    public class MatchEntry : ISerialize
    {
        private int _entryNumber;

        public MatchEntry(int entryId)
        {
            EntryNumber = entryId;
        }

        public MatchEntry(BinaryReader reader)
        {
            ArgumentNullException.ThrowIfNull(reader);
            EntryNumber = reader.method_9();
        }

        public void Serialize(BinaryWriter writer)
        {
            ArgumentNullException.ThrowIfNull(writer);
            writer.method_9(EntryNumber);
        }

        public int EntryNumber
        {
            get => _entryNumber;
            set
            {
                if (!InRange(value))
                    throw new ArgumentOutOfRangeException(nameof(value), "Value must be between 1 and 22.");

                _entryNumber = value;
            }
        }

        public static int ToSquad(int id, bool splited = false)
        {
            int squad = id - 1;
            return splited ? squad % 11 : squad;
        }

        public int ToSquad(bool splited = false) => ToSquad(EntryNumber, splited);

        public static bool InRange(int value) => value >= 1 && value <= 22;
    }
}
