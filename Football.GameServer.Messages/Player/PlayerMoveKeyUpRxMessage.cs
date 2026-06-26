using Football.Network.Messaging;
using Football.Serialization;

namespace Football.GameServer.Messages.Player
{
    [Message(13399)]
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
