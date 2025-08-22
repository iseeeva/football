
// Token: 0x0200000C RID: 12
using System.Numerics;
using Sobee.Serialization;

public class BinaryWriter
{
    // Token: 0x06000048 RID: 72 RVA: 0x000023D5 File Offset: 0x000005D5
    public BinaryWriter(Stream stream_1, GDelegate1 gdelegate1_1, GDelegate2 gdelegate2_1)
    {
        this.stream_0 = stream_1;
        if (stream_1.CanWrite)
        {
            this.binaryWriter_0 = new System.IO.BinaryWriter(stream_1);
        }
        this.gdelegate1_0 = gdelegate1_1;
        this.gdelegate2_0 = gdelegate2_1;
    }

    // Token: 0x06000049 RID: 73 RVA: 0x00002406 File Offset: 0x00000606
    public void method_0()
    {
        this.stream_0.Flush();
    }

    // Token: 0x0600004A RID: 74 RVA: 0x00002413 File Offset: 0x00000613
    public void method_1(bool bool_0)
    {
        this.binaryWriter_0.Write(bool_0);
    }

    // Token: 0x0600004B RID: 75 RVA: 0x00002421 File Offset: 0x00000621
    public void method_2(byte byte_0)
    {
        this.binaryWriter_0.Write(byte_0);
    }

    // Token: 0x0600004C RID: 76 RVA: 0x00002E9C File Offset: 0x0000109C
    public void method_3(byte[] byte_0)
    {
        uint uint_ = (uint)byte_0.Length;
        this.method_16(uint_);
        this.binaryWriter_0.Write(byte_0);
    }

    // Token: 0x0600004D RID: 77 RVA: 0x0000242F File Offset: 0x0000062F
    public void method_4(char char_0)
    {
        this.binaryWriter_0.Write(char_0);
    }

    // Token: 0x0600004E RID: 78 RVA: 0x00002EC0 File Offset: 0x000010C0
    public void method_5(char[] char_0)
    {
        uint uint_ = (uint)char_0.Length;
        this.method_16(uint_);
        this.binaryWriter_0.Write(char_0);
    }

    // Token: 0x0600004F RID: 79 RVA: 0x0000243D File Offset: 0x0000063D
    public void method_6(decimal decimal_0)
    {
        this.binaryWriter_0.Write(decimal_0);
    }

    // Token: 0x06000050 RID: 80 RVA: 0x0000244B File Offset: 0x0000064B
    public void method_7(double double_0)
    {
        this.binaryWriter_0.Write(double_0);
    }

    // Token: 0x06000051 RID: 81 RVA: 0x00002459 File Offset: 0x00000659
    public void method_8(short short_0)
    {
        this.binaryWriter_0.Write(short_0);
    }

    // Token: 0x06000052 RID: 82 RVA: 0x00002467 File Offset: 0x00000667
    public void method_9(int int_0)
    {
        this.binaryWriter_0.Write(int_0);
    }

    // Token: 0x06000053 RID: 83 RVA: 0x00002475 File Offset: 0x00000675
    public void method_10(long long_0)
    {
        this.binaryWriter_0.Write(long_0);
    }

    // Token: 0x06000054 RID: 84 RVA: 0x00002483 File Offset: 0x00000683
    public void method_11(sbyte sbyte_0)
    {
        this.binaryWriter_0.Write(sbyte_0);
    }

    // Token: 0x06000055 RID: 85 RVA: 0x00002491 File Offset: 0x00000691
    public void method_12(float float_0)
    {
        this.binaryWriter_0.Write(float_0);
    }

    // Token: 0x06000056 RID: 86 RVA: 0x0000249F File Offset: 0x0000069F
    public void method_13(float? nullable_0)
    {
        this.binaryWriter_0.Write(nullable_0 != null);
        if (nullable_0 != null)
        {
            this.binaryWriter_0.Write(nullable_0.Value);
        }
    }

    // Token: 0x06000057 RID: 87 RVA: 0x000024CE File Offset: 0x000006CE
    public void method_14(string string_0)
    {
        this.binaryWriter_0.Write((string_0 == null) ? string.Empty : string_0);
    }

    // Token: 0x06000058 RID: 88 RVA: 0x000024E6 File Offset: 0x000006E6
    public void method_15(ushort ushort_0)
    {
        this.binaryWriter_0.Write(ushort_0);
    }

