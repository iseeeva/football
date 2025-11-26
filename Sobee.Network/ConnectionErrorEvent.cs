using System.Net.Sockets;
using Sobee.Network;

// Token: 0x02000009 RID: 9
public class ConnectionErrorEvent : EventArgs
{
    // Token: 0x06000053 RID: 83 RVA: 0x000025DB File Offset: 0x000007DB
    public SocketError method_0()
    {
        return this.SocketError;
    }

    // Token: 0x06000054 RID: 84 RVA: 0x000025E3 File Offset: 0x000007E3
    public ConnectionError method_1()
    {
        return this.ConnectionError;
    }

    // Token: 0x06000055 RID: 85 RVA: 0x000025EB File Offset: 0x000007EB
    public SocketWrapper method_2()
    {
        return this.Socket;
    }

    // Token: 0x06000056 RID: 86 RVA: 0x000025F3 File Offset: 0x000007F3
    public ConnectionErrorEvent(SocketError socketError_1)
    {
        this.ConnectionError = ConnectionError.SocketError;
        this.SocketError = socketError_1;
    }

    // Token: 0x06000057 RID: 87 RVA: 0x00002609 File Offset: 0x00000809
    public ConnectionErrorEvent(ConnectionError connectionError_1)
    {
        this.ConnectionError = connectionError_1;
        this.SocketError = SocketError.Success;
    }

    // Token: 0x06000058 RID: 88 RVA: 0x0000261F File Offset: 0x0000081F
    public ConnectionErrorEvent(SocketWrapper gclass297_1)
    {
        this.Socket = gclass297_1;
    }

    // Token: 0x06000059 RID: 89 RVA: 0x0000262E File Offset: 0x0000082E
    public override string ToString()
    {
        if (this.ConnectionError == ConnectionError.SocketError)
        {
            return "SocketError: " + this.SocketError.ToString();
        }
        return "ConnectionError: " + this.ConnectionError.ToString();
    }

    // Token: 0x04000026 RID: 38
    private SocketWrapper Socket;

    // Token: 0x04000027 RID: 39
    private ConnectionError ConnectionError;

    // Token: 0x04000028 RID: 40
    private SocketError SocketError;
}
