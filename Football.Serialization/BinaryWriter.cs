using Football.Serialization;
using System.Numerics;

public class BinaryWriter : IDisposable
{
    private bool _isDisposed;

    private readonly Stream _stream;
    private readonly System.IO.BinaryWriter _binaryWriter;
    private readonly DispatchToMessageDelegate _dispatchToMessage;
    private readonly MessageIdFromTypeDelegate _messageIdFromType;

    public bool CanWrite
        => _stream.CanWrite;

    public BinaryWriter(Stream stream, DispatchToMessageDelegate dispatchToMessageDelegate, MessageIdFromTypeDelegate messageIdFromTypeDelegate)
    {
        _stream = stream;
        if (_stream.CanWrite)
            _binaryWriter = new System.IO.BinaryWriter(_stream);
        else
            throw new ArgumentException("Stream was not writable.");

        _dispatchToMessage = dispatchToMessageDelegate;
        _messageIdFromType = messageIdFromTypeDelegate;
    }

    // Token: 0x0600004A RID: 74 RVA: 0x00002413 File Offset: 0x00000613
    public void method_1(bool bool_0)
    {
        _binaryWriter.Write(bool_0);
    }

    // Token: 0x0600004B RID: 75 RVA: 0x00002421 File Offset: 0x00000621
    public void method_2(byte byte_0)
    {
        _binaryWriter.Write(byte_0);
    }

    // Token: 0x0600004C RID: 76 RVA: 0x00002E9C File Offset: 0x0000109C
    public void method_3(byte[] byte_0)
    {
        uint uint_ = (uint)byte_0.Length;
        method_16(uint_);
        _binaryWriter.Write(byte_0);
    }

    // Token: 0x0600004D RID: 77 RVA: 0x0000242F File Offset: 0x0000062F
    public void method_4(char char_0)
    {
        _binaryWriter.Write(char_0);
    }

    // Token: 0x0600004E RID: 78 RVA: 0x00002EC0 File Offset: 0x000010C0
    public void method_5(char[] char_0)
    {
        uint uint_ = (uint)char_0.Length;
        method_16(uint_);
        _binaryWriter.Write(char_0);
    }

    // Token: 0x0600004F RID: 79 RVA: 0x0000243D File Offset: 0x0000063D
    public void method_6(decimal decimal_0)
    {
        _binaryWriter.Write(decimal_0);
    }

    // Token: 0x06000050 RID: 80 RVA: 0x0000244B File Offset: 0x0000064B
    public void method_7(double double_0)
    {
        _binaryWriter.Write(double_0);
    }

    // Token: 0x06000051 RID: 81 RVA: 0x00002459 File Offset: 0x00000659
    public void method_8(short short_0)
    {
        _binaryWriter.Write(short_0);
    }

    // Token: 0x06000052 RID: 82 RVA: 0x00002467 File Offset: 0x00000667
    public void method_9(int int_0)
    {
        _binaryWriter.Write(int_0);
    }

    // Token: 0x06000053 RID: 83 RVA: 0x00002475 File Offset: 0x00000675
    public void method_10(long long_0)
    {
        _binaryWriter.Write(long_0);
    }

    // Token: 0x06000054 RID: 84 RVA: 0x00002483 File Offset: 0x00000683
    public void method_11(sbyte sbyte_0)
    {
        _binaryWriter.Write(sbyte_0);
    }

    // Token: 0x06000055 RID: 85 RVA: 0x00002491 File Offset: 0x00000691
    public void method_12(float float_0)
    {
        _binaryWriter.Write(float_0);
    }

    // Token: 0x06000056 RID: 86 RVA: 0x0000249F File Offset: 0x0000069F
    public void method_13(float? nullable_0)
    {
        _binaryWriter.Write(nullable_0 != null);
        if (nullable_0 != null)
        {
            _binaryWriter.Write(nullable_0.Value);
        }
    }

    // Token: 0x06000057 RID: 87 RVA: 0x000024CE File Offset: 0x000006CE
    public void method_14(string string_0)
    {
        _binaryWriter.Write(string_0 == null ? string.Empty : string_0);
    }

