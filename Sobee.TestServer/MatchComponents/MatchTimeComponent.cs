using Sobee.TestServer.Match;
using Sobee.TestServer.Messages.Match;

namespace Sobee.TestServer.MatchComponents
{
    public class MatchTimeComponent : MatchComponent<MatchRoom>
    {
        public MatchTimeComponent()
        {

        }

        #region Lifecycle
        protected override void OnUpdate(double delta)
        {
            if (Owner == null)
            {
                _log.Warning("Owner is null. Component aborted.");
                Stop();

                return;
            }

            var matchInfo = Owner.MatchInformation;
            var phaseInfo = matchInfo.PhaseInfo;

            if (matchInfo.MatchState == MatchState.Running)
            {
                //if (matchInfo.TimeMultiplier <= 0)
                //{
                //    _log.Warning("{roomId} TimeMultiplier was 0 or negative, resetting to 1.", _matchRoom.Id);
                //    matchInfo.TimeMultiplier = 1;
                //}
                phaseInfo.MatchTime += delta * matchInfo.TimeMultiplier;
            }
        }
        #endregion
    }
}