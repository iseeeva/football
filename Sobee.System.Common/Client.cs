using System.Net.Sockets;
using Sobee.Messaging;

namespace Sobee.System.Common
{
    public class Client
    {
        private readonly Guid Id;
        private readonly SocketHandle Socket;
        public Messages.Common.Player.Initialize Information;

        public Client(Guid Id, Socket Socket)
        {
            this.Id = Id;
            this.Socket = new SocketHandle(Socket);
        }

        public Guid GetId() { return Id; }

        public void Update()
        {
            this.Socket.Update();
        }

        public void SetDispatchSource(DispatchHelper dispatcher)
        {
            this.Socket.SetDispatchSource(dispatcher);
        }
    }
}
