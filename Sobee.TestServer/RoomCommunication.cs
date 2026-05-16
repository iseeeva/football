using Sobee.Network.Messaging;
using Sobee.TestServer.Messages;

namespace Sobee.TestServer
{
    public class RoomCommunication : MessageCommunication
    {
        public RoomCommunication() : base()
        {
            RegisterMessages("Sobee.TestServer.Messages");
            RegisterMessage<LatencyMessage>();
        }
    }
}