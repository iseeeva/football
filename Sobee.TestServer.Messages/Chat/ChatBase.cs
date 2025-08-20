using Sobee.Messaging;

namespace Sobee.TestServer.Messages.Chat
{
    [GAttribute0(13335)]
    public abstract class ChatBase : Message
    {
        public int SquadNumber { get; }

        public ChatBase(int squadNumber)
        {
            SquadNumber = squadNumber;
        }

        public ChatBase(BinaryReader reader) : base(reader)
        {
            SquadNumber = reader.method_9();
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_9(SquadNumber);
        }
    }
}