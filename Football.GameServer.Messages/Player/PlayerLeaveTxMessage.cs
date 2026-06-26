using Football.Network.Messaging;
using Football.Serialization;

namespace Football.GameServer.Messages.Player
{
    [Message(51914)]
    public class PlayerLeaveTxMessage : Message
    {
        /// <summary> absolute </summary>
        public readonly sbyte SquadNumber;

        public PlayerLeaveTxMessage(BinaryReader reader) : base(reader)
        {
            SquadNumber = reader.method_11();
        }

        public PlayerLeaveTxMessage(sbyte squadNumber)
        {
            SquadNumber = squadNumber;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_11(SquadNumber);
        }
    }
}
