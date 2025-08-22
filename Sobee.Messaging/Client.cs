using Sobee.Messaging;
using Sobee.Network;

namespace Sobee.TestServer.Common
{
    // Token: 0x02000011 RID: 17
    public class Client : Session
    {
        // Token: 0x060000B8 RID: 184 RVA: 0x00002B16 File Offset: 0x00000D16
        public Client(SocketWrapper gclass297_1, MessageDispatch gclass292_1) : base(gclass297_1, gclass292_1)
        {
        }

        // Token: 0x060000B9 RID: 185 RVA: 0x00002B20 File Offset: 0x00000D20
        public void SendHeartbeat()
        {
            this.SendMessage(new HeartbeatMessage());
        }
    }
}