    // Token: 0x06000059 RID: 89 RVA: 0x000024F4 File Offset: 0x000006F4
    public void method_16(uint uint_0)
    {
        this.binaryWriter_0.Write(uint_0);
    }

    // Token: 0x0600005A RID: 90 RVA: 0x00002502 File Offset: 0x00000702
    public void method_17(ulong ulong_0)
    {
        this.binaryWriter_0.Write(ulong_0);
    }

    // Token: 0x0600005B RID: 91 RVA: 0x00002510 File Offset: 0x00000710
    public void method_18(Guid guid_0)
    {
        this.binaryWriter_0.Write(guid_0.ToByteArray());
    }

    // Token: 0x0600005C RID: 92 RVA: 0x00002524 File Offset: 0x00000724
    public void method_19(Vector2 vector2_0)
    {
        this.binaryWriter_0.Write(vector2_0.X);
        this.binaryWriter_0.Write(vector2_0.Y);
    }

    // Token: 0x0600005D RID: 93 RVA: 0x0000254A File Offset: 0x0000074A
    public void method_20(Vector3 vector3_0)
    {
        this.binaryWriter_0.Write(vector3_0.X);
        this.binaryWriter_0.Write(vector3_0.Y);
        this.binaryWriter_0.Write(vector3_0.Z);
    }

    // Token: 0x0600005E RID: 94 RVA: 0x00002EE4 File Offset: 0x000010E4
    public void method_21(Vector4 vector4_0)
    {
        this.binaryWriter_0.Write(vector4_0.X);
        this.binaryWriter_0.Write(vector4_0.Y);
        this.binaryWriter_0.Write(vector4_0.Z);
        this.binaryWriter_0.Write(vector4_0.W);
    }

    // Token: 0x0600005F RID: 95 RVA: 0x00002F3C File Offset: 0x0000113C
    public void method_22(Matrix4x4 matrix_0)
    {
        this.binaryWriter_0.Write(matrix_0.M11);
        this.binaryWriter_0.Write(matrix_0.M12);
        this.binaryWriter_0.Write(matrix_0.M13);
        this.binaryWriter_0.Write(matrix_0.M14);
        this.binaryWriter_0.Write(matrix_0.M21);
        this.binaryWriter_0.Write(matrix_0.M22);
        this.binaryWriter_0.Write(matrix_0.M23);
        this.binaryWriter_0.Write(matrix_0.M24);
        this.binaryWriter_0.Write(matrix_0.M31);
        this.binaryWriter_0.Write(matrix_0.M32);
        this.binaryWriter_0.Write(matrix_0.M33);
        this.binaryWriter_0.Write(matrix_0.M34);
        this.binaryWriter_0.Write(matrix_0.M41);
        this.binaryWriter_0.Write(matrix_0.M42);
        this.binaryWriter_0.Write(matrix_0.M43);
        this.binaryWriter_0.Write(matrix_0.M44);
    }

    // Token: 0x06000060 RID: 96 RVA: 0x0000306C File Offset: 0x0000126C
    public void method_23(Version version_0)
    {
        this.binaryWriter_0.Write(version_0.Major);
        this.binaryWriter_0.Write(version_0.Minor);
        this.binaryWriter_0.Write(version_0.Build);
        this.binaryWriter_0.Write(version_0.Revision);
    }

    // Token: 0x06000061 RID: 97 RVA: 0x000030C0 File Offset: 0x000012C0
    public void method_24(Quaternion quaternion_0)
    {
        this.binaryWriter_0.Write(quaternion_0.X);
        this.binaryWriter_0.Write(quaternion_0.Y);
        this.binaryWriter_0.Write(quaternion_0.Z);
        this.binaryWriter_0.Write(quaternion_0.W);
    }

    // Token: 0x06000062 RID: 98 RVA: 0x00003118 File Offset: 0x00001318
    public void method_25(IMessage ginterface7_0)
    {
        MessageHelper gclass = new MessageHelper(this.stream_0, this.gdelegate1_0, this.gdelegate2_0);
        gclass.WriteMessage(ginterface7_0);
    }

    // Token: 0x04000018 RID: 24
    private Stream stream_0;

    // Token: 0x04000019 RID: 25
    private System.IO.BinaryWriter binaryWriter_0;

    // Token: 0x0400001A RID: 26
    private GDelegate1 gdelegate1_0;

    // Token: 0x0400001B RID: 27
    private GDelegate2 gdelegate2_0;
}
