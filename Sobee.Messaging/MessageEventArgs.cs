namespace Sobee.Messaging
{
    public class MessageEventArgs : EventArgs
    {
        public SocketMessageHandle handler { get; private set; }
        public Message message { get; private set; }

        public MessageEventArgs(SocketMessageHandle gclass306_1, Message gclass175_1)
        {
            handler = gclass306_1;
            message = gclass175_1;
        }
    }
}