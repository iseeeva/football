using Football.Common;
using Football.GameServer.Match;
using Football.GameServer.MatchComponents;
using Football.GameServer.Messages.Ball;
using System.Numerics;
namespace Football.GameServer.MatchHelpers
{
    public class MatchBallHelper
    {
        private static readonly Serilog.ILogger _log = LogFactory.GetContextForType<MatchBallHelper>();

        public static bool GetBall(MatchRoom matchRoom, sbyte squadNumber)
        {
            var ballComponent = matchRoom.GetComponent<MatchBallComponent>();
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
            matchRoom.Players.SendMessage(new BallUpdateTxMessage(matchRoom.MatchInformation.BallPosition, matchRoom.MatchInformation.BallVelocity));
            matchRoom.Players.SendMessage(new BallPlayerGetTxMessage(
                squadNumber,
                playerPosition,
                playerDirection,
                0
            ));

            return true;
        }
    }
}
