// Token: 0x02000013 RID: 19
public class MessageDelegateArgs : EventArgs
{
    // Token: 0x060000BE RID: 190 RVA: 0x00002B2D File Offset: 0x00000D2D
    public object method_0()
    {
        return this.sender;
    }

    // Token: 0x060000BF RID: 191 RVA: 0x00002B35 File Offset: 0x00000D35
    public MessageEventArgs method_1()
    {
        return this.GEventArgs9_0;
    }

    // Token: 0x060000C0 RID: 192 RVA: 0x00002B3D File Offset: 0x00000D3D
    public MessageDelegateArgs(object gclass306_1, MessageEventArgs gclass175_1)
    {
        this.sender = gclass306_1;
        this.GEventArgs9_0 = gclass175_1;
    }

    // Token: 0x04000046 RID: 70
    private MessageEventArgs GEventArgs9_0;

    // Token: 0x04000047 RID: 71
    private object sender;
}
