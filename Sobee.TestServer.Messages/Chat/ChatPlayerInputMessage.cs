using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Chat
{
    [MessageAttribute(21144)]
    public class ChatPlayerInputMessage : ChatBaseMessage
    {
        public string MessageText { get; }

        public ChatPlayerInputMessage(Guid teamId, string text) : base(teamId)
        {
            MessageText = text;
        }

        public ChatPlayerInputMessage(BinaryReader reader) : base(reader)
        {
            MessageText = reader.method_14();
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_14(MessageText);
        }

        public override string ToString()
        {
            return $"{TeamId} - {MessageText}";
        }
    }
}