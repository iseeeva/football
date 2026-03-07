using Sobee.Network.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Player
{
    [MessageAttribute(51914)]
    public class PlayerLeaveTxMessage : Message
    {
        /// <summary> absolute </summary>
        public readonly sbyte SquadNumber;

        public PlayerLeaveTxMessage(BinaryReader reader) : base(reader)
        {
            this.SquadNumber = reader.method_11();
        }

        public PlayerLeaveTxMessage(sbyte squadNumber)
        {
            this.SquadNumber = squadNumber;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_11(this.SquadNumber);
        }
    }
}
