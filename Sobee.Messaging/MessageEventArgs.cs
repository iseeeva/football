namespace Sobee.Messaging
{
    public class MessageEventArgs : EventArgs
    {
        public Session handler { get; private set; }
        public Message message { get; private set; }

        public MessageEventArgs(Session gclass306_1, Message gclass175_1)
        {
            handler = gclass306_1;
            message = gclass175_1;
        }
    }
}