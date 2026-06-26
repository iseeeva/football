using Football.Network.Messaging;
using Football.Serialization;

namespace Football.GameServer.Messages.Ball
{
    [Message(48418)]
    public class BallInterceptRxMessage : Message
    {
        public readonly InterceptCategory InterceptCategory;

        public BallInterceptRxMessage(BinaryReader reader) : base(reader)
        {
            InterceptCategory = (InterceptCategory)reader.method_9();
        }

        public BallInterceptRxMessage(InterceptCategory interceptCategory)
        {
            InterceptCategory = interceptCategory;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_9((int)InterceptCategory);
        }
    }
}
