using System.Runtime.CompilerServices;

// Token: 0x02000002 RID: 2
public class GClass324
{
    // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void method_0(EventHandler eventHandler_6)
    {
        this.eventHandler_0 = (EventHandler)Delegate.Combine(this.eventHandler_0, eventHandler_6);
    }

    // Token: 0x06000002 RID: 2 RVA: 0x00002069 File Offset: 0x00000269
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void method_1(EventHandler eventHandler_6)
    {
        this.eventHandler_0 = (EventHandler)Delegate.Remove(this.eventHandler_0, eventHandler_6);
    }

    // Token: 0x06000003 RID: 3 RVA: 0x00002082 File Offset: 0x00000282
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void method_2(EventHandler eventHandler_6)
    {
        this.eventHandler_1 = (EventHandler)Delegate.Combine(this.eventHandler_1, eventHandler_6);
    }

    // Token: 0x06000004 RID: 4 RVA: 0x0000209B File Offset: 0x0000029B
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void method_3(EventHandler eventHandler_6)
    {
        this.eventHandler_1 = (EventHandler)Delegate.Remove(this.eventHandler_1, eventHandler_6);
    }

    // Token: 0x06000005 RID: 5 RVA: 0x000020B4 File Offset: 0x000002B4
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void method_4(EventHandler eventHandler_6)
    {
        this.eventHandler_2 = (EventHandler)Delegate.Combine(this.eventHandler_2, eventHandler_6);
    }

    // Token: 0x06000006 RID: 6 RVA: 0x000020CD File Offset: 0x000002CD
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void method_5(EventHandler eventHandler_6)
    {
        this.eventHandler_2 = (EventHandler)Delegate.Remove(this.eventHandler_2, eventHandler_6);
    }

    // Token: 0x06000007 RID: 7 RVA: 0x000020E6 File Offset: 0x000002E6
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void method_6(EventHandler<GEventArgs13> eventHandler_6)
    {
        this.eventHandler_3 = (EventHandler<GEventArgs13>)Delegate.Combine(this.eventHandler_3, eventHandler_6);
    }

    // Token: 0x06000008 RID: 8 RVA: 0x000020FF File Offset: 0x000002FF
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void method_7(EventHandler<GEventArgs13> eventHandler_6)
    {
        this.eventHandler_3 = (EventHandler<GEventArgs13>)Delegate.Remove(this.eventHandler_3, eventHandler_6);
    }

    // Token: 0x06000009 RID: 9 RVA: 0x00002118 File Offset: 0x00000318
    [MethodImpl(MethodImplOptions.Synchronized)]
    internal void method_8(EventHandler eventHandler_6)
    {
        this.eventHandler_4 = (EventHandler)Delegate.Combine(this.eventHandler_4, eventHandler_6);
    }

    // Token: 0x0600000A RID: 10 RVA: 0x00002131 File Offset: 0x00000331
    [MethodImpl(MethodImplOptions.Synchronized)]
    internal void method_9(EventHandler eventHandler_6)
    {
        this.eventHandler_4 = (EventHandler)Delegate.Remove(this.eventHandler_4, eventHandler_6);
    }

    // Token: 0x0600000B RID: 11 RVA: 0x0000214A File Offset: 0x0000034A
    [MethodImpl(MethodImplOptions.Synchronized)]
    internal void method_10(EventHandler<GEventArgs13> eventHandler_6)
    {
        this.eventHandler_5 = (EventHandler<GEventArgs13>)Delegate.Combine(this.eventHandler_5, eventHandler_6);
    }

    // Token: 0x0600000C RID: 12 RVA: 0x00002163 File Offset: 0x00000363
    [MethodImpl(MethodImplOptions.Synchronized)]
    internal void method_11(EventHandler<GEventArgs13> eventHandler_6)
    {
        this.eventHandler_5 = (EventHandler<GEventArgs13>)Delegate.Remove(this.eventHandler_5, eventHandler_6);
    }

    // Token: 0x0600000D RID: 13 RVA: 0x0000217C File Offset: 0x0000037C
    public object method_12()
    {
        return this.object_0;
    }

    // Token: 0x0600000E RID: 14 RVA: 0x00002184 File Offset: 0x00000384
    public double method_13()
    {
        return this.double_1;
    }

    // Token: 0x0600000F RID: 15 RVA: 0x0000218C File Offset: 0x0000038C
    public int method_14()
    {
        return this.int_0;
    }

    // Token: 0x06000010 RID: 16 RVA: 0x00002194 File Offset: 0x00000394
    public double method_15()
    {
        return this.double_2;
    }

    // Token: 0x06000011 RID: 17 RVA: 0x0000219C File Offset: 0x0000039C
    public int method_16()
    {
        return this.int_1;
    }

    // Token: 0x06000012 RID: 18 RVA: 0x000021A4 File Offset: 0x000003A4
    internal GDelegate4 method_17()
    {
        return this.gdelegate4_0;
    }

    // Token: 0x06000013 RID: 19 RVA: 0x000021AC File Offset: 0x000003AC
    internal bool method_18()
    {
        return this.bool_0;
    }

