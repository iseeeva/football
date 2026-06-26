using Football.Serialization;
using System.Numerics;

public class BinaryReader : IDisposable
{
    private bool _isDisposed;

    private readonly Stream _stream;
    private readonly System.IO.BinaryReader _binaryReader;
    private readonly DispatchToMessageDelegate _dispatchToMessage;
    private readonly MessageIdFromTypeDelegate _messageIdFromType;

    public bool CanRead
        => _stream.CanRead;

    public BinaryReader(Stream stream, DispatchToMessageDelegate dispatchToMessageDelegate, MessageIdFromTypeDelegate messageIdFromTypeDelegate)
    {
        _stream = stream;
        if (_stream.CanRead)
            _binaryReader = new System.IO.BinaryReader(_stream);
        else
            throw new ArgumentException("Stream was not readable.");

        _dispatchToMessage = dispatchToMessageDelegate;
        _messageIdFromType = messageIdFromTypeDelegate;
    }

    public bool method_1()
    {
        return _binaryReader.ReadBoolean();
    }

    public byte method_2()
    {
        return _binaryReader.ReadByte();
    }

    public byte[] method_3()
    {
        uint count = method_16();
        return _binaryReader.ReadBytes((int)count);
    }

    public char method_4()
    {
        return _binaryReader.ReadChar();
    }

    public char[] method_5()
    {
        uint count = method_16();
        return _binaryReader.ReadChars((int)count);
    }

    public decimal method_6()
    {
        return _binaryReader.ReadDecimal();
    }

    public double method_7()
    {
        return _binaryReader.ReadDouble();
    }

    public short method_8()
    {
        return _binaryReader.ReadInt16();
    }

    public int method_9()
    {
        return _binaryReader.ReadInt32();
    }

    public long method_10()
    {
        return _binaryReader.ReadInt64();
    }

    public sbyte method_11()
    {
        return _binaryReader.ReadSByte();
    }

    public float method_12()
    {
        return _binaryReader.ReadSingle();
    }

    public float? method_13()
    {
        float? result;
        if (!_binaryReader.ReadBoolean())
        {
            result = new float?(_binaryReader.ReadSingle());
        }
        else
        {
            result = null;
        }
        return result;
    }

    public string method_14()
    {
        return _binaryReader.ReadString();
    }

    public ushort method_15()
    {
        return _binaryReader.ReadUInt16();
    }

    public uint method_16()
    {
        return _binaryReader.ReadUInt32();
    }

    public ulong method_17()
    {
        return _binaryReader.ReadUInt64();
    }

    public Guid method_18()
    {
        return new Guid(_binaryReader.ReadBytes(16));
    }

    public Vector2 method_19()
    {
        return new Vector2(_binaryReader.ReadSingle(), _binaryReader.ReadSingle());
    }

    public Vector3 method_20()
    {
        return new Vector3(_binaryReader.ReadSingle(), _binaryReader.ReadSingle(), _binaryReader.ReadSingle());
    }

    public Vector4 method_21()
    {
        return new Vector4(_binaryReader.ReadSingle(), _binaryReader.ReadSingle(), _binaryReader.ReadSingle(), _binaryReader.ReadSingle());
    }

    public Matrix4x4 method_22()
    {
        return new Matrix4x4
        {
            M11 = _binaryReader.ReadSingle(),
            M12 = _binaryReader.ReadSingle(),
            M13 = _binaryReader.ReadSingle(),
            M14 = _binaryReader.ReadSingle(),
            M21 = _binaryReader.ReadSingle(),
            M22 = _binaryReader.ReadSingle(),
            M23 = _binaryReader.ReadSingle(),
            M24 = _binaryReader.ReadSingle(),
            M31 = _binaryReader.ReadSingle(),
            M32 = _binaryReader.ReadSingle(),
            M33 = _binaryReader.ReadSingle(),
            M34 = _binaryReader.ReadSingle(),
            M41 = _binaryReader.ReadSingle(),
            M42 = _binaryReader.ReadSingle(),
            M43 = _binaryReader.ReadSingle(),
            M44 = _binaryReader.ReadSingle()
        };
    }

    public Quaternion method_23()
    {
        return new Quaternion(_binaryReader.ReadSingle(), _binaryReader.ReadSingle(), _binaryReader.ReadSingle(), _binaryReader.ReadSingle());
    }

    public Version method_24()
    {
        return new Version(_binaryReader.ReadInt32(), _binaryReader.ReadInt32(), _binaryReader.ReadInt32(), _binaryReader.ReadInt32());
    }

    public ISerialize method_25()
    {
        MessageSerialization gclass = new MessageSerialization(_stream, _dispatchToMessage, _messageIdFromType);
        return gclass.ReadMessage();
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
            _binaryReader.Dispose();
        }
    }
    #endregion
}
