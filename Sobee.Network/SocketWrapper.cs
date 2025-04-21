using System.ComponentModel;
using System.Net;
using System.Net.Sockets;

// Token: 0x02000008 RID: 8
[ToolboxItem(false)]
public class SocketWrapper : SocketQueueHandler
{
    // Token: 0x06000052 RID: 82 RVA: 0x000025C1 File Offset: 0x000007C1
    public SocketWrapper(Socket socket_1) : base(socket_1, (IPEndPoint)socket_1.RemoteEndPoint)
    {
        base.BeginReceive();
    }

}