    // Token: 0x06000014 RID: 20 RVA: 0x000021B4 File Offset: 0x000003B4
    internal double method_19()
    {
        return this.double_0;
    }

    // Token: 0x06000015 RID: 21 RVA: 0x000021BC File Offset: 0x000003BC
    internal bool method_20()
    {
        return this.bool_1;
    }

    // Token: 0x06000016 RID: 22 RVA: 0x00002624 File Offset: 0x00000824
    internal GClass324(GDelegate4 gdelegate4_1, object object_2, double double_3, double double_4, int int_2, double double_5, bool bool_2, object object_3)
    {
        this.gdelegate4_0 = gdelegate4_1;
        this.object_0 = object_2;
        this.double_0 = double_3;
        this.double_1 = double_4;
        this.int_0 = int_2;
        this.double_2 = double_5;
        this.bool_1 = bool_2;
        this.object_1 = object_3;
    }

    // Token: 0x06000017 RID: 23 RVA: 0x000021C4 File Offset: 0x000003C4
    public void method_21()
    {
        this.bool_0 = true;
    }

    // Token: 0x06000018 RID: 24 RVA: 0x000021CD File Offset: 0x000003CD
    protected virtual void vmethod_0()
    {
        if (this.eventHandler_4 != null)
        {
            this.eventHandler_4(this, EventArgs.Empty);
        }
    }

    // Token: 0x06000019 RID: 25 RVA: 0x000021E8 File Offset: 0x000003E8
    protected virtual void vmethod_1(Exception exception_0)
    {
        if (this.eventHandler_5 != null)
        {
            this.eventHandler_5(this, new GEventArgs13(exception_0));
        }
    }

    // Token: 0x0600001A RID: 26 RVA: 0x00002204 File Offset: 0x00000404
    protected internal virtual void vmethod_2()
    {
        if (this.eventHandler_0 != null)
        {
            this.eventHandler_0(this, EventArgs.Empty);
        }
    }

    // Token: 0x0600001B RID: 27 RVA: 0x0000221F File Offset: 0x0000041F
    protected internal virtual void vmethod_3()
    {
        if (this.eventHandler_2 != null)
        {
            this.eventHandler_2(this, EventArgs.Empty);
        }
    }

    // Token: 0x0600001C RID: 28 RVA: 0x0000223A File Offset: 0x0000043A
    protected internal virtual void vmethod_4()
    {
        if (this.eventHandler_1 != null)
        {
            this.eventHandler_1(this, EventArgs.Empty);
        }
    }

    // Token: 0x0600001D RID: 29 RVA: 0x00002255 File Offset: 0x00000455
    protected internal virtual void vmethod_5(Exception exception_0)
    {
        if (this.eventHandler_3 != null)
        {
            this.eventHandler_3(this, new GEventArgs13(exception_0));
        }
    }

    // Token: 0x0600001E RID: 30 RVA: 0x00002674 File Offset: 0x00000874
    internal void method_22(object object_2)
    {
        try
        {
            if (this.object_1 != null && !this.method_20())
            {
                lock (this.object_1)
                {
                    this.gdelegate4_0(this.object_0);
                    goto IL_48;
                }
            }
            this.gdelegate4_0(this.object_0);
        IL_48:;
        }
        catch (Exception ex)
        {
            //GClass149.gclass149_0.vmethod_1(string.Concat(new string[]
            //{
            //	"Exception at task Execute",
            //	this.bool_1 ? "Sync" : "Async",
            //	" for ",
            //	this.gdelegate4_0.Method.Name,
            //	"\n",
            //	ex.ToString()
            //}));
            this.vmethod_1(ex);
        }
        finally
        {
            this.int_1++;
            this.double_1 += this.double_2;
            this.vmethod_0();
        }
    }

    // Token: 0x04000001 RID: 1
    private EventHandler eventHandler_0;

    // Token: 0x04000002 RID: 2
    private EventHandler eventHandler_1;

    // Token: 0x04000003 RID: 3
    private EventHandler eventHandler_2;

    // Token: 0x04000004 RID: 4
    private EventHandler<GEventArgs13> eventHandler_3;

    // Token: 0x04000005 RID: 5
    private EventHandler eventHandler_4;

    // Token: 0x04000006 RID: 6
    private EventHandler<GEventArgs13> eventHandler_5;

    // Token: 0x04000007 RID: 7
    private GDelegate4 gdelegate4_0;

    // Token: 0x04000008 RID: 8
    private object object_0;

    // Token: 0x04000009 RID: 9
    private bool bool_0;

    // Token: 0x0400000A RID: 10
    private double double_0;

    // Token: 0x0400000B RID: 11
    private double double_1;

    // Token: 0x0400000C RID: 12
    private double double_2;

    // Token: 0x0400000D RID: 13
    private int int_0;

    // Token: 0x0400000E RID: 14
    private int int_1;

    // Token: 0x0400000F RID: 15
    private bool bool_1;

    // Token: 0x04000010 RID: 16
    private object object_1;
}
