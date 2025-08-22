// Token: 0x02000005 RID: 5
using Sobee.Serialization;

namespace Sobee.Messaging
{
    [GAttribute0(1)]
    public class Message : IMessage
    {
        // Token: 0x06000059 RID: 89 RVA: 0x00002598 File Offset: 0x00000798
        public virtual bool vmethod_0()
        {
            return true;
        }

        // Token: 0x0600005A RID: 90 RVA: 0x0000259B File Offset: 0x0000079B
        public Message()
        {
        }

        // Token: 0x0600005B RID: 91 RVA: 0x0000259B File Offset: 0x0000079B
        public Message(BinaryReader gclass315_0)
        {
        }

        // Token: 0x0600005C RID: 92 RVA: 0x000025A3 File Offset: 0x000007A3
        public virtual void Serialize(BinaryWriter gclass316_0)
        {
        }

        // Token: 0x04000021 RID: 33
        public int byteLength;
    }
}