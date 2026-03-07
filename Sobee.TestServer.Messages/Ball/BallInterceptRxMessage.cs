using Sobee.Network.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Ball
{
    [MessageAttribute(48418)]
    public class BallInterceptRxMessage : Message
    {
        public readonly InterceptCategory InterceptCategory;

        public BallInterceptRxMessage(BinaryReader reader) : base(reader)
        {
            this.InterceptCategory = (InterceptCategory)reader.method_9();
        }

        public BallInterceptRxMessage(InterceptCategory interceptCategory)
        {
            this.InterceptCategory = interceptCategory;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_9((int)this.InterceptCategory);
        }
    }
}
