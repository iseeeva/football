using Sobee.Common;

namespace Sobee.TestServer.Match
{
    public class MatchComponent : Component
    {
        private readonly Serilog.ILogger _log = Logging.Get<MatchComponent>();
        private bool _isDisposed;

        protected readonly MatchRoom _matchRoom;

        public MatchComponent(MatchRoom matchRoom) : base()
        {
            _matchRoom = matchRoom;
            _log.Debug("{id} initialized.", Id);
        }

        public override Task Update(double delta)
        {
            return Task.CompletedTask;
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                if (disposing)
                {
                    _log.Information("{id} disposed.", Id);
                }
            }
            base.Dispose(disposing);
        }
    }
}
