using System.Net.Sockets;
using System.Runtime.Serialization;

namespace Sobee.Messaging
{
    public class SocketMessageHandler : SocketQueueHandler
    {
        private MessageDispatcher? Dispatcher;
        private MessageHelper? MessageHelperUpdate;
        private MessageHelper? MessageHelperSend;

        private readonly MemoryStream UpdateMessageReadingStream = new MemoryStream(SocketMessageHandler.MaxReceivingSize);
        private readonly MemoryStream ReadingStreamSend = new MemoryStream(SocketMessageHandler.MaxSendingSize);

        public SocketMessageHandler(Socket socket) : base(socket) { }

        public virtual async Task Update()
        {
            if (!this.IsSocketAlive) return;

            try
            {
                await ProcessIncomingMessagesAsync();
                await ProcessSendQueueAsync();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private async Task ProcessIncomingMessagesAsync()
        {
            byte[]? array;
            while ((array = this.DequeueReceiveMessage()) != null)
            {
                try
                {
                    PrepareStream(UpdateMessageReadingStream, array);
                    var message = (Message)this.MessageHelperUpdate.ReadMessage();
                    message.int_0 = array.Length;

                    await DispatchMessageAsync(new MessageDelegateArgs(Dispatcher.Owner, new MessageEventArgs(this, message)));
                }
                catch (SerializationException ex)
                {
                    HandleSerializationException(ex);
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

        public virtual void SendMessage(Message message)
        {
            try
            {
                PrepareStream(ReadingStreamSend);
                this.MessageHelperSend.WriteMessage(message);
                this.EnqueueSendMessage(ReadingStreamSend.GetBuffer());
                message.int_0 = (int)ReadingStreamSend.Length;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private static void PrepareStream(MemoryStream stream, byte[]? data = null)
        {
            stream.Position = 0L;
            stream.SetLength(0L);
            if (data != null)
            {
                stream.Write(data, 0, data.Length);
                stream.Position = 0L;
            }
        }

        protected virtual async Task DispatchMessageAsync(MessageDelegateArgs args)
        {
            await Task.Run(() => Dispatcher.DispatchToMessageEvent(args.method_1().method_1().GetType(), args));
        }

        public virtual void SetDispatchSource(MessageDispatcher dispatcher)
        {
            this.Dispatcher = dispatcher;
            this.MessageHelperUpdate = new MessageHelper(UpdateMessageReadingStream, dispatcher.GetDispatcher(), dispatcher.GetMessageTypeToIdDelegate());
            this.MessageHelperSend = new MessageHelper(ReadingStreamSend, dispatcher.GetDispatcher(), dispatcher.GetMessageTypeToIdDelegate());
        }

        private void HandleSerializationException(SerializationException ex)
        {
            // Loglama
            Console.WriteLine($"Serialization error: {ex.Message}");
        }

        private void HandleException(Exception ex)
        {
            // Loglama
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
