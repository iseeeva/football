using System.Net.Sockets;
using Serilog;
using Sobee.Common;

namespace Sobee.System.Common
{
    public class Client : SocketHandleBase
    {
        private readonly ILogger log = Logging.Get<Client>();

        public Messages.Common.Player.Information? Information;

        public Client(Guid Id, Socket Socket) : base(Id, Socket)
        {

        }

        public override Task Update()
        {
            return base.Update();
        }

        public override void Dispose()
        {
            log.Information("{ClientId} disposed.", Id);
            GC.SuppressFinalize(this);
            base.Dispose();
        }
    }
}
