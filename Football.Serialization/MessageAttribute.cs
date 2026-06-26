namespace Football.Serialization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public sealed class MessageAttribute : Attribute
    {
        public readonly ushort MessageId;

        public MessageAttribute(ushort messageId)
        {
            MessageId = messageId;
        }
    }
}
