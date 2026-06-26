namespace Football.Network.Messaging
{
    public class MessageEventArgs : EventArgs
    {
        public readonly Session Handler;
        public readonly Message Message;

        public MessageEventArgs(Session handler, Message message)
        {
            Handler = handler;
            Message = message;
        }
    }
}