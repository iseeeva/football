using System;
using System.ComponentModel;

namespace Sobee.Messaging
{
	// Token: 0x02000014 RID: 20
	[TypeConverter(typeof(EnumConverter))]
	public enum SessionType
	{
		// Token: 0x04000049 RID: 73
		Authentication = 1,
		// Token: 0x0400004A RID: 74
		Main,
		// Token: 0x0400004B RID: 75
		Patcher,
		// Token: 0x0400004C RID: 76
		Chat,
		// Token: 0x0400004D RID: 77
		Game,
		// Token: 0x0400004E RID: 78
		User,
		// Token: 0x0400004F RID: 79
		Admin
	}
}
