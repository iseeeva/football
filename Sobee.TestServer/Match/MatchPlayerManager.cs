using System.Diagnostics.CodeAnalysis;
using Sobee.Common;
using Sobee.TestServer.Auth;
using Sobee.TestServer.Helpers;

namespace Sobee.TestServer.Match
{
    public class MatchPlayerManager : SessionManager<MatchPlayer>
    {
        private static readonly Serilog.ILogger _log = Logging.Get<MatchPlayerManager>();

        // TODO: Odadan cikan oyuncularin kontrol edilmesi lazim.
        // Socket cokmesi, timeout olmasi vs. icin

        private readonly MatchRoom _matchRoom;

        public event Action<MatchRoom, MatchPlayer>? PlayerJoined;
        public event Action<MatchRoom, MatchPlayer>? PlayerLeft;

        public MatchPlayerManager(MatchRoom matchRoom) : base(matchRoom)
        {
            _matchRoom = matchRoom;
        }

        public bool TryCreate(AuthUser authUser)
        {
            if (MatchHelper.TryGeneratePlayer(_matchRoom, authUser, out var matchPlayer, out var matchPlayerInfo))
                authUser.Dispose();
            else
                return false;

            var team = _matchRoom.MatchInformation.GetTeam(matchPlayerInfo.StadiumSitting);
            if (team == null || team.Any(
                p => p.PlayerId == matchPlayerInfo.PlayerId ||
                p.SquadNumber == matchPlayerInfo.SquadNumber
            ))
                return false;

            if (TryAdd(matchPlayer))
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

            PlayerJoined?.Invoke(_matchRoom, matchPlayer);
            return true;
        }

        public override bool TryRemove(Guid id, [MaybeNullWhen(false)] out MatchPlayer matchPlayer)
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

            PlayerLeft?.Invoke(_matchRoom, matchPlayer);
            return true;
        }
    }
}