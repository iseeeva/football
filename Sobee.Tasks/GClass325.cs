using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

// Token: 0x02000004 RID: 4
public class GClass325
{
	// Token: 0x06000023 RID: 35 RVA: 0x00002271 File Offset: 0x00000471
	[MethodImpl(MethodImplOptions.Synchronized)]
	public void method_0(EventHandler<GEventArgs11> eventHandler_4)
	{
		this.eventHandler_0 = (EventHandler<GEventArgs11>)Delegate.Combine(this.eventHandler_0, eventHandler_4);
	}

	// Token: 0x06000024 RID: 36 RVA: 0x0000228A File Offset: 0x0000048A
	[MethodImpl(MethodImplOptions.Synchronized)]
	public void method_1(EventHandler<GEventArgs11> eventHandler_4)
	{
		this.eventHandler_0 = (EventHandler<GEventArgs11>)Delegate.Remove(this.eventHandler_0, eventHandler_4);
	}

	// Token: 0x06000025 RID: 37 RVA: 0x000022A3 File Offset: 0x000004A3
	[MethodImpl(MethodImplOptions.Synchronized)]
	public void method_2(EventHandler<GEventArgs12> eventHandler_4)
	{
		this.eventHandler_1 = (EventHandler<GEventArgs12>)Delegate.Combine(this.eventHandler_1, eventHandler_4);
	}

	// Token: 0x06000026 RID: 38 RVA: 0x000022BC File Offset: 0x000004BC
	[MethodImpl(MethodImplOptions.Synchronized)]
	public void method_3(EventHandler<GEventArgs12> eventHandler_4)
	{
		this.eventHandler_1 = (EventHandler<GEventArgs12>)Delegate.Remove(this.eventHandler_1, eventHandler_4);
	}

	// Token: 0x06000027 RID: 39 RVA: 0x000022D5 File Offset: 0x000004D5
	[MethodImpl(MethodImplOptions.Synchronized)]
	public void method_4(EventHandler<GEventArgs11> eventHandler_4)
	{
		this.eventHandler_2 = (EventHandler<GEventArgs11>)Delegate.Combine(this.eventHandler_2, eventHandler_4);
	}

