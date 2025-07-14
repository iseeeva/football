using System.Net.Sockets;
using Serilog;
using Sobee.Common;
using Sobee.Messaging;

namespace Sobee.System.Common
{
    public class Client : ClientBase
    {
        private readonly ILogger _log = Logging.Get<Client>();
        private bool _isDisposed;

        public Messages.Common.Player.Information? Information;

        public Client(Socket socket, SessionType sessionType) : base(socket, sessionType)
        {

        }

        public override Task Update()
        {
            return base.Update();
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

                    this.Information = null;

                    _log.Debug("{id} disposed.", Id);
                }
            }

            base.Dispose(disposing);
        }
    }
}
