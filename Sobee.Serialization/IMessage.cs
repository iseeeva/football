namespace Sobee.Serialization
{
    public interface IMessage : ISerialize
    {
        new void Serialize(BinaryWriter writer);
    }
}
