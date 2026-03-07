using Sobee.Network.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Player
{
    [MessageAttribute(13399)]
    public class PlayerMoveKeyUpRxMessage : Message
    {
        public PlayerMoveKeyUpRxMessage()
        {

        }

        public PlayerMoveKeyUpRxMessage(BinaryReader reader) : base(reader)
        {

        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
        }
    }

}
