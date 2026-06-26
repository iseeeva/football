using Football.Network.Messaging;

public class PlayerAppearanceMessage : Message
{
    private const int PartCount = 60;
    public byte[] Parts { get; private set; } = new byte[PartCount];

    public PlayerAppearanceMessage()
    {
        for (int i = 0; i < 45; i++)
            Parts[i] = 128;

        Parts[53] = 44;
        Parts[54] = 79;
        Parts[56] = 128;
        Parts[57] = 128;
    }

    public PlayerAppearanceMessage(BinaryReader reader)
    {
        for (int i = 0; i < PartCount; i++)
        {
            Parts[i] = reader.method_2();
        }
    }

    public override void Serialize(BinaryWriter writer)
    {
        foreach (var part in Parts)
        {
            writer.method_2(part);
        }
    }

    public void CopyFrom(PlayerAppearanceMessage other)
    {
        ArgumentNullException.ThrowIfNull(other);
        Array.Copy(other.Parts, this.Parts, PartCount);
    }
}