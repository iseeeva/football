using Sobee.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Chat
{
    [MessageAttribute(13335)]
    public abstract class ChatBase : Message
    {
        public Guid TeamId { get; }

        public ChatBase(Guid teamId)
        {
            TeamId = teamId;
        }

        public ChatBase(BinaryReader reader) : base(reader)
        {
            TeamId = GuidConverter.ConvertFromInt(reader.method_9());
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_9(GuidConverter.ConvertToInt(TeamId));
        }
    }
}