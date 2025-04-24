// Token: 0x0200000B RID: 11
using System.Numerics;

public class BinaryReader
{
    // Token: 0x0600002D RID: 45 RVA: 0x000021EF File Offset: 0x000003EF
    public BinaryReader(Stream stream_1, GDelegate1 gdelegate1_1, GDelegate2 gdelegate2_1)
    {
        this.stream_0 = stream_1;
        if (stream_1.CanRead)
        {
            this.binaryReader_0 = new System.IO.BinaryReader(stream_1);
        }
        this.gdelegate1_0 = gdelegate1_1;
        this.gdelegate2_0 = gdelegate2_1;
    }

    // Token: 0x0600002E RID: 46 RVA: 0x00002220 File Offset: 0x00000420
    public void method_0()
    {
        this.stream_0.Flush();
    }

    // Token: 0x0600002F RID: 47 RVA: 0x0000222D File Offset: 0x0000042D
    public bool method_1()
    {
        return this.binaryReader_0.ReadBoolean();
    }

    // Token: 0x06000030 RID: 48 RVA: 0x0000223A File Offset: 0x0000043A
    public byte method_2()
    {
        return this.binaryReader_0.ReadByte();
    }

    // Token: 0x06000031 RID: 49 RVA: 0x00002CC0 File Offset: 0x00000EC0
    public byte[] method_3()
    {
        uint count = this.method_16();
        return this.binaryReader_0.ReadBytes((int)count);
    }

    // Token: 0x06000032 RID: 50 RVA: 0x00002247 File Offset: 0x00000447
    public char method_4()
    {
        return this.binaryReader_0.ReadChar();
    }

    // Token: 0x06000033 RID: 51 RVA: 0x00002CE0 File Offset: 0x00000EE0
    public char[] method_5()
    {
        uint count = this.method_16();
        return this.binaryReader_0.ReadChars((int)count);
    }

    // Token: 0x06000034 RID: 52 RVA: 0x00002254 File Offset: 0x00000454
    public decimal method_6()
    {
        return this.binaryReader_0.ReadDecimal();
    }

    // Token: 0x06000035 RID: 53 RVA: 0x00002261 File Offset: 0x00000461
    public double method_7()
    {
        return this.binaryReader_0.ReadDouble();
    }

    // Token: 0x06000036 RID: 54 RVA: 0x0000226E File Offset: 0x0000046E
    public short method_8()
    {
        return this.binaryReader_0.ReadInt16();
    }

    // Token: 0x06000037 RID: 55 RVA: 0x0000227B File Offset: 0x0000047B
    public int method_9()
    {
        return this.binaryReader_0.ReadInt32();
    }

    // Token: 0x06000038 RID: 56 RVA: 0x00002288 File Offset: 0x00000488
    public long method_10()
    {
        return this.binaryReader_0.ReadInt64();
    }

    // Token: 0x06000039 RID: 57 RVA: 0x00002295 File Offset: 0x00000495
    public sbyte method_11()
    {
        return this.binaryReader_0.ReadSByte();
    }

    // Token: 0x0600003A RID: 58 RVA: 0x000022A2 File Offset: 0x000004A2
    public float method_12()
    {
        return this.binaryReader_0.ReadSingle();
    }

    // Token: 0x0600003B RID: 59 RVA: 0x00002D00 File Offset: 0x00000F00
    public float? method_13()
    {
        float? result;
        if (!this.binaryReader_0.ReadBoolean())
        {
            result = new float?(this.binaryReader_0.ReadSingle());
        }
        else
        {
            result = null;
        }
        return result;
    }

    // Token: 0x0600003C RID: 60 RVA: 0x000022AF File Offset: 0x000004AF
    public string method_14()
    {
        return this.binaryReader_0.ReadString();
    }

    // Token: 0x0600003D RID: 61 RVA: 0x000022BC File Offset: 0x000004BC
    public ushort method_15()
    {
        return this.binaryReader_0.ReadUInt16();
    }

