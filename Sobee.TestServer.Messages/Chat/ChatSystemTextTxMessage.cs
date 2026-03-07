using Sobee.Network.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Chat
{
    [MessageAttribute(33995)]
    public class ChatSystemTextTxMessage : Message
    {
        public readonly string MessageText;
        public readonly ChatSystemMessageType MessageType;

        public ChatSystemTextTxMessage(BinaryReader reader) : base(reader)
        {
            this.MessageText = reader.method_14();
            this.MessageType = (ChatSystemMessageType)reader.method_9();
        }

        public ChatSystemTextTxMessage(string messageText, ChatSystemMessageType messageType)
        {
            this.MessageText = messageText;
            this.MessageType = messageType;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_14(this.MessageText);
            writer.method_9((int)this.MessageType);
        }
    }
}
