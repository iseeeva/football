// Token: 0x02000083 RID: 131
namespace Sobee.Messages.Common.Chat
{
    [GAttribute0(21144)]
    public class Messaging : Base
    {
        public string text { get; set; }

        public Messaging(int squad, string text) : base(squad)
        {
            this.text = text;
        }

        public Messaging(BinaryReader gclass315_0) : base(gclass315_0)
        {
            text = gclass315_0.method_14();
        }

        public override void Deserialize(BinaryWriter gclass316_0)
        {
            base.Deserialize(gclass316_0);
            gclass316_0.method_14(text);
        }

        public override string ToString()
        {
            return $"{squad} - {text}";
        }
    }
}