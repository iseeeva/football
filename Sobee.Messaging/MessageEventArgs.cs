using Sobee.Messaging;

public class MessageEventArgs : System.EventArgs
{
    public SocketMessageHandle handler { get; private set; }
    public Message message { get; private set; }

    public MessageEventArgs(SocketMessageHandle gclass306_1, Message gclass175_1)
    {
        this.handler = gclass306_1;
        this.message = gclass175_1;
    }
}
