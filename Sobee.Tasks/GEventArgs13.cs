using System;

// Token: 0x02000007 RID: 7
public class GEventArgs13 : EventArgs
{
	// Token: 0x06000052 RID: 82 RVA: 0x0000260A File Offset: 0x0000080A
	public Exception method_0()
	{
		return this.exception_0;
	}

	// Token: 0x06000053 RID: 83 RVA: 0x00002612 File Offset: 0x00000812
	public GEventArgs13(Exception exception_1)
	{
		this.exception_0 = exception_1;
	}

	// Token: 0x0400001C RID: 28
	private Exception exception_0;
}
