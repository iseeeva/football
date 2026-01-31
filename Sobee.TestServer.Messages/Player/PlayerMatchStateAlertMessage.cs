using Sobee.Network.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Player
{
    [Message(49149)]
    public class PlayerMatchStateAlertMessage : Message
    {
        public PlayerMatchStateAlertMessage(BinaryReader gclass315_0) : base(gclass315_0)
        {
            IsAlerted = gclass315_0.method_1();
        }

        public PlayerMatchStateAlertMessage(bool isAlerted)
        {
            IsAlerted = isAlerted;
        }

        public override void Serialize(BinaryWriter gclass316_0)
        {
            base.Serialize(gclass316_0);
            gclass316_0.method_1(IsAlerted);
        }

        public readonly bool IsAlerted;
    }

}
