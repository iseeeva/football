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
        private readonly MatchHelper _matchHelper;

        public event Action<MatchRoom, MatchPlayer>? PlayerJoined;
        public event Action<MatchRoom, MatchPlayer>? PlayerLeft;

        public MatchPlayerManager(MatchRoom matchRoom)
            : base(matchRoom)
        {
            _matchRoom = matchRoom;
            _matchHelper = new MatchHelper(_matchRoom);
        }

        public bool TryAddPlayer(AuthUser authUser)
        {
            if (_matchHelper.TryGeneratePlayer(authUser, out var matchPlayer, out var matchPlayerInfo))
                authUser.Dispose();
            else
                return false;

            if (_matchRoom.MatchInformation.GetTeam(matchPlayerInfo.StadiumSitting).Any(
                p => p.PlayerId == matchPlayerInfo.PlayerId ||
                p.SquadNumber == matchPlayerInfo.SquadNumber
            ))
                return false;

            if (TryAdd(matchPlayer))
            {
                if (!_matchHelper.TryAssignPlayerInfoToMatchInfo(matchPlayerInfo))
                {
                    if (!TryRemovePlayer(matchPlayer.Id))
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

        public bool TryRemovePlayer(Guid id)
        {
            if (!TryRemove(id, out var matchPlayer))
                return false;

            if (!_matchHelper.TryRemovePlayerInfoFromMatchInfo(matchPlayer))
            {
                _log.Error("{managerId}, failed to remove player info after removing player. PlayerId: {playerId}", Id, matchPlayer.Id);
                _matchRoom.Dispose();
                return false;
            }

            matchPlayer.Disconnect();

            PlayerLeft?.Invoke(_matchRoom, matchPlayer);
            return true;
        }
    }
}