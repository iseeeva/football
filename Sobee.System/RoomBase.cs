using Serilog;
using Sobee.Common;

namespace Sobee.System
{
    public class RoomBase : Component
    {
        private readonly ILogger log = Logging.Get<RoomBase>();

        public Guid Id { get; private set; }

        public RoomBase(Guid Id)
        {
            this.Id = Id;
        }

        public override async Task Update()
        {

        }

        public override void Dispose()
        {
            log.Information("{id} disposing.", Id);
            GC.SuppressFinalize(this);
        }
    }
}
