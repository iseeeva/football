using Serilog;
using Sobee.Common;

namespace Sobee.TestServer.Common
{
    public class RoomBase : Component
    {
        private static readonly ILogger _log = Logging.Get<RoomBase>();
        private bool _isDisposed;

        public RoomBase()
        {
            _log.Debug("{id} initialized.", Id);
        }

        public override async Task Update(double delta)
        {

        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                if (disposing)
                {
                    _log.Information("{id} disposing.", Id);

                    _log.Information("{id} disposed.", Id);
                }
            }

            base.Dispose(disposing);
        }
    }
}