    // Token: 0x06000058 RID: 88 RVA: 0x000024E6 File Offset: 0x000006E6
    public void method_15(ushort ushort_0)
    {
        _binaryWriter.Write(ushort_0);
    }

    // Token: 0x06000059 RID: 89 RVA: 0x000024F4 File Offset: 0x000006F4
    public void method_16(uint uint_0)
    {
        _binaryWriter.Write(uint_0);
    }

    // Token: 0x0600005A RID: 90 RVA: 0x00002502 File Offset: 0x00000702
    public void method_17(ulong ulong_0)
    {
        _binaryWriter.Write(ulong_0);
    }

    // Token: 0x0600005B RID: 91 RVA: 0x00002510 File Offset: 0x00000710
    public void method_18(Guid guid_0)
    {
        _binaryWriter.Write(guid_0.ToByteArray());
    }

    // Token: 0x0600005C RID: 92 RVA: 0x00002524 File Offset: 0x00000724
    public void method_19(Vector2 vector2_0)
    {
        _binaryWriter.Write(vector2_0.X);
        _binaryWriter.Write(vector2_0.Y);
    }

    // Token: 0x0600005D RID: 93 RVA: 0x0000254A File Offset: 0x0000074A
    public void method_20(Vector3 vector3_0)
    {
        _binaryWriter.Write(vector3_0.X);
        _binaryWriter.Write(vector3_0.Y);
        _binaryWriter.Write(vector3_0.Z);
    }

    // Token: 0x0600005E RID: 94 RVA: 0x00002EE4 File Offset: 0x000010E4
    public void method_21(Vector4 vector4_0)
    {
        _binaryWriter.Write(vector4_0.X);
        _binaryWriter.Write(vector4_0.Y);
        _binaryWriter.Write(vector4_0.Z);
        _binaryWriter.Write(vector4_0.W);
    }

    // Token: 0x0600005F RID: 95 RVA: 0x00002F3C File Offset: 0x0000113C
    public void method_22(Matrix4x4 matrix_0)
    {
        _binaryWriter.Write(matrix_0.M11);
        _binaryWriter.Write(matrix_0.M12);
        _binaryWriter.Write(matrix_0.M13);
        _binaryWriter.Write(matrix_0.M14);
        _binaryWriter.Write(matrix_0.M21);
        _binaryWriter.Write(matrix_0.M22);
        _binaryWriter.Write(matrix_0.M23);
        _binaryWriter.Write(matrix_0.M24);
        _binaryWriter.Write(matrix_0.M31);
        _binaryWriter.Write(matrix_0.M32);
        _binaryWriter.Write(matrix_0.M33);
        _binaryWriter.Write(matrix_0.M34);
        _binaryWriter.Write(matrix_0.M41);
        _binaryWriter.Write(matrix_0.M42);
        _binaryWriter.Write(matrix_0.M43);
        _binaryWriter.Write(matrix_0.M44);
    }

    // Token: 0x06000060 RID: 96 RVA: 0x0000306C File Offset: 0x0000126C
    public void method_23(Version version_0)
    {
        _binaryWriter.Write(version_0.Major);
        _binaryWriter.Write(version_0.Minor);
        _binaryWriter.Write(version_0.Build);
        _binaryWriter.Write(version_0.Revision);
    }

    // Token: 0x06000061 RID: 97 RVA: 0x000030C0 File Offset: 0x000012C0
    public void method_24(Quaternion quaternion_0)
    {
        _binaryWriter.Write(quaternion_0.X);
        _binaryWriter.Write(quaternion_0.Y);
        _binaryWriter.Write(quaternion_0.Z);
        _binaryWriter.Write(quaternion_0.W);
    }

    // Token: 0x06000062 RID: 98 RVA: 0x00003118 File Offset: 0x00001318
    public void method_25(IMessage ginterface7_0)
    {
        MessageSerialization gclass = new MessageSerialization(_stream, _dispatchToMessage, _messageIdFromType);
        gclass.WriteMessage(ginterface7_0);
    }

    #region Dispose
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
            return;

        _isDisposed = true;

        if (disposing)
        {
            _binaryWriter.Dispose();
        }
    }
    #endregion
}