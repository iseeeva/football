// Token: 0x02000083 RID: 131
namespace Sobee.TestServer.Messages.Chat
{
    [GAttribute0(21144)]
    public class ChatMessage : ChatBase
    {
        public string MessageText { get; }

        public ChatMessage(Guid teamId, string text) : base(teamId)
        {
            MessageText = text;
        }

        public ChatMessage(BinaryReader reader) : base(reader)
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