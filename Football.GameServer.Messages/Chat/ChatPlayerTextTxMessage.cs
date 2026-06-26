using Football.Serialization;

namespace Football.GameServer.Messages.Chat
{
    [Message(9210)]
    public class ChatPlayerTextTxMessage : ChatPlayerTextAbstractMessage
    {
        public readonly Guid PlayerId;
        public readonly string MessageText;
        /// <summary> pre-defined color numbers by client (not hex or sum) </summary>
        public readonly byte MessageColorNumber;

        public ChatPlayerTextTxMessage(Guid teamId, Guid playerId, string messageText, byte messageColorNumber) : base(teamId)
        {
            PlayerId = playerId;
            MessageText = messageText;
            MessageColorNumber = messageColorNumber;
        }

        public ChatPlayerTextTxMessage(BinaryReader reader) : base(reader)
        {
            PlayerId = GuidConverter.ConvertFromInt(reader.method_9());
            MessageText = reader.method_14();
            MessageColorNumber = reader.method_2();
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_9(GuidConverter.ConvertToInt(PlayerId));
            writer.method_14(MessageText);
            writer.method_2(MessageColorNumber);
        }
    }
}
