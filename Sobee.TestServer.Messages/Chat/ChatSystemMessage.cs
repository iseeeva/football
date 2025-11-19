using Sobee.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Chat
{
    [MessageAttribute(33995)]
    public class ChatSystemMessage : Message
    {
        private string Text { get; }

        private ChatSystemMessageType MessageType { get; }

        public ChatSystemMessage(BinaryReader reader) : base(reader)
        {
            this.Text = reader.method_14();
            this.MessageType = (ChatSystemMessageType)reader.method_9();
        }

        public ChatSystemMessage(string string_1, ChatSystemMessageType systemMessageType_1)
        {
            this.Text = string_1;
            this.MessageType = systemMessageType_1;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_14(this.Text);
            writer.method_9((int)this.MessageType);
        }
    }
}
