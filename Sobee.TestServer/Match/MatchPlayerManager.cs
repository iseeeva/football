using Sobee.TestServer.Auth;

namespace Sobee.TestServer.Match
{
    public class MatchPlayerManager : SessionManager<MatchPlayer>
    {
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
            if (!_matchHelper.TryGeneratePlayer(authUser, out var player, out var info))
                return false;

            authUser.Dispose();

            if (!TryAdd(player))
            {
                player.Disconnect();
                return false;
            }

            _matchHelper.TryAssignPlayerInfoToMatchInfo(info);
            PlayerJoined?.Invoke(_matchRoom, player);
            return true;
        }

        public bool TryRemovePlayer(Guid id)
        {
            if (!TryGet(id, out var player))
                return false;

            if (!TryRemove(id, out _))
                return false;

            _matchHelper.TryRemovePlayerInfoFromMatchInfo(player);
            PlayerLeft?.Invoke(_matchRoom, player);
            return true;
        }
    }
}