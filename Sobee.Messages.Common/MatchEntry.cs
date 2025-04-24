namespace Sobee.Messages.Common
{
    public class MatchEntry : IDeserialize
    {
        private int EntryId;

        public MatchEntry(int value)
        {
            this.EntryId = value;
        }

        public MatchEntry(BinaryReader reader)
        {
            this.EntryId = reader.method_9();
        }

        public int GetEntryId()
        {
            return this.EntryId;
        }

        public int ValueProperty
        {
            get => EntryId;
            set
            {
                if (value < 1 || value > 22)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Value must be between 1 and 22.");
                }
                EntryId = value;
            }
        }
        public static int ToSquad(int id, bool splited = false)
        {
            int squad = id - 1;
            return splited ? (squad % 11) : squad;
        }

        public int ToSquad(bool splited = false)
        {
            int squad = EntryId - 1;
            return splited ? (squad % 11) : squad;
        }

        public static bool InRange(int value)
        {
            return value >= 1 && value <= 22;
        }

        public void Deserialize(BinaryWriter writer)
        {
            writer.method_9(EntryId);
        }
    }
}
