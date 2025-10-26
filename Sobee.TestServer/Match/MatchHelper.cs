using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Sobee.TestServer.Auth;
using Sobee.TestServer.Messages;
using Sobee.TestServer.Messages.Match;
using Sobee.TestServer.Messages.Player;

namespace Sobee.TestServer.Match
{
    public class MatchHelper(MatchRoom matchRoom)
    {
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

            matchPlayer = new MatchPlayer(authUser.AuthInformation, authUser.Socket, matchRoom);
            playerMatchInformation = new PlayerMatchInformation(
                  matchRoom.Id,
                  matchPlayer.Id,
                  $"Temporary {authUser.AuthInformation.Entry.EntryNumber}",
                  new PlayerAppearance(),
                  StadiumSitting.HomePlayer,
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
            if (matchRoom.PlayerCount > MatchRoom.MAX_PLAYER)
                return false;

            if (matchRoom.MatchInformation.HasPlayer((x) =>
                    x.PlayerId == playerInformation.PlayerId ||
                    x.SquadNumber == playerInformation.SquadNumber
            ))
                return false;

            switch (playerInformation.StadiumSitting)
            {
                case StadiumSitting.HomePlayer:
                    matchRoom.MatchInformation.HomeTeam.Add(playerInformation);
                    break;
                case StadiumSitting.HomeSpectator:
                    matchRoom.MatchInformation.HomeSpectator.Add(playerInformation);
                    break;
                case StadiumSitting.AwayPlayer:
                    matchRoom.MatchInformation.AwayTeam.Add(playerInformation);
                    break;
                case StadiumSitting.AwaySpectator:
                    matchRoom.MatchInformation.AwaySpectator.Add(playerInformation);
                    break;
                case StadiumSitting.Invalid:
                    return false;
            }

            return true;
        }

        public bool TryRemovePlayerInfoFromMatchInfo(MatchPlayer player)
        {
            if (player.AuthInformation == null)
                return false;

            var playerInfo = matchRoom.MatchInformation.GetPlayer(player.Id);
            if (playerInfo == null)
                return false;

            switch (playerInfo.StadiumSitting)
            {
                case StadiumSitting.HomePlayer:
                    matchRoom.MatchInformation.HomeTeam.Remove(playerInfo);
                    break;
                case StadiumSitting.HomeSpectator:
                    matchRoom.MatchInformation.HomeSpectator.Remove(playerInfo);
                    break;
                case StadiumSitting.AwayPlayer:
                    matchRoom.MatchInformation.AwayTeam.Remove(playerInfo);
                    break;
                case StadiumSitting.AwaySpectator:
                    matchRoom.MatchInformation.AwaySpectator.Remove(playerInfo);
                    break;
                case StadiumSitting.Invalid:
                    return false;
            }
            return true;
        }
    }
}
