using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Match
{
    public class MatchEntry : ISerialize
    {
        private int _entryId;

        public MatchEntry(int entryId)
        {
            EntryId = entryId;
        }

        public MatchEntry(BinaryReader reader)
        {
            ArgumentNullException.ThrowIfNull(reader);
            EntryId = reader.method_9();
        }

        public void Serialize(BinaryWriter writer)
        {
            ArgumentNullException.ThrowIfNull(writer);
            writer.method_9(EntryId);
        }

        public int EntryId
        {
            get => _entryId;
            set
            {
                if (!InRange(value))
                    throw new ArgumentOutOfRangeException(nameof(value), "Value must be between 1 and 22.");
                _entryId = value;
            }
        }

        public static int ToSquad(int id, bool splited = false)
        {
            int squad = id - 1;
            return splited ? squad % 11 : squad;
        }

        public int ToSquad(bool splited = false) => ToSquad(EntryId, splited);

        public static bool InRange(int value) => value >= 1 && value <= 22;
    }
}
