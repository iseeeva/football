using Sobee.Network.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages
{
    // Token: 0x02000007 RID: 7
    [MessageAttribute(2)]
    public class LatencyMessage : Message
    {
        // Token: 0x06000061 RID: 97 RVA: 0x000025C2 File Offset: 0x000007C2
        public float method_0()
        {
            return this.float_0;
        }

        // Token: 0x06000062 RID: 98 RVA: 0x000025A5 File Offset: 0x000007A5
        public override bool vmethod_0()
        {
            return false;
        }

        // Token: 0x06000063 RID: 99 RVA: 0x000025CA File Offset: 0x000007CA
        public LatencyMessage(float float_1)
        {
            this.float_0 = float_1;
        }

        // Token: 0x06000064 RID: 100 RVA: 0x000025D9 File Offset: 0x000007D9
        public LatencyMessage(BinaryReader reader) : base(reader)
        {
            this.float_0 = reader.method_12();
        }

        // Token: 0x06000065 RID: 101 RVA: 0x000025EE File Offset: 0x000007EE
        public override void Serialize(BinaryWriter gclass316_0)
        {
            base.Serialize(gclass316_0);
            gclass316_0.method_12(this.float_0);
        }

        // Token: 0x04000022 RID: 34
        private float float_0;
    }

}
