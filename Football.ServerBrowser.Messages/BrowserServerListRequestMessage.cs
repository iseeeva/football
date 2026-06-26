using Football.Network.Messaging;
using Football.Serialization;

namespace Football.ServerBrowser.Messages
{

    [MessageAttribute(46212)]
    public class BrowserServerListRequestMessage : Message
    {
        public BrowserServerListRequestMessage(BinaryReader reader)
        {

        }

        public BrowserServerListRequestMessage()
        {

        }

        public override void Serialize(BinaryWriter writer)
        {

        }
    }
}
