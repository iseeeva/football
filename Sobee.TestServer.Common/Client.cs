using System.Net.Sockets;
using Serilog;
using Sobee.Common;
using Sobee.Messaging;
using Sobee.Network;

namespace Sobee.TestServer.Common
{
    public class Client : ClientBase
    {
        private static readonly ILogger _log = Logging.Get<Client>();
        private bool _isDisposed;

        public Messages.Player.PlayerInformation? Information;

        public Client(Socket socket) : base(socket, SessionType.User)
        {
            _log.Debug("{id} initialized.", Id);
        }

        public override Task Update(double delta)
        {
            return base.Update(delta);
        }

        public virtual void Broadcast(Message message)
        {
            ArgumentNullException.ThrowIfNull(message);

            try
            {
                messageHandle.SendMessage(message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
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
