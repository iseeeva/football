using Football.Serialization;

namespace Football.Network.Messaging
{
    [Message(1)]
    public class Message : IMessage
    {
        public virtual bool vmethod_0()
        {
            return true;
        }

        public Message()
        {
        }

        public Message(BinaryReader gclass315_0)
        {
        }

        public virtual void Serialize(BinaryWriter gclass316_0)
        {
        }

        public int byteLength;
    }
}