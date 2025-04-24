using System.Net.Sockets;
using System.Runtime.Serialization;

namespace Sobee.Messaging
{
    public class SocketMessageHandler : SocketQueueHandler
    {
        private MessageDispatcher? Dispatcher;

        private MessageHelper? MessageHelperUpdate;
        private MemoryStream UpdateMessageReadingStream = new MemoryStream(SocketMessageHandler.MaxMessageSize);

        private MessageHelper? MessageHelperSend;
        private MemoryStream ReadingStreamSend = new MemoryStream(SocketMessageHandler.BufferSize);

        public SocketMessageHandler(Socket Socket) : base(Socket)
        {
            base.BeginReceive();
        }

        public virtual void Update()
        {
            lock (this)
            {
                try
                {
                    // Gelen mesajları işleme
                    ProcessIncomingMessages();

                    // Gönderim kuyruğundaki mesajları gönder
                    if (this.GetSocketAlive())
                    {
                        this.SendAsync();
                    }

                    // Mesajları kuyruklama
                    this.QueueBufferedMessage();
                }
                catch (Exception ex)
                {
                    // Genel hata işleme
                    HandleException(ex);
                }
            }
        }

        private void ProcessIncomingMessages()
        {
            if (!this.GetSocketAlive())
                return;

            byte[]? array;
            while ((array = this.DequeueMessage()) != null)
            {
                try
                {
                    UpdateMessageReadingStream.Position = 0L;
                    UpdateMessageReadingStream.SetLength(0L);
                    UpdateMessageReadingStream.Write(array, 0, array.Length);
                    UpdateMessageReadingStream.Position = 0L;

                    var message = (Message)this.MessageHelperUpdate.ReadMessage();
                    message.int_0 = array.Length;

                    // Mesajı işleme
                    this.vmethod_11(new MessageDelegateArgs(Dispatcher.Owner, new MessageEventArgs(this, message)));
                }
                catch (SerializationException ex)
                {
                    // Serileştirme hatası işleme
                    HandleSerializationException(ex);
                }
                catch (Exception ex)
                {
                    // Diğer hatalar
                    HandleException(ex);
                }
            }
        }

        public virtual void SendMessage(Message message)
        {
            lock (this)
            {
                try
                {
                    this.ReadingStreamSend.Position = 0L;
                    this.ReadingStreamSend.SetLength(0L);
                    this.MessageHelperSend.WriteMessage(message);
                    this.EnqueueMessage(this.ReadingStreamSend.GetBuffer(), 0, (int)this.ReadingStreamSend.Length);
                    message.int_0 = (int)this.ReadingStreamSend.Length;
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

        protected virtual void vmethod_11(MessageDelegateArgs args)
        {
            Dispatcher.DispatchToMessageEvent(args.method_1().method_1().GetType(), args);
        }

        public virtual void SetDispatchSource(MessageDispatcher dispatcher)
        {
            this.Dispatcher = dispatcher;
            this.MessageHelperUpdate = new MessageHelper(UpdateMessageReadingStream, this.Dispatcher.GetDispatcher(), this.Dispatcher.GetMessageTypeToIdDelegate());
            this.MessageHelperSend = new MessageHelper(ReadingStreamSend, this.Dispatcher.GetDispatcher(), this.Dispatcher.GetMessageTypeToIdDelegate());
        }

        private void HandleSerializationException(SerializationException ex)
        {
            // Serileştirme hatası için loglama veya özel işlem
            Console.WriteLine($"Serialization error: {ex.Message}");
        }

        private void HandleException(Exception ex)
        {
            // Genel hata işleme
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
