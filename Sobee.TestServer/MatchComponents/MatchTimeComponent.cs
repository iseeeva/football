using Sobee.Common;
using Sobee.TestServer.Match;
using Sobee.TestServer.Messages.Match;

namespace Sobee.TestServer.MatchComponents
{
    public class MatchTimeComponent : MatchComponent
    {
        private readonly Serilog.ILogger _log = Logging.Get<MatchTimeComponent>();
        private bool _isDisposed;

        public MatchTimeComponent(MatchRoom room) : base(room)
        {
            _log.Debug("{id} initialized.", Id);
        }

        public override void Update(double delta)
        {
            var matchInfo = _matchRoom.MatchInformation;
            var phaseInfo = matchInfo.PhaseInfo;

            if (matchInfo.MatchState == MatchStateType.Running)
            {
                //if (matchInfo.TimeMultiplier <= 0)
                //{
                //    _log.Warning("{roomId} TimeMultiplier was 0 or negative, resetting to 1 for avoiding errors.", _matchRoom.Id);
                //    matchInfo.TimeMultiplier = 1;
                //}

                // Update match time
                phaseInfo.MatchTime += (delta * matchInfo.TimeMultiplier);
            }

            base.Update(delta);
        }

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            if (disposing)
            {
                _log.Debug("{id} disposed.", Id);
            }

            base.Dispose(disposing);
        }
    }
}
