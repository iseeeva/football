// Token: 0x02000083 RID: 131
namespace Sobee.TestServer.Messages.Chat
{
    [GAttribute0(21144)]
    public class ChatMessage : ChatBase
    {
        public string Text { get; }

        public ChatMessage(int squad, string text) : base(squad)
        {
            Text = text;
        }

        public ChatMessage(BinaryReader reader) : base(reader)
        {
            Text = reader.method_14();
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_14(Text);
        }

        public override string ToString()
        {
            return $"{SquadNumber} - {Text}";
        }
    }
}