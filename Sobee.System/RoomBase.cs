using Serilog;
using Sobee.Common;

namespace Sobee.System
{
    public class RoomBase : Component
    {
        private readonly ILogger log = Logging.Get<RoomBase>();
        private bool _isDisposed;

        public RoomBase()
        {

        }

        public override async Task Update()
        {

        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                if (disposing)
                {
                    log.Information("{id} disposing.", this.Id);

                    log.Information("{id} disposed.", this.Id);
                }
            }

            base.Dispose(disposing);
        }
    }
}
