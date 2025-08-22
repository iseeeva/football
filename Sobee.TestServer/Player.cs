using Serilog;
using Sobee.Common;
using Sobee.Messaging;
using Sobee.Network;
using Sobee.TestServer.Common;

namespace Sobee.TestServer
{
    public class Player : Client
    {
        private static readonly ILogger _log = Logging.Get<Player>();
        private bool _isDisposed;

        public Messages.Player.PlayerInformation? Information;

        public Player(SocketWrapper socket, RoomCommunication playerComm) : base(socket, playerComm)
        {
            playerComm.SubscribePlayerInformation(new EventHandler<MessageEventArgs>(GameEvents.PlayerEvent.Information));
            _log.Debug("{id} initialized.", Id);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                if (disposing)
                {
                    _log.Debug("{id} disposing.", Id);

                    Information = null;

                    _log.Debug("{id} disposed.", Id);
                }
            }

            base.Dispose(disposing);
        }
    }
}
