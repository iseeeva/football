using Football.Network.Messaging;
using Football.Serialization;

namespace Football.ServerBrowser.Messages
{
    [MessageAttribute(19328)]
    public class BrowserStatusResponseMessage : Message
    {
        public readonly bool Status;
        public readonly string StatusMessage;

        public BrowserStatusResponseMessage(BinaryReader reader)
        {
            Status = reader.method_1();
            StatusMessage = reader.method_14();
        }

        public BrowserStatusResponseMessage(bool status, string statusMessage)
        {
            Status = status;
            StatusMessage = statusMessage;
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.method_1(Status);
            writer.method_14(StatusMessage);
        }

        public override string ToString()
        {
            return $"Status: {Status}, StatusMessage: {StatusMessage}";
        }
    }
}
