using Sobee.Common;

namespace Sobee.Serialization
{
    public class MessageSerialization : Disposable
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
            ushort num = messageTypeToId(message.GetType());
            num ^= 7779;
            binaryWriter.method_15(num);
            message.Serialize(binaryWriter);
        }

        public IMessage ReadMessage()
        {
            if (binaryReader == null)
            {
                throw new ArgumentException("Stream was not readable.");
            }
            ushort num = binaryReader.method_15();
            num ^= 7779;
            return (IMessage)dispatchToMessage(num, binaryReader);
        }

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            if (disposing)
            {
                binaryWriter.Dispose();
                binaryReader.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}