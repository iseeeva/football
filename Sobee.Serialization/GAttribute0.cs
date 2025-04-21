// Token: 0x02000008 RID: 8
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
public sealed class GAttribute0 : Attribute
{
    // Token: 0x06000018 RID: 24 RVA: 0x00002102 File Offset: 0x00000302
    public ushort method_0()
    {
        return this.Id;
    }

    // Token: 0x06000019 RID: 25 RVA: 0x0000210A File Offset: 0x0000030A
    public GAttribute0(ushort ushort_1)
    {
        this.Id = ushort_1;
    }

    // Token: 0x0400000C RID: 12
    private ushort Id;
}
