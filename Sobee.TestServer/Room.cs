using Serilog;
using Sobee.Common;
using Sobee.Messaging;
using Sobee.Network;

namespace Sobee.TestServer
{
    public class Room<T> : Communication where T : Session
    {
        private static readonly ILogger _log = Logging.Get<Room<T>>();
        private bool _isDisposed;

        protected readonly SessionManager<T> _sessions;

        public Room() : base()
        {
            _log.Debug("{id} initializing.", Id);
            CommunicationType = SessionType.Game;
            _sessions = CreateSessionManager();
            _log.Debug("{id} initialized.", Id);
        }

        protected virtual SessionManager<T> CreateSessionManager()
            => new(this);

        public override async Task Update(double delta)
        {
            await _sessions.Update(delta);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                if (disposing)
                {
                    _log.Debug("{id} disposing.", Id);
                    _sessions.Dispose();
                    _log.Debug("{id} disposed.", Id);
                }
            }

            base.Dispose(disposing);
        }
    }
}
