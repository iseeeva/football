using System.Net.Sockets;
using Serilog;
using Sobee.Common;

namespace Sobee.Messaging
{
    public class SocketMessageHandle : SocketQueueHandle
    {
        private readonly ILogger log = Logging.Get<SocketMessageHandle>();
        private MessageDispatch? _dispatcher;

        private MessageHelper? _receiveHelper;
        private readonly MemoryStream _receiveStream = new MemoryStream(SocketQueueHandle.MaxReceivingSize);

        private MessageHelper? _sendHelper;
        private readonly MemoryStream _sendStream = new MemoryStream(SocketQueueHandle.MaxSendingSize);

        public SocketMessageHandle(Socket socket) : base(socket)
        {
            log.Debug("{id} initialized.", socket.RemoteEndPoint);
        }

        public override async Task Update()
        {
            if (!IsConnected) return;

            try
            {
                await base.Update();
                ProcessIncomingMessages();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public virtual void SendMessage(Message message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            try
            {
                if (_sendHelper == null)
                    throw new InvalidOperationException($"{nameof(_sendHelper)} is not initialized.");

                PrepareStream(_sendStream);
                _sendHelper.WriteMessage(message);
                this.EnqueueSendData(_sendStream.GetBuffer());
                message.byteLength = (int)_sendStream.Length;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public virtual void SetDispatchSource(MessageDispatch dispatcher)
        {
            _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
            _receiveHelper = new MessageHelper(_receiveStream, dispatcher.GetDispatcher(), dispatcher.GetMessageTypeToIdDelegate());
            _sendHelper = new MessageHelper(_sendStream, dispatcher.GetDispatcher(), dispatcher.GetMessageTypeToIdDelegate());
        }

        private void ProcessIncomingMessages()
        {
            byte[]? array;
            while ((array = this.DequeueReceiveData()) != null)
            {
                try
                {
                    if (_receiveHelper == null)
                        throw new InvalidOperationException($"{nameof(_receiveHelper)} is not initialized.");

                    PrepareStream(_receiveStream, array);
                    var message = (Message)_receiveHelper.ReadMessage();
                    message.byteLength = array.Length;

                    if (_dispatcher == null)
                        throw new InvalidOperationException($"{nameof(_dispatcher)} is not initialized.");

                    _dispatcher.DispatchToMessageEvent(new MessageDelegateArgs(_dispatcher.owner, new MessageEventArgs(this, message)));
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
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

        public override void Dispose()
        {
            log.Debug("{id} disposing.", Socket.RemoteEndPoint);

            _receiveStream.Dispose();
            _sendStream.Dispose();
            GC.SuppressFinalize(this);
            base.Dispose();
        }
    }
}