    // Token: 0x0600003E RID: 62 RVA: 0x000022C9 File Offset: 0x000004C9
    public uint method_16()
    {
        return this.binaryReader_0.ReadUInt32();
    }

    // Token: 0x0600003F RID: 63 RVA: 0x000022D6 File Offset: 0x000004D6
    public ulong method_17()
    {
        return this.binaryReader_0.ReadUInt64();
    }

    // Token: 0x06000040 RID: 64 RVA: 0x000022E3 File Offset: 0x000004E3
    public Guid method_18()
    {
        return new Guid(this.binaryReader_0.ReadBytes(16));
    }

    // Token: 0x06000041 RID: 65 RVA: 0x000022F7 File Offset: 0x000004F7
    public Vector2 method_19()
    {
        return new Vector2(this.binaryReader_0.ReadSingle(), this.binaryReader_0.ReadSingle());
    }

    // Token: 0x06000042 RID: 66 RVA: 0x00002314 File Offset: 0x00000514
    public Vector3 method_20()
    {
        return new Vector3(this.binaryReader_0.ReadSingle(), this.binaryReader_0.ReadSingle(), this.binaryReader_0.ReadSingle());
    }

    // Token: 0x06000043 RID: 67 RVA: 0x0000233C File Offset: 0x0000053C
    public Vector4 method_21()
    {
        return new Vector4(this.binaryReader_0.ReadSingle(), this.binaryReader_0.ReadSingle(), this.binaryReader_0.ReadSingle(), this.binaryReader_0.ReadSingle());
    }

    // Token: 0x06000044 RID: 68 RVA: 0x00002D38 File Offset: 0x00000F38
    public Matrix4x4 method_22()
    {
        return new Matrix4x4
        {
            M11 = this.binaryReader_0.ReadSingle(),
            M12 = this.binaryReader_0.ReadSingle(),
            M13 = this.binaryReader_0.ReadSingle(),
            M14 = this.binaryReader_0.ReadSingle(),
            M21 = this.binaryReader_0.ReadSingle(),
            M22 = this.binaryReader_0.ReadSingle(),
            M23 = this.binaryReader_0.ReadSingle(),
            M24 = this.binaryReader_0.ReadSingle(),
            M31 = this.binaryReader_0.ReadSingle(),
            M32 = this.binaryReader_0.ReadSingle(),
            M33 = this.binaryReader_0.ReadSingle(),
            M34 = this.binaryReader_0.ReadSingle(),
            M41 = this.binaryReader_0.ReadSingle(),
            M42 = this.binaryReader_0.ReadSingle(),
            M43 = this.binaryReader_0.ReadSingle(),
            M44 = this.binaryReader_0.ReadSingle()
        };
    }

    // Token: 0x06000045 RID: 69 RVA: 0x0000236F File Offset: 0x0000056F
    public Quaternion method_23()
    {
        return new Quaternion(this.binaryReader_0.ReadSingle(), this.binaryReader_0.ReadSingle(), this.binaryReader_0.ReadSingle(), this.binaryReader_0.ReadSingle());
    }

    // Token: 0x06000046 RID: 70 RVA: 0x000023A2 File Offset: 0x000005A2
    public Version method_24()
    {
        return new Version(this.binaryReader_0.ReadInt32(), this.binaryReader_0.ReadInt32(), this.binaryReader_0.ReadInt32(), this.binaryReader_0.ReadInt32());
    }

    // Token: 0x06000047 RID: 71 RVA: 0x00002E70 File Offset: 0x00001070
    public IDeserialize method_25()
    {
        MessageHelper gclass = new MessageHelper(this.stream_0, this.gdelegate1_0, this.gdelegate2_0);
        return gclass.ReadMessage();
    }

    // Token: 0x04000014 RID: 20
    private Stream stream_0;

    // Token: 0x04000015 RID: 21
    private System.IO.BinaryReader binaryReader_0;

    // Token: 0x04000016 RID: 22
    private GDelegate1 gdelegate1_0;

    // Token: 0x04000017 RID: 23
    private GDelegate2 gdelegate2_0;
}
