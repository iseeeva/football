using Sobee.TestServer.Auth;
using Sobee.TestServer.Match;
using Sobee.TestServer.Messages;
using Sobee.TestServer.Messages.Match;
using Sobee.TestServer.Messages.Player;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Sobee.TestServer.MatchHelpers
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
            // TODO: Database den kontrol edilecek.

            matchPlayer = null;
            playerMatchInformation = null;

            if (authUser.Socket == null || authUser.AuthInformation == null)
                return false;

            // TODO: Gecici squad numarasi, database eklenince duzeltilmesi gerek.
            sbyte tempAbsoluteSquadNo0 = (sbyte)(authUser.AuthInformation.Entry.Value - 1);
            if (!PlayerSquadNumber.IsValidAbsoluteSquadNumber(tempAbsoluteSquadNo0))
                return false;

            var tempSquadNo0 = tempAbsoluteSquadNo0 % ScenarioInfo.MAX_TEAM_SIZE;
            // ----

            // TODO: Gecici squad atamalari, database eklenince duzeltilmesi gerek.
            var homePlayer = matchRoom.MatchInformation.GetSittingSide(StadiumSitting.HomePlayer);
            var awayPlayer = matchRoom.MatchInformation.GetSittingSide(StadiumSitting.AwayPlayer);
            var homeSpectator = matchRoom.MatchInformation.GetSittingSide(StadiumSitting.HomeSpectator);
            var awaySpectator = matchRoom.MatchInformation.GetSittingSide(StadiumSitting.AwaySpectator);
            if (homePlayer == null || awayPlayer == null || homeSpectator == null || awaySpectator == null)
                return false;

            var tempSittingSide = PlayerSquadNumber.GetSittingFromAbsoluteSquadNumber(tempAbsoluteSquadNo0, [
                homePlayer.Count<matchRoom.MatchInformation.ScenarioInfo.team1Size
                    ? StadiumSitting.HomePlayer
                    : homeSpectator.Count < homeSpectator.Capacity ? StadiumSitting.HomeSpectator : StadiumSitting.Invalid,
                awayPlayer.Count<matchRoom.MatchInformation.ScenarioInfo.team2Size
                    ? StadiumSitting.AwayPlayer
                    : awaySpectator.Count < awaySpectator.Capacity ? StadiumSitting.AwaySpectator : StadiumSitting.Invalid
            ]);
            // ----

            matchPlayer = new MatchPlayer(authUser.Socket, authUser.AuthInformation, matchRoom.Communication);
            playerMatchInformation = new PlayerMatchInformationMessage(
                  matchRoom.Id,
                  matchPlayer.Id,
                      $"Temporary {authUser.AuthInformation.Entry.Value}",
                      new PlayerAppearanceMessage(),
                      tempSittingSide,
                      new PlayerSquadNumber((sbyte)tempSquadNo0),
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

            var sittingSide = matchRoom.MatchInformation.GetSittingSide(playerInformation.StadiumSitting);

            if (
                sittingSide == null ||
                sittingSide.Count >= sittingSide.Capacity ||
                sittingSide.Any(
                    p => p.PlayerId == playerInformation.PlayerId ||
                    p.SquadNumber == playerInformation.SquadNumber
                )
               )
                return false;

            sittingSide.Add(playerInformation);
            return true;
        }

        public static bool TryRemovePlayerInfoFromMatchInfo(MatchRoom matchRoom, PlayerMatchInformationMessage playerInformation)
        {
            if (matchRoom == null || playerInformation == null)
                return false;

            var sittingSide = matchRoom.MatchInformation.GetSittingSide(playerInformation.StadiumSitting);
            if (
                sittingSide == null ||
                !sittingSide.Any(p =>
                    p.PlayerId == playerInformation.PlayerId &&
                    p.SquadNumber == playerInformation.SquadNumber
                )
               )
                return false;

            sittingSide.Remove(playerInformation);
            return true;
        }
    }
}
