using System.Net.Sockets;
using Serilog;
using Sobee.Common;
using Sobee.Messaging;
using Sobee.Network;

namespace Sobee.TestServer.Common
{
    public class ClientBase : Session
    {
        private static readonly ILogger _log = Logging.Get<ClientBase>();
        private bool _isDisposed;

        public readonly SessionMessageHandle messageHandle;

        public ClientBase(Socket socket, SessionType sessionType) : base(socket, sessionType)
        {
            try
            {
                messageHandle = new SessionMessageHandle(this);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error initializing: {ex.Message}", ex);
            }

            _log.Debug("{id} initialized.", Id);
        }

        public override async Task Update(double delta)
        {
            await messageHandle.Update(delta);
            await base.Update(delta);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                if (disposing)
                {
                    _log.Information("{id} disposing.", Id);

                    messageHandle.Dispose();

                    _log.Information("{id} disposed.", Id);
                }
            }

            base.Dispose(disposing);
        }
    }
}
