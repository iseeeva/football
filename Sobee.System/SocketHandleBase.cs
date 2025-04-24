using System.Net.Sockets;
using Sobee.Messaging;

namespace Sobee.System
{
    public class SocketHandleBase : SocketMessageHandler
    {
        private readonly Guid Id;

        public SocketHandleBase(Guid Id, Socket Socket) : base(Socket)
        {
            this.Id = Id;
        }

        public Guid GetId() { return Id; }

        public override void Update()
        {
            base.Update();
        }
    }
}
