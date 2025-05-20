namespace Sobee.Messaging
{
    public class MessageDelegateArgs : EventArgs
    {
        public MessageEventArgs eventArgs { get; private set; }
        public object sender { get; private set; }

        public MessageDelegateArgs(object gclass306_1, MessageEventArgs gclass175_1)
        {
            sender = gclass306_1;
            eventArgs = gclass175_1;
        }
    }
}