using System.Numerics;
using Sobee.TestServer.Match;
using Sobee.TestServer.MatchComponents;
using Sobee.TestServer.Messages.Ball;
namespace Sobee.TestServer.Helpers
{
    public class BallHelper
    {
        public static void GetBall(MatchRoom matchRoom, sbyte squadNumber)
        {
            var ballComponent = matchRoom.Components.GetComponent<MatchBall>();
            if (ballComponent == null)
                throw new InvalidOperationException("MatchBallComponent not found in MatchRoom.");

            var playerMatchInformation = matchRoom.MatchInformation.GetPlayer(squadNumber);
            if (playerMatchInformation == null)
                throw new ArgumentException($"No player found with squad number {squadNumber}.");

            var matchPlayer = matchRoom.Players[playerMatchInformation.PlayerId];
            if (matchPlayer == null)
                throw new ArgumentException($"No match player found with player ID {playerMatchInformation.PlayerId}.");

            var playerPosition = playerMatchInformation.Position;
            matchRoom.MatchInformation.BallPosition = new Vector3(playerPosition.X, playerPosition.Y, (float)ballComponent.BallBoundary.Z);

            var playerDirection = playerMatchInformation.Direction;
            matchRoom.MatchInformation.BallVelocity = new Vector3(0, 0, 0);

            matchRoom.MatchInformation.Actor.Actioner = (sbyte)matchPlayer.AuthInformation.Entry.ToSquad();
            matchRoom.Players.SendMessage(new BallUpdate(matchRoom.MatchInformation.BallPosition, matchRoom.MatchInformation.BallVelocity));
            matchRoom.Players.SendMessage(new BallGet(
                squadNumber,
                playerPosition,
                playerDirection,
                0
            ));
        }
    }
}
