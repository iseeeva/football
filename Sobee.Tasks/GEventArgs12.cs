using System;

// Token: 0x02000006 RID: 6
public class GEventArgs12 : GEventArgs11
{
	// Token: 0x06000050 RID: 80 RVA: 0x000025F2 File Offset: 0x000007F2
	public Exception method_1()
	{
		return this.exception_0;
	}

	// Token: 0x06000051 RID: 81 RVA: 0x000025FA File Offset: 0x000007FA
	public GEventArgs12(GClass324 gclass324_1, Exception exception_1) : base(gclass324_1)
	{
		this.exception_0 = exception_1;
	}

	// Token: 0x0400001B RID: 27
	private Exception exception_0;
}
