using System.Net.Sockets;
using Serilog;
using Sobee.Common;
using Sobee.Messaging;

namespace Sobee.System.Common
{
    public class Client : ClientBase
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

        public virtual void Broadcast(Message message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            try
            {
                SendMessage(message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public override void Dispose()
        {
            log.Information("{id} disposing.", Id);
            GC.SuppressFinalize(this);
            base.Dispose();
        }
    }
}
