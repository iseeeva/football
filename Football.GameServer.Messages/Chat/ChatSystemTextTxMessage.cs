using Football.Network.Messaging;
using Football.Serialization;

namespace Football.GameServer.Messages.Chat
{
    [Message(33995)]
    public class ChatSystemTextTxMessage : Message
    {
        public readonly string MessageText;
        public readonly ChatSystemMessageType MessageType;

        public ChatSystemTextTxMessage(BinaryReader reader) : base(reader)
        {
            MessageText = reader.method_14();
            MessageType = (ChatSystemMessageType)reader.method_9();
        }

        public ChatSystemTextTxMessage(string messageText, ChatSystemMessageType messageType)
        {
            MessageText = messageText;
            MessageType = messageType;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_14(MessageText);
            writer.method_9((int)MessageType);
        }
    }
}
