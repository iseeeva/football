using System.Net.Sockets;

namespace Sobee.System.Common
{
    public class Client : SocketHandleBase
    {
        public Messages.Common.Player.Information Information;

        public Client(Guid Id, Socket Socket) : base(Id, Socket)
        {

        }

        public override Task Update()
        {
            return base.Update();
        }
    }
}
