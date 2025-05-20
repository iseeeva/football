using Serilog;
using Sobee.Common;
using Sobee.Messaging;

namespace Sobee.System.Common
{
    public class Room : RoomBase
    {
        private readonly ILogger log = Logging.Get<Room>();

        public readonly ClientManager Clients;
        public Messages.Common.Match.Information Information = new Messages.Common.Match.Information();

        public Room(Guid Id, MessageDispatch Dispatch) : base(Id)
        {
            Clients = new ClientManager(Dispatch);
        }

        public void Broadcast(Message message)
        {
            Clients.Broadcast(message);
        }

        public override async Task Update()
        {
            if (Clients != null)
            {
                await Clients.Update();
            }

            await base.Update();
        }

        public override void Dispose()
        {
            log.Information("{id} disposing.", Id);
            GC.SuppressFinalize(this);
            base.Dispose();
        }
    }
}
