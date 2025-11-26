using Sobee.Network.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Match
{
    // INFO: Client, kickoff atisindan hemen sonra gonderiyor.

    [MessageAttribute(49149)]
    public class MatchStateAlert : Message
    {
        public MatchStateAlert(BinaryReader gclass315_0) : base(gclass315_0)
        {
            this.IsAlerted = gclass315_0.method_1();
        }

        public MatchStateAlert(bool isAlerted)
        {
            this.IsAlerted = isAlerted;
        }

        public override void Serialize(BinaryWriter gclass316_0)
        {
            base.Serialize(gclass316_0);
            gclass316_0.method_1(this.IsAlerted);
        }

        public readonly bool IsAlerted;
    }

}
