using Football.Network.Messaging;
using Football.Serialization;

namespace Football.GameServer.Messages.Chat
{
    [Message(13335)]
    public abstract class ChatPlayerTextAbstractMessage : Message
    {
        public readonly Guid TeamId;

        public ChatPlayerTextAbstractMessage(Guid teamId)
        {
            TeamId = teamId;
        }

        public ChatPlayerTextAbstractMessage(BinaryReader reader) : base(reader)
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