using Sobee.Network.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Player
{
    [MessageAttribute(51914)]
    public class PlayerLeaveMessage : Message
    {
        public PlayerLeaveMessage(BinaryReader gclass315_0) : base(gclass315_0)
        {
            this.SquadNumber = gclass315_0.method_11();
        }

        public PlayerLeaveMessage(sbyte sbyte_1)
        {
            this.SquadNumber = sbyte_1;
        }

        public override void Serialize(BinaryWriter gclass316_0)
        {
            base.Serialize(gclass316_0);
            gclass316_0.method_11(this.SquadNumber);
        }

        /// <summary>
        /// (not splited)
        /// </summary>
        public readonly sbyte SquadNumber;
    }
}
