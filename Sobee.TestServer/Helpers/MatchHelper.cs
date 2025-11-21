using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Sobee.TestServer.Auth;
using Sobee.TestServer.Match;
using Sobee.TestServer.Messages.Match;
using Sobee.TestServer.Messages.Player;

namespace Sobee.TestServer.Helpers
{
    public class MatchHelper
    {
        private readonly MatchRoom _matchRoom;

        public MatchHelper(MatchRoom matchRoom)
        {
            _matchRoom = matchRoom;
        }

        public bool TryGeneratePlayer(
            AuthUser authUser,
            [MaybeNullWhen(false)] out MatchPlayer matchPlayer,
            [MaybeNullWhen(false)] out PlayerMatchInformation playerMatchInformation
        )
        {
            // TODO: Database den kontrol edilecek

            matchPlayer = null;
            playerMatchInformation = null;

            if (authUser.Socket == null || authUser.AuthInformation == null)
                return false;

            matchPlayer = new MatchPlayer(authUser.AuthInformation, authUser.Socket, _matchRoom);
            playerMatchInformation = new PlayerMatchInformation(
                  _matchRoom.Id,
                  matchPlayer.Id,
                  $"Temporary {authUser.AuthInformation.Entry.EntryNumber}",
                  new PlayerAppearance(),
                  _matchRoom.MatchInformation.ScenarioInfo.GetScenarioSittingFromEntry(authUser.AuthInformation.Entry.EntryNumber),
                  (sbyte)authUser.AuthInformation.Entry.ToSquad(true),
                  new Vector2(0, 0),
                  new Vector3(0, 0, 0),
                  new Vector2(0, 0),
                  MatchCard.None,
                  "<XMLData><Script></Script></XMLData>"
            );

            return true;
        }

        public bool TryAssignPlayerInfoToMatchInfo(PlayerMatchInformation playerInformation)
        {
            if (_matchRoom.Players.Count > MatchRoom.MAX_PLAYER)
                return false;

            if (_matchRoom.MatchInformation.GetTeam(playerInformation.StadiumSitting).Any(
                    p => p.PlayerId == playerInformation.PlayerId ||
                    p.SquadNumber == playerInformation.SquadNumber
               ))
                return false;

            try
            {
                var team = _matchRoom.MatchInformation.GetTeam(playerInformation.StadiumSitting);
                team?.Add(playerInformation);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool TryRemovePlayerInfoFromMatchInfo(MatchPlayer player)
        {
            if (player.AuthInformation == null)
                return false;

            var playerInformation = _matchRoom.MatchInformation.GetPlayer(player.Id);
            if (playerInformation == null)
                return false;

            try
            {
                var team = _matchRoom.MatchInformation.GetTeam(playerInformation.StadiumSitting);
                team?.Remove(playerInformation);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
