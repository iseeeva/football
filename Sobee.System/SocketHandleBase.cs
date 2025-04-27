using System.Net.Sockets;
using Sobee.Messaging;

namespace Sobee.System
{
    public class SocketHandleBase : SocketMessageHandle
    {
        public Guid Id { get; private set; }

        public SocketHandleBase(Guid Id, Socket Socket) : base(Socket)
        {
            this.Id = Id;
        }

        public override Task Update()
        {
            return base.Update();
        }
    }
}