	// Token: 0x06000028 RID: 40 RVA: 0x000022EE File Offset: 0x000004EE
	[MethodImpl(MethodImplOptions.Synchronized)]
	public void method_5(EventHandler<GEventArgs11> eventHandler_4)
	{
		this.eventHandler_2 = (EventHandler<GEventArgs11>)Delegate.Remove(this.eventHandler_2, eventHandler_4);
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00002307 File Offset: 0x00000507
	[MethodImpl(MethodImplOptions.Synchronized)]
	public void method_6(EventHandler<GEventArgs11> eventHandler_4)
	{
		this.eventHandler_3 = (EventHandler<GEventArgs11>)Delegate.Combine(this.eventHandler_3, eventHandler_4);
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00002320 File Offset: 0x00000520
	[MethodImpl(MethodImplOptions.Synchronized)]
	public void method_7(EventHandler<GEventArgs11> eventHandler_4)
	{
		this.eventHandler_3 = (EventHandler<GEventArgs11>)Delegate.Remove(this.eventHandler_3, eventHandler_4);
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00002339 File Offset: 0x00000539
	public double method_8()
	{
		return this.double_0;
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00002341 File Offset: 0x00000541
	public int method_9()
	{
		throw new Exception("Implement this function please by keeping track of running tasks. Also think about how to interrupt of a running task to perform more...");
	}

	// Token: 0x0600002D RID: 45 RVA: 0x0000234D File Offset: 0x0000054D
	public GClass325() : this(null)
	{
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00002356 File Offset: 0x00000556
	public GClass325(object object_1)
	{
		this.object_0 = object_1;
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00002386 File Offset: 0x00000586
	public GClass324 method_10(GDelegate4 gdelegate4_0)
	{
		return this.method_14(gdelegate4_0, null, 0.0, 1, 0.0);
	}

	// Token: 0x06000030 RID: 48 RVA: 0x000023A3 File Offset: 0x000005A3
	public GClass324 method_11(GDelegate4 gdelegate4_0, double double_1)
	{
		return this.method_14(gdelegate4_0, null, double_1, 1, 0.0);
	}

	// Token: 0x06000031 RID: 49 RVA: 0x000023B8 File Offset: 0x000005B8
	public GClass324 method_12(GDelegate4 gdelegate4_0, object object_1)
	{
		return this.method_14(gdelegate4_0, object_1, 0.0, 1, 0.0);
	}

	// Token: 0x06000032 RID: 50 RVA: 0x000023D5 File Offset: 0x000005D5
	public GClass324 method_13(GDelegate4 gdelegate4_0, object object_1, double double_1)
	{
		return this.method_14(gdelegate4_0, object_1, double_1, 1, 0.0);
	}

	// Token: 0x06000033 RID: 51 RVA: 0x000023EA File Offset: 0x000005EA
	public GClass324 method_14(GDelegate4 gdelegate4_0, object object_1, double double_1, int int_0, double double_2)
	{
		return this.method_15(gdelegate4_0, object_1, double_1, int_0, double_2, this.object_0);
	}

	// Token: 0x06000034 RID: 52 RVA: 0x0000278C File Offset: 0x0000098C
	public GClass324 method_15(GDelegate4 gdelegate4_0, object object_1, double double_1, int int_0, double double_2, object object_2)
	{
		GClass324 gclass = new GClass324(gdelegate4_0, object_1, this.double_0, double_1 + this.double_0, int_0, double_2, false, object_2);
		gclass.method_8(new EventHandler(this.method_35));
		gclass.method_10(new EventHandler<GEventArgs13>(this.method_36));
		lock (this.list_0)
		{
			this.list_0.Add(gclass);
		}
		return gclass;
	}

	// Token: 0x06000035 RID: 53 RVA: 0x000023FF File Offset: 0x000005FF
	public GClass324 method_16(GDelegate4 gdelegate4_0)
	{
		return this.method_20(gdelegate4_0, null, 0.0, 1, 0.0);
	}

	// Token: 0x06000036 RID: 54 RVA: 0x0000241C File Offset: 0x0000061C
	public GClass324 method_17(GDelegate4 gdelegate4_0, double double_1)
	{
		return this.method_20(gdelegate4_0, null, double_1, 1, 0.0);
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00002431 File Offset: 0x00000631
	public GClass324 method_18(GDelegate4 gdelegate4_0, object object_1)
	{
		return this.method_20(gdelegate4_0, object_1, 0.0, 1, 0.0);
	}

	// Token: 0x06000038 RID: 56 RVA: 0x0000244E File Offset: 0x0000064E
	public GClass324 method_19(GDelegate4 gdelegate4_0, object object_1, double double_1)
	{
		return this.method_20(gdelegate4_0, object_1, double_1, 1, 0.0);
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00002463 File Offset: 0x00000663
	public GClass324 method_20(GDelegate4 gdelegate4_0, object object_1, double double_1, int int_0, double double_2)
	{
		return this.method_21(gdelegate4_0, object_1, double_1, int_0, double_2, this.object_0);
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00002810 File Offset: 0x00000A10
	public GClass324 method_21(GDelegate4 gdelegate4_0, object object_1, double double_1, int int_0, double double_2, object object_2)
	{
		GClass324 gclass = new GClass324(gdelegate4_0, object_1, this.double_0, double_1 + this.double_0, int_0, double_2, true, object_2);
		gclass.method_8(new EventHandler(this.method_35));
		gclass.method_10(new EventHandler<GEventArgs13>(this.method_36));
		lock (this.list_0)
		{
			this.list_0.Add(gclass);
		}
		return gclass;
	}

	// Token: 0x0600003B RID: 59 RVA: 0x00002478 File Offset: 0x00000678
	public GClass324 method_22(GDelegate4 gdelegate4_0)
	{
		return this.method_26(gdelegate4_0, null, 0.0, 1, 0.0);
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00002495 File Offset: 0x00000695
	public GClass324 method_23(GDelegate4 gdelegate4_0, object object_1)
	{
		return this.method_26(gdelegate4_0, object_1, 0.0, 1, 0.0);
	}

	// Token: 0x0600003D RID: 61 RVA: 0x000024B2 File Offset: 0x000006B2
	public GClass324 method_24(GDelegate4 gdelegate4_0, double double_1)
	{
		return this.method_26(gdelegate4_0, null, double_1, 1, 0.0);
	}

	// Token: 0x0600003E RID: 62 RVA: 0x000024C7 File Offset: 0x000006C7
	public GClass324 method_25(GDelegate4 gdelegate4_0, object object_1, double double_1)
	{
		return this.method_26(gdelegate4_0, object_1, double_1, 1, 0.0);
	}

	// Token: 0x0600003F RID: 63 RVA: 0x000024DC File Offset: 0x000006DC
	public GClass324 method_26(GDelegate4 gdelegate4_0, object object_1, double double_1, int int_0, double double_2)
	{
		return this.method_27(gdelegate4_0, object_1, double_1, int_0, double_2, this.object_0);
	}

	// Token: 0x06000040 RID: 64 RVA: 0x00002894 File Offset: 0x00000A94
	public GClass324 method_27(GDelegate4 gdelegate4_0, object object_1, double double_1, int int_0, double double_2, object object_2)
	{
		GClass324 gclass = this.method_15(gdelegate4_0, object_1, double_1, int_0, double_2, object_2);
		gclass.method_21();
		return gclass;
	}

	// Token: 0x06000041 RID: 65 RVA: 0x000024F1 File Offset: 0x000006F1
	public GClass324 method_28(GDelegate4 gdelegate4_0)
	{
		return this.method_32(gdelegate4_0, null, 0.0, 1, 0.0);
	}

	// Token: 0x06000042 RID: 66 RVA: 0x0000250E File Offset: 0x0000070E
	public GClass324 method_29(GDelegate4 gdelegate4_0, object object_1)
	{
		return this.method_32(gdelegate4_0, object_1, 0.0, 1, 0.0);
	}

	// Token: 0x06000043 RID: 67 RVA: 0x0000252B File Offset: 0x0000072B
	public GClass324 method_30(GDelegate4 gdelegate4_0, double double_1)
	{
		return this.method_32(gdelegate4_0, null, double_1, 1, 0.0);
	}

	// Token: 0x06000044 RID: 68 RVA: 0x00002540 File Offset: 0x00000740
	public GClass324 method_31(GDelegate4 gdelegate4_0, object object_1, double double_1)
	{
		return this.method_32(gdelegate4_0, object_1, double_1, 1, 0.0);
	}

	// Token: 0x06000045 RID: 69 RVA: 0x00002555 File Offset: 0x00000755
	public GClass324 method_32(GDelegate4 gdelegate4_0, object object_1, double double_1, int int_0, double double_2)
	{
		return this.method_33(gdelegate4_0, object_1, double_1, int_0, double_2, this.object_0);
	}

	// Token: 0x06000046 RID: 70 RVA: 0x000028B8 File Offset: 0x00000AB8
	public GClass324 method_33(GDelegate4 gdelegate4_0, object object_1, double double_1, int int_0, double double_2, object object_2)
	{
		GClass324 gclass = this.method_21(gdelegate4_0, object_1, double_1, int_0, double_2, object_2);
		gclass.method_21();
		return gclass;
	}

	// Token: 0x06000047 RID: 71 RVA: 0x000028DC File Offset: 0x00000ADC
	public void method_34(double double_1)
	{
		this.double_0 += double_1;
		List<GClass324> list;
		lock (this.list_0)
		{
			list = new List<GClass324>(this.list_0);
		}
		List<GClass324> list2 = new List<GClass324>();
		for (int i = 0; i < list.Count; i++)
		{
			GClass324 gclass = list[i];
			if (gclass.method_18() && this.double_0 >= gclass.method_13())
			{
				list2.Add(gclass);
				if (gclass.method_16() == 0)
				{
					gclass.vmethod_2();
					this.vmethod_0(gclass);
				}
				else
				{
					gclass.vmethod_3();
					this.vmethod_1(gclass);
				}
				if (gclass.method_20())
				{
					gclass.method_22(null);
				}
				else
				{
					ThreadPool.QueueUserWorkItem(new WaitCallback(gclass.method_22));
				}
			}
		}
		lock (this.list_0)
		{
			for (int j = 0; j < list2.Count; j++)
			{
				this.list_0.Remove(list2[j]);
			}
		}
		List<GClass324> list3 = null;
		lock (this.list_1)
		{
			list3 = new List<GClass324>(this.list_1);
			this.list_1.Clear();
		}
		int k = 0;
		while (k < list3.Count)
		{
			GClass324 gclass2 = list3[k];
			if (gclass2.method_16() < gclass2.method_14())
			{
				goto IL_166;
			}
			if (gclass2.method_14() == -1)
			{
				goto IL_166;
			}
			gclass2.vmethod_4();
			this.vmethod_2(gclass2);
			IL_173:
			k++;
			continue;
			IL_166:
			this.list_0.Add(gclass2);
			goto IL_173;
		}
		List<KeyValuePair<GClass324, GEventArgs13>> list4 = null;
		lock (this.list_2)
		{
			list4 = new List<KeyValuePair<GClass324, GEventArgs13>>(this.list_2);
			this.list_2.Clear();
		}
		for (int l = 0; l < list4.Count; l++)
		{
			list4[l].Key.vmethod_5(list4[l].Value.method_0());
			this.vmethod_3(list4[l].Key, list4[l].Value.method_0());
		}
	}

	// Token: 0x06000048 RID: 72 RVA: 0x0000256A File Offset: 0x0000076A
	protected virtual void vmethod_0(GClass324 gclass324_0)
	{
		if (this.eventHandler_0 != null)
		{
			this.eventHandler_0(this, new GEventArgs11(gclass324_0));
		}
	}

	// Token: 0x06000049 RID: 73 RVA: 0x00002586 File Offset: 0x00000786
	protected virtual void vmethod_1(GClass324 gclass324_0)
	{
		if (this.eventHandler_3 != null)
		{
			this.eventHandler_3(this, new GEventArgs11(gclass324_0));
		}
	}

	// Token: 0x0600004A RID: 74 RVA: 0x000025A2 File Offset: 0x000007A2
	protected virtual void vmethod_2(GClass324 gclass324_0)
	{
		if (this.eventHandler_2 != null)
		{
			this.eventHandler_2(this, new GEventArgs11(gclass324_0));
		}
	}

	// Token: 0x0600004B RID: 75 RVA: 0x000025BE File Offset: 0x000007BE
	protected virtual void vmethod_3(GClass324 gclass324_0, Exception exception_0)
	{
		if (this.eventHandler_1 != null)
		{
			this.eventHandler_1(this, new GEventArgs12(gclass324_0, exception_0));
		}
	}

	// Token: 0x0600004C RID: 76 RVA: 0x00002B48 File Offset: 0x00000D48
	private void method_35(object sender, EventArgs e)
	{
		GClass324 item = (GClass324)sender;
		lock (this.list_1)
		{
			this.list_1.Add(item);
		}
	}

	// Token: 0x0600004D RID: 77 RVA: 0x00002B90 File Offset: 0x00000D90
	private void method_36(object sender, GEventArgs13 e)
	{
		GClass324 key = (GClass324)sender;
		lock (this.list_2)
		{
			this.list_2.Add(new KeyValuePair<GClass324, GEventArgs13>(key, e));
		}
	}

	// Token: 0x04000011 RID: 17
	private EventHandler<GEventArgs11> eventHandler_0;

	// Token: 0x04000012 RID: 18
	private EventHandler<GEventArgs12> eventHandler_1;

	// Token: 0x04000013 RID: 19
	private EventHandler<GEventArgs11> eventHandler_2;

	// Token: 0x04000014 RID: 20
	private EventHandler<GEventArgs11> eventHandler_3;

	// Token: 0x04000015 RID: 21
	private List<GClass324> list_0 = new List<GClass324>();

	// Token: 0x04000016 RID: 22
	private List<GClass324> list_1 = new List<GClass324>();

	// Token: 0x04000017 RID: 23
	private List<KeyValuePair<GClass324, GEventArgs13>> list_2 = new List<KeyValuePair<GClass324, GEventArgs13>>();

	// Token: 0x04000018 RID: 24
	private double double_0;

	// Token: 0x04000019 RID: 25
	private object object_0;
}
