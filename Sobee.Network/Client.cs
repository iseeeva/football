using System.Net.Sockets;

namespace Sobee.Network
{
    public class Client
    {
        private readonly SocketHandle Socket;

        public Client(Socket Socket)
        {
            this.Socket = new GClass300(Socket);
        }
    }
}
