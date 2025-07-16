// Token: 0x02000079 RID: 121
using Sobee.Messaging;

namespace Sobee.Messages.Common.Chat
{
    [GAttribute0(13335)]
    public abstract class Base : Message
    {
        public int squad { get; set; }

        public Base(int squad)
        {
            this.squad = squad;
        }

        public Base(BinaryReader reader) : base(reader)
        {
            squad = reader.method_9();
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_9(squad);
        }
    }
}