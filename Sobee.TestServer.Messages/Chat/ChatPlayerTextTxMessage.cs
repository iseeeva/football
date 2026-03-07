using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Chat
{
    [MessageAttribute(9210)]
    public class ChatPlayerTextTxMessage : ChatPlayerTextAbstractMessage
    {
        public readonly Guid PlayerId;
        public readonly string MessageText;
        /// <summary> pre-defined color numbers by client (not hex or sum) </summary>
        public readonly byte MessageColorNumber;

        public ChatPlayerTextTxMessage(Guid teamId, Guid playerId, string messageText, byte messageColorNumber) : base(teamId)
        {
            this.PlayerId = playerId;
            this.MessageText = messageText;
            this.MessageColorNumber = messageColorNumber;
        }

        public ChatPlayerTextTxMessage(BinaryReader reader) : base(reader)
        {
            this.PlayerId = GuidConverter.ConvertFromInt(reader.method_9());
            this.MessageText = reader.method_14();
            this.MessageColorNumber = reader.method_2();
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_9(GuidConverter.ConvertToInt(this.PlayerId));
            writer.method_14(this.MessageText);
            writer.method_2(this.MessageColorNumber);
        }
    }
}
