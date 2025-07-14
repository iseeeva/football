using Serilog;
using Sobee.Common;
using Sobee.Messaging;

namespace Sobee.System.Common
{
    public class Room : RoomBase
    {
        private readonly ILogger _log = Logging.Get<Room>();
        private bool _isDisposed;

        public readonly ClientManager Clients;
        public Messages.Common.Match.Information Information = new Messages.Common.Match.Information();

        public Room(MessageDispatch dispatch) : base()
        {
            Clients = new ClientManager(dispatch);
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

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                if (disposing)
                {
                    _log.Debug("{id} disposing.", this.Id);
                    _log.Debug("{id} disposed.", this.Id);
                }
            }

            base.Dispose(disposing);
        }
    }
}
