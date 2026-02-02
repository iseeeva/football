using System.Diagnostics.CodeAnalysis;
using Sobee.Common;
using Sobee.Network;
using Sobee.TestServer.Auth;
using Sobee.TestServer.MatchHelpers;
using Sobee.TestServer.Messages.Player;

namespace Sobee.TestServer.Match
{
    public class MatchPlayerManager : SessionManager<MatchPlayer>
    {
        private static readonly Serilog.ILogger _log = Logging.Get<MatchPlayerManager>();
        private bool _isDisposed;

        private readonly MatchRoom _matchRoom;
        public event Action<MatchRoom, MatchPlayer>? PlayerJoinEvent;
        public event Action<MatchRoom, MatchPlayer, PlayerMatchInformationMessage>? PlayerLeaveEvent;

        public MatchPlayerManager(MatchRoom matchRoom) : base(matchRoom)
        {
            _matchRoom = matchRoom;
            _log.Information("{managerId} initialized.", Id);
        }

        public override bool TryAdd(MatchPlayer session)
        {
            _log.Warning("TryAdd doesn't implemented, use TryCreate for now.");
            return false;
        }

        public virtual bool TryCreate(AuthUser authUser)
        {
            if (MatchHelper.TryGeneratePlayer(_matchRoom, authUser, out var matchPlayer, out var matchPlayerInfo))
                authUser.Dispose();
            else
                return false;

            var team = _matchRoom.MatchInformation.GetSittingSide(matchPlayerInfo.StadiumSitting);
            if (team == null || team.Any(
                p => p.PlayerId == matchPlayerInfo.PlayerId ||
                p.SquadNumber == matchPlayerInfo.SquadNumber
            ))
                return false;

            if (base.TryAdd(matchPlayer))
            {
                if (!MatchHelper.TryAssignPlayerInfoToMatchInfo(_matchRoom, matchPlayerInfo))
                {
                    if (!TryRemove(matchPlayer.Id, out _))
                    {
                        _log.Error("{managerId}, failed to remove player after failing to assign player info. PlayerId: {playerId}", Id, matchPlayer.Id);
                        _matchRoom.Dispose();
                    }

                    return false;
                }
            }
            else
            {
                matchPlayer.Disconnect();
                return false;
            }

            PlayerJoinEvent?.Invoke(_matchRoom, matchPlayer);
            return true;
        }

        public override bool TryRemove(Guid id, [NotNullWhen(true)] out MatchPlayer? matchPlayer)
        {
            matchPlayer = this[id];

            if (matchPlayer == null)
            {
                _log.Error("{managerId}, player not found for removal. PlayerId: {playerId}", Id, id);
                matchPlayer = null;
                return false;
            }

            var matchPlayerInfo = _matchRoom.MatchInformation.GetPlayer(matchPlayer.Id);
            if (matchPlayerInfo == null)
            {
                _log.Error("{managerId}, player info not found in match info for removal. PlayerId: {playerId}", Id, matchPlayer.Id);
                return false;
            }

            // Once oyuncu infosunu kaldir, sonra oyuncuyu kaldir.
            if (!MatchHelper.TryRemovePlayerInfoFromMatchInfo(_matchRoom, matchPlayerInfo) || !base.TryRemove(matchPlayer.Id, out _))
            {
                _log.Error("{managerId}, failed to remove player. PlayerId: {playerId}", Id, matchPlayer.Id);
                _matchRoom.Dispose();
                return false;
            }

            PlayerLeaveEvent?.Invoke(_matchRoom, matchPlayer, matchPlayerInfo);
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            if (disposing)
            {
                _log.Debug("{id} disposing.", Id);
                PlayerJoinEvent = null;
                PlayerLeaveEvent = null;
                _log.Debug("{id} disposed.", Id);
            }

            base.Dispose(disposing);
        }
    }
}