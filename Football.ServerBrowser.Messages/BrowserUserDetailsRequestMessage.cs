using Football.Network.Messaging;
using Football.Serialization;

namespace Football.ServerBrowser.Messages
{
    [MessageAttribute(4631)]
    public class BrowserUserDetailsRequestMessage : Message
    {
        public BrowserUserDetailsRequestMessage(BinaryReader reader)
        {

        }

        public BrowserUserDetailsRequestMessage()
        {

        }

        public override void Serialize(BinaryWriter writer)
        {

        }
    }
}
