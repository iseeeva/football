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
        public static bool TryGeneratePlayer(
            MatchRoom matchRoom,
            AuthUser authUser,
            [NotNullWhen(true)] out MatchPlayer? matchPlayer,
            [NotNullWhen(true)] out PlayerMatchInformationMessage? playerMatchInformation
        )
        {
            // TODO: Database den kontrol edilecek

            matchPlayer = null;
            playerMatchInformation = null;

            if (authUser.Socket == null || authUser.AuthInformation == null)
                return false;

            matchPlayer = new MatchPlayer(authUser.Socket, authUser.AuthInformation, matchRoom);
            playerMatchInformation = new PlayerMatchInformationMessage(
                  matchRoom.Id,
                  matchPlayer.Id,
                  $"Temporary {authUser.AuthInformation.Entry.EntryNumber}",
                  new PlayerAppearanceMessage(),
                  matchRoom.MatchInformation.ScenarioInfo.GetScenarioSittingFromEntry(authUser.AuthInformation.Entry.EntryNumber),
                  (sbyte)authUser.AuthInformation.Entry.ToSquad(true),
                  new Vector2(0, 0),
                  new Vector3(0, 0, 0),
                  new Vector2(0, 0),
                  MatchCard.None,
                  "<XMLData><Script></Script></XMLData>"
            );

            return true;
        }

        public static PlayerMatchInformationMessage? GetPlayerInfoFromMatchInfo(MatchRoom matchRoom, Guid playerId)
        {
            if (matchRoom == null)
                return null;

            return matchRoom.MatchInformation.GetPlayer(playerId);
        }

        public static bool TryAssignPlayerInfoToMatchInfo(MatchRoom matchRoom, PlayerMatchInformationMessage playerInformation)
        {
            if (matchRoom == null || playerInformation == null)
                return false;

            if (matchRoom.Players.Count > MatchRoom.MAX_PLAYER)
                return false;

            var team = matchRoom.MatchInformation.GetSittingSide(playerInformation.StadiumSitting);

            if (team == null || team.Any(
                    p => p.PlayerId == playerInformation.PlayerId ||
                    p.SquadNumber == playerInformation.SquadNumber
               ))
                return false;

            team.Add(playerInformation);
            return true;
        }

        public static bool TryRemovePlayerInfoFromMatchInfo(MatchRoom matchRoom, PlayerMatchInformationMessage playerInformation)
        {
            if (matchRoom == null || playerInformation == null)
                return false;

            var team = matchRoom.MatchInformation.GetSittingSide(playerInformation.StadiumSitting);
            if (team == null)
                return false;

            var targetInformation = team.FirstOrDefault(p =>
                p.PlayerId == playerInformation.PlayerId &&
                p.SquadNumber == playerInformation.SquadNumber);

            if (targetInformation == null)
                return false;

            team.Remove(targetInformation);
            return true;
        }
    }
}
