using Sobee.Network.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Ball
{
    [MessageAttribute(48418)]
    public class BallInterceptMessage : Message
    {
        public readonly InterceptCategory interceptCategory_0;

        public BallInterceptMessage(BinaryReader reader) : base(reader)
        {
            this.interceptCategory_0 = (InterceptCategory)reader.method_9();
        }

        public BallInterceptMessage(InterceptCategory interceptCategory_1)
        {
            this.interceptCategory_0 = interceptCategory_1;
        }

        public override void Serialize(BinaryWriter gclass316_0)
        {
            base.Serialize(gclass316_0);
            gclass316_0.method_9((int)this.interceptCategory_0);
        }
    }
}
