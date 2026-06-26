namespace Football.Serialization
{
    public class MessageSerialization : IDisposable
    {
        private bool _isDisposed;

        private readonly BinaryWriter binaryWriter;
        private readonly BinaryReader binaryReader;
        private readonly DispatchToMessageDelegate dispatchToMessage;
        private readonly MessageIdFromTypeDelegate messageTypeToId;

        public MessageSerialization(Stream stream_0, DispatchToMessageDelegate dispatchToMessageDelegate, MessageIdFromTypeDelegate messageIdFromTypeDelegate)
        {
            binaryWriter = new BinaryWriter(stream_0, dispatchToMessageDelegate, messageIdFromTypeDelegate);
            binaryReader = new BinaryReader(stream_0, dispatchToMessageDelegate, messageIdFromTypeDelegate);
            dispatchToMessage = dispatchToMessageDelegate;
            messageTypeToId = messageIdFromTypeDelegate;
        }

        public void WriteMessage(IMessage message)
        {
            if (binaryWriter == null || !binaryWriter.CanWrite)
                throw new ArgumentException("BinaryWriter is null or can't writeable.");

            ushort num = messageTypeToId(message.GetType());
            num ^= 7779;
            binaryWriter.method_15(num);
            message.Serialize(binaryWriter);
        }

        public IMessage ReadMessage()
        {
            if (binaryReader == null || !binaryReader.CanRead)
                throw new ArgumentException("BinaryReader is null or can't readable.");

            ushort num = binaryReader.method_15();
            num ^= 7779;
            return (IMessage)dispatchToMessage(num, binaryReader);
        }

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
                binaryWriter.Dispose();
                binaryReader.Dispose();
            }
        }
    }
}