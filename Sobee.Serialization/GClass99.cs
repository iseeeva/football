using System;

// Token: 0x02000008 RID: 8
public class GClass99 : ICloneable
{
	// Token: 0x06000061 RID: 97 RVA: 0x000034C8 File Offset: 0x000016C8
	public object Clone()
	{
		return new GClass99
		{
			string_0 = this.string_0,
			double_0 = this.double_0
		};
	}

	// Token: 0x0400001B RID: 27
	public string string_0 = string.Empty;

	// Token: 0x0400001C RID: 28
	public double double_0;
}
