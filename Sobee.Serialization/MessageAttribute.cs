namespace Sobee.Serialization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public sealed class MessageAttribute : Attribute
    {
        public ushort MessageId { get; private set; }

        public MessageAttribute(ushort messageId)
        {
            MessageId = messageId;
        }

    }
}
