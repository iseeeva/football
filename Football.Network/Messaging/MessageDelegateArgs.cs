namespace Football.Network.Messaging
{
    public class MessageDelegateArgs : EventArgs
    {
        public readonly MessageEventArgs EventArgs;
        public readonly object Sender;

        public MessageDelegateArgs(object gclass306_1, MessageEventArgs gclass175_1)
        {
            Sender = gclass306_1;
            EventArgs = gclass175_1;
        }
    }
}