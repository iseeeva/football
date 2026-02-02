using System.Numerics;
using Sobee.Common;
using Sobee.TestServer.Match;
using Sobee.TestServer.MatchComponents;
using Sobee.TestServer.Messages.Ball;
namespace Sobee.TestServer.MatchHelpers
{
    public class MatchBallHelper
    {
        private static readonly Serilog.ILogger _log = Logging.Get<MatchBallHelper>();

        public static bool GetBall(MatchRoom matchRoom, sbyte squadNumber)
        {
            var ballComponent = matchRoom.Components.GetComponent<MatchBallComponent>();
            if (ballComponent == null)
            {
                _log.Warning("[BallHelper] MatchBall is null in match {matchId}.", matchRoom.Id);
                return false;
            }

            var playerMatchInformation = matchRoom.MatchInformation.GetPlayer(squadNumber);
            if (playerMatchInformation == null)
            {
                _log.Warning("[BallHelper] No player match information found for squad number {squadNumber} in match {matchId}.", squadNumber, matchRoom.Id);
                return false;
            }

            var matchPlayer = matchRoom.Players[playerMatchInformation.PlayerId];
            if (matchPlayer == null)
            {
                _log.Warning("[BallHelper] No match player found for player ID {playerId} in match {matchId}.", playerMatchInformation.PlayerId, matchRoom.Id);
                return false;
            }

            var playerPosition = playerMatchInformation.Position;
            matchRoom.MatchInformation.BallPosition = new Vector3(playerPosition.X, playerPosition.Y, ballComponent.BallBoundary.Z);

            var playerDirection = playerMatchInformation.Direction;
            matchRoom.MatchInformation.BallVelocity = Vector3.Zero;

            matchRoom.MatchInformation.BallOwner = playerMatchInformation.GetAbsoluteSquadNumber();
            matchRoom.Players.SendMessage(new BallUpdateMessage(matchRoom.MatchInformation.BallPosition, matchRoom.MatchInformation.BallVelocity));
            matchRoom.Players.SendMessage(new BallGetMessage(
                squadNumber,
                playerPosition,
                playerDirection,
                0
            ));

            return true;
        }
    }
}
