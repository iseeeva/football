using System.Net.Sockets;
using Sobee.Network;


// Token: 0x02000009 RID: 9
public class GEventArgs10 : EventArgs
{
    // Token: 0x06000053 RID: 83 RVA: 0x000025DB File Offset: 0x000007DB
    public SocketError method_0()
    {
        return this.socketError_0;
    }

    // Token: 0x06000054 RID: 84 RVA: 0x000025E3 File Offset: 0x000007E3
    public ConnectionError method_1()
    {
        return this.connectionError_0;
    }

    // Token: 0x06000055 RID: 85 RVA: 0x000025EB File Offset: 0x000007EB
    public SocketQueueHandler method_2()
    {
        return this.SocketHandle_0;
    }

    // Token: 0x06000056 RID: 86 RVA: 0x000025F3 File Offset: 0x000007F3
    public GEventArgs10(SocketError socketError_1)
    {
        this.connectionError_0 = ConnectionError.SocketError;
        this.socketError_0 = socketError_1;
    }

    // Token: 0x06000057 RID: 87 RVA: 0x00002609 File Offset: 0x00000809
    public GEventArgs10(ConnectionError connectionError_1)
    {
        this.connectionError_0 = connectionError_1;
        this.socketError_0 = SocketError.Success;
    }

    // Token: 0x06000058 RID: 88 RVA: 0x0000261F File Offset: 0x0000081F
    public GEventArgs10(SocketQueueHandler SocketHandle_1)
    {
        this.SocketHandle_0 = SocketHandle_1;
    }

    // Token: 0x06000059 RID: 89 RVA: 0x0000262E File Offset: 0x0000082E
    public override string ToString()
    {
        if (this.connectionError_0 == ConnectionError.SocketError)
        {
            return "SocketError: " + this.socketError_0.ToString();
        }
        return "ConnectionError: " + this.connectionError_0.ToString();
    }

    // Token: 0x04000026 RID: 38
    private SocketQueueHandler SocketHandle_0;

    // Token: 0x04000027 RID: 39
    private ConnectionError connectionError_0;

    // Token: 0x04000028 RID: 40
    private SocketError socketError_0;
}
