using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Chat
{
    [MessageAttribute(21144)]
    public class ChatPlayerTextRxMessage : ChatPlayerTextAbstractMessage
    {
        public readonly string MessageText;

        public ChatPlayerTextRxMessage(Guid teamId, string messageText) : base(teamId)
        {
            MessageText = messageText;
        }

        public ChatPlayerTextRxMessage(BinaryReader reader) : base(reader)
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