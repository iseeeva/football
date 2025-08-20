using Serilog;
using Sobee.Common;
using Sobee.Messaging;

namespace Sobee.TestServer.Common
{
    public class Room : RoomBase
    {
        private static readonly ILogger _log = Logging.Get<Room>();
        private bool _isDisposed;

        public readonly ClientManager Clients;
        public Messages.Match.MatchInformation Information = new Messages.Match.MatchInformation();

        public readonly static int MAX_IDLE_TIME = 60 * 1000;

        public Room(MessageDispatch dispatch) : base()
        {
            Clients = new ClientManager(dispatch);
            _log.Debug("{id} initialized.", Id);
        }

        public void Broadcast(Message message)
        {
            Clients.Broadcast(message);
        }

        public override async Task Update(double delta)
        {
            if (Clients != null)
            {
                await Clients.Update(delta);
            }

            await base.Update(delta);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                if (disposing)
                {
                    _log.Debug("{id} disposing.", Id);
                    Clients.Dispose();
                    _log.Debug("{id} disposed.", Id);
                }
            }

            base.Dispose(disposing);
        }
    }
}
