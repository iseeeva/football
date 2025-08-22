using Serilog;
using Sobee.Common;
using Sobee.Messaging;
using Sobee.Network;
using Sobee.TestServer.Common;

namespace Sobee.TestServer
{
    public class Room : Component
    {
        private static readonly ILogger _log = Logging.Get<Room>();
        private bool _isDisposed;

        private RoomCommunication Communication { get; set; } = new RoomCommunication();

        public readonly PlayerManager Players;
        public Messages.Match.MatchInformation Information = new Messages.Match.MatchInformation();

        public readonly static int MAX_IDLE_TIME = 60 * 1000;

        public Room() : base()
        {
            Communication.SessionType = SessionType.Game;
            Players = new PlayerManager(Communication);
            _log.Debug("{id} initialized.", Id);
        }

        public void Broadcast(Message message)
        {
            Players.Broadcast(message);
        }

        public override async Task Update(double delta)
        {
            if (Players != null)
            {
                await Players.Update(delta);
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
                    Players.Dispose();
                    _log.Debug("{id} disposed.", Id);
                }
            }

            base.Dispose(disposing);
        }
    }
}
