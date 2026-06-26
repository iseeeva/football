namespace Football.Serialization
{
    public interface IMessage : ISerialize
    {
        new void Serialize(BinaryWriter writer);
    }
}
