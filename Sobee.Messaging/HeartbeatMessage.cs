namespace Sobee.Messaging
{
    // Token: 0x02000006 RID: 6
    [GAttribute0(3)]
    public class HeartbeatMessage : Message
    {
        // Token: 0x0600005D RID: 93 RVA: 0x000025A5 File Offset: 0x000007A5
        public override bool vmethod_0()
        {
            return false;
        }

        // Token: 0x0600005E RID: 94 RVA: 0x000025A8 File Offset: 0x000007A8
        public HeartbeatMessage()
        {
        }

        // Token: 0x0600005F RID: 95 RVA: 0x000025B0 File Offset: 0x000007B0
        public HeartbeatMessage(BinaryReader gclass315_0) : base(gclass315_0)
        {
        }

        // Token: 0x06000060 RID: 96 RVA: 0x000025B9 File Offset: 0x000007B9
        public override void Serialize(BinaryWriter gclass316_0)
        {
            base.Serialize(gclass316_0);
        }
    }

}
