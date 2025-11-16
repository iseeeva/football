using System.Numerics;
using Sobee.TestServer.Match;
using Sobee.TestServer.Messages.Ball;
namespace Sobee.TestServer.Helpers
{
    public class BallHelper
    {
        public static readonly double BallBoundryZ = 11.254;

        public static void GetBall(MatchRoom matchRoom, sbyte squadNumber, float speed)
        {
            var playerMatchInformation = matchRoom.MatchInformation.GetPlayer(squadNumber);
            if (playerMatchInformation == null)
                throw new ArgumentException($"No player found with squad number {squadNumber}.");
            if (!matchRoom.Players.TryGet(playerMatchInformation.PlayerId, out var matchPlayer))
                throw new ArgumentException($"No match player found with player ID {playerMatchInformation.PlayerId}.");

            var ballPosition = playerMatchInformation.Position;
            matchRoom.MatchInformation.BallPosition = new Vector3(ballPosition.X, ballPosition.Y, (float)BallHelper.BallBoundryZ);

            var ballDirection = playerMatchInformation.Direction;
            matchRoom.MatchInformation.BallVelocity = new Vector3(0, 0, 0);

            var ballGetMessage = new Messages.Ball.BallGet(
                squadNumber,
                ballPosition,
                ballDirection,
                speed
            );

            // Update all players with the new ball position and velocity
            matchRoom.Players.SendMessage(new BallUpdate(matchRoom.MatchInformation.BallPosition, matchRoom.MatchInformation.BallVelocity));

            // Update the actioner in match information
            matchRoom.MatchInformation.Actor.Actioner = (sbyte)matchPlayer.AuthInformation.Entry.ToSquad();
            matchRoom.Players.SendMessage(ballGetMessage);
        }
    }
}
