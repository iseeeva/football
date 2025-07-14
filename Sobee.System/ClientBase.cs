using System.Net.Sockets;
using Serilog;
using Sobee.Common;
using Sobee.Messaging;
using Sobee.Network;

namespace Sobee.System
{
    public class ClientBase : Session
    {
        private readonly ILogger _log = Logging.Get<ClientBase>();
        private bool _isDisposed;

        public readonly SessionQueueHandle queueHandle;
        public readonly SessionMessageHandle messageHandle;

        public ClientBase(Socket socket, SessionType sessionType) : base(socket, sessionType)
        {
            try
            {
                queueHandle = new SessionQueueHandle(this);
                messageHandle = new SessionMessageHandle(queueHandle, this);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error initializing: {ex.Message}", ex);
            }

            _log.Debug("{id} initialized.", Id);
        }

        public override async Task Update()
        {
            await queueHandle.Update();
            await messageHandle.Update();
            await base.Update();
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                if (disposing)
                {
                    _log.Information("{id} disposing.", Id);

                    queueHandle.Dispose();
                    messageHandle.Dispose();

                    _log.Information("{id} disposed.", Id);
                }
            }

            base.Dispose(disposing);
        }
    }
}
