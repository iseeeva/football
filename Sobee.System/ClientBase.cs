using System.Net.Sockets;
using Serilog;
using Sobee.Common;

namespace Sobee.System
{
    public class ClientBase : SocketHandleBase
    {
        private readonly ILogger log = Logging.Get<ClientBase>();

        public Guid Id { get; private set; }

        public ClientBase(Guid Id, Socket Socket) : base(Socket)
        {
            this.Id = Id;
        }

        public override Task Update()
        {
            return base.Update();
        }

        public override void Dispose()
        {
            log.Information("{id} disposing.", Id);
            GC.SuppressFinalize(this);
            base.Dispose();
        }
    }
}
