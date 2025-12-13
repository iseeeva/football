// Token: 0x0200000B RID: 11
using System.Numerics;
using Sobee.Common;
using Sobee.Serialization;

public class BinaryReader : Disposable
{
    private bool _isDisposed;

    private readonly Stream _stream;
    private readonly System.IO.BinaryReader _binaryReader;
    private readonly DispatchToMessageDelegate _dispatchToMessage;
    private readonly MessageIdFromTypeDelegate _messageIdFromType;

    public BinaryReader(Stream stream, DispatchToMessageDelegate dispatchToMessageDelegate, MessageIdFromTypeDelegate messageIdFromTypeDelegate)
    {
        _stream = stream;

        if (_stream.CanWrite)
            _binaryReader = new System.IO.BinaryReader(_stream);
        else
            throw new ArgumentException("Stream was not writable.");

        _dispatchToMessage = dispatchToMessageDelegate;
        _messageIdFromType = messageIdFromTypeDelegate;
    }



    // Token: 0x0600002F RID: 47 RVA: 0x0000222D File Offset: 0x0000042D
    public bool method_1()
    {
        return this._binaryReader.ReadBoolean();
    }

    // Token: 0x06000030 RID: 48 RVA: 0x0000223A File Offset: 0x0000043A
    public byte method_2()
    {
        return this._binaryReader.ReadByte();
    }

    // Token: 0x06000031 RID: 49 RVA: 0x00002CC0 File Offset: 0x00000EC0
    public byte[] method_3()
    {
        uint count = this.method_16();
        return this._binaryReader.ReadBytes((int)count);
    }

    // Token: 0x06000032 RID: 50 RVA: 0x00002247 File Offset: 0x00000447
    public char method_4()
    {
        return this._binaryReader.ReadChar();
    }

    // Token: 0x06000033 RID: 51 RVA: 0x00002CE0 File Offset: 0x00000EE0
    public char[] method_5()
    {
        uint count = this.method_16();
        return this._binaryReader.ReadChars((int)count);
    }

    // Token: 0x06000034 RID: 52 RVA: 0x00002254 File Offset: 0x00000454
    public decimal method_6()
    {
        return this._binaryReader.ReadDecimal();
    }

    // Token: 0x06000035 RID: 53 RVA: 0x00002261 File Offset: 0x00000461
    public double method_7()
    {
        return this._binaryReader.ReadDouble();
    }

    // Token: 0x06000036 RID: 54 RVA: 0x0000226E File Offset: 0x0000046E
    public short method_8()
    {
        return this._binaryReader.ReadInt16();
    }

    // Token: 0x06000037 RID: 55 RVA: 0x0000227B File Offset: 0x0000047B
    public int method_9()
    {
        return this._binaryReader.ReadInt32();
    }

    // Token: 0x06000038 RID: 56 RVA: 0x00002288 File Offset: 0x00000488
    public long method_10()
    {
        return this._binaryReader.ReadInt64();
    }

    // Token: 0x06000039 RID: 57 RVA: 0x00002295 File Offset: 0x00000495
    public sbyte method_11()
    {
        return this._binaryReader.ReadSByte();
    }

    // Token: 0x0600003A RID: 58 RVA: 0x000022A2 File Offset: 0x000004A2
    public float method_12()
    {
        return this._binaryReader.ReadSingle();
    }

    // Token: 0x0600003B RID: 59 RVA: 0x00002D00 File Offset: 0x00000F00
    public float? method_13()
    {
        float? result;
        if (!this._binaryReader.ReadBoolean())
        {
            result = new float?(this._binaryReader.ReadSingle());
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
        return this._binaryReader.ReadString();
    }

    // Token: 0x0600003D RID: 61 RVA: 0x000022BC File Offset: 0x000004BC
    public ushort method_15()
    {
        return this._binaryReader.ReadUInt16();
    }

    // Token: 0x0600003E RID: 62 RVA: 0x000022C9 File Offset: 0x000004C9
    public uint method_16()
    {
        return this._binaryReader.ReadUInt32();
    }

    // Token: 0x0600003F RID: 63 RVA: 0x000022D6 File Offset: 0x000004D6
    public ulong method_17()
    {
        return this._binaryReader.ReadUInt64();
    }

    // Token: 0x06000040 RID: 64 RVA: 0x000022E3 File Offset: 0x000004E3
    public Guid method_18()
    {
        return new Guid(this._binaryReader.ReadBytes(16));
    }

    // Token: 0x06000041 RID: 65 RVA: 0x000022F7 File Offset: 0x000004F7
    public Vector2 method_19()
    {
        return new Vector2(this._binaryReader.ReadSingle(), this._binaryReader.ReadSingle());
    }

    // Token: 0x06000042 RID: 66 RVA: 0x00002314 File Offset: 0x00000514
    public Vector3 method_20()
    {
        return new Vector3(this._binaryReader.ReadSingle(), this._binaryReader.ReadSingle(), this._binaryReader.ReadSingle());
    }

    // Token: 0x06000043 RID: 67 RVA: 0x0000233C File Offset: 0x0000053C
    public Vector4 method_21()
    {
        return new Vector4(this._binaryReader.ReadSingle(), this._binaryReader.ReadSingle(), this._binaryReader.ReadSingle(), this._binaryReader.ReadSingle());
    }

    // Token: 0x06000044 RID: 68 RVA: 0x00002D38 File Offset: 0x00000F38
    public Matrix4x4 method_22()
    {
        return new Matrix4x4
        {
            M11 = this._binaryReader.ReadSingle(),
            M12 = this._binaryReader.ReadSingle(),
            M13 = this._binaryReader.ReadSingle(),
            M14 = this._binaryReader.ReadSingle(),
            M21 = this._binaryReader.ReadSingle(),
            M22 = this._binaryReader.ReadSingle(),
            M23 = this._binaryReader.ReadSingle(),
            M24 = this._binaryReader.ReadSingle(),
            M31 = this._binaryReader.ReadSingle(),
            M32 = this._binaryReader.ReadSingle(),
            M33 = this._binaryReader.ReadSingle(),
            M34 = this._binaryReader.ReadSingle(),
            M41 = this._binaryReader.ReadSingle(),
            M42 = this._binaryReader.ReadSingle(),
            M43 = this._binaryReader.ReadSingle(),
            M44 = this._binaryReader.ReadSingle()
        };
    }

    // Token: 0x06000045 RID: 69 RVA: 0x0000236F File Offset: 0x0000056F
    public Quaternion method_23()
    {
        return new Quaternion(this._binaryReader.ReadSingle(), this._binaryReader.ReadSingle(), this._binaryReader.ReadSingle(), this._binaryReader.ReadSingle());
    }

    // Token: 0x06000046 RID: 70 RVA: 0x000023A2 File Offset: 0x000005A2
    public Version method_24()
    {
        return new Version(this._binaryReader.ReadInt32(), this._binaryReader.ReadInt32(), this._binaryReader.ReadInt32(), this._binaryReader.ReadInt32());
    }

    // Token: 0x06000047 RID: 71 RVA: 0x00002E70 File Offset: 0x00001070
    public ISerialize method_25()
    {
        MessageSerialization gclass = new MessageSerialization(_stream, _dispatchToMessage, _messageIdFromType);
        return gclass.ReadMessage();
    }

    protected override void Dispose(bool disposing)
    {
        if (_isDisposed)
            return;

        _isDisposed = true;

        if (disposing)
        {
            _binaryReader.Dispose();
        }

        base.Dispose(disposing);
    }
}
