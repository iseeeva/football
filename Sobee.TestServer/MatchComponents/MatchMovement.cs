using System.Numerics;
using Sobee.Common;
using Sobee.Serialization.GameServer;
using Sobee.TestServer.Helpers;
using Sobee.TestServer.Match;
using Sobee.TestServer.Messages.Match;

namespace Sobee.TestServer.MatchComponents
{
    internal class MatchMovement : MatchComponent
    {
        private readonly Serilog.ILogger _log = Logging.Get<MatchMovement>();
        private bool _isDisposed;

        /// <summary>Movement collision radius</summary>
        public double MovementCollisionRadius { get; set; } = 15;
        /// <summary>Movement walk speed (per frame)</summary>
        public double MovementWalkSpeed { get; set; } = 250;

        /// <summary>Movement sprint factor (for sprintSpeed)</summary>
        public double MovementSprintFactor { get; set; } = 1.5;
        /// <summary>Movement sprint speed (per frame)</summary>
        public double MovementSprintSpeed => MovementWalkSpeed * MovementSprintFactor;

        public MatchMovement(MatchRoom room) : base(room)
        {
            _log.Information("{id} initialized.", Id);
        }

        public override void Update(double delta)
        {
            var matchInfo = _matchRoom.MatchInformation;
            var ballComponent = _matchRoom.Components.GetComponent<MatchBall>();

            foreach (var playerMatchInfo in matchInfo.GetTeamPlayers())
            {
                var player = _matchRoom.Players[playerMatchInfo.PlayerId];
                if (player == null)
                {
                    _log.Error("Player {playerId} not found, disposing match.", playerMatchInfo.PlayerId);
                    _matchRoom.Dispose();
                    return;
                }

                HandlePlayerMovement(player, delta);

                if (matchInfo.FieldPositioning == MatchFieldPositioning.Running &&
                    ballComponent != null)
                {
                    HandleBallCollision(player, matchInfo, ballComponent);
                }
            }
        }

        private void HandlePlayerMovement(MatchPlayer player, double delta)
        {
            var playerMatchInfo = _matchRoom.MatchInformation.GetPlayer(player.Id);
            if (playerMatchInfo == null)
            {
                _log.Error("Player {playerId} match info missing.", player.Id);
                player.Disconnect();
                return;
            }

            if (!playerMatchInfo.IsMoving)
                return;

            if (playerMatchInfo.Velocity.LengthSquared() > 0)
            {
                playerMatchInfo.Position.X += (float)(playerMatchInfo.Velocity.X * delta);
                playerMatchInfo.Position.Y += (float)(playerMatchInfo.Velocity.Y * delta);
            }
            else
            {
                playerMatchInfo.IsMoving = false;
                playerMatchInfo.Velocity = Vector3.Zero;
                _log.Warning("Player {playerId} velocity zero but moving flag set.", playerMatchInfo.PlayerId);
            }
        }

        private void HandleBallCollision(
            MatchPlayer player,
            MatchInformationMessage matchInfo,
            MatchBall ballComponent)
        {
            var playerMatchInfo = matchInfo.GetPlayer(player.Id);
            if (playerMatchInfo == null)
            {
                _log.Error("Player {playerId} match info missing.", player.Id);
                player.Disconnect();
                return;
            }

            var playerPos = new Vector2(playerMatchInfo.Position.X, playerMatchInfo.Position.Y);
            var ballPos = new Vector2(matchInfo.BallPosition.X, matchInfo.BallPosition.Y);

            bool inRange =
                (
                    Vector2.DistanceSquared(playerPos, ballPos) <=
                    Math.Pow(ballComponent.BallCollisionRadius + MovementCollisionRadius, 2)
                ) &&
                    matchInfo.BallPosition.Z <= 180; // TODO: Boy olcusu icin ekstra kontrol. Ilerde degistirilebilir.

            bool ballFree = matchInfo.Actor.BallOwner == -1;

            if (inRange && ballFree)
            {
                if (BallHelper.GetBall(_matchRoom, (sbyte)player.AuthInformation.Entry.ToSquad()))
                {
                    _log.Information("Player {playerId} took the ball.", player.Id);
                }
                else
                {
                    _log.Error("Failed to assign ball to player {playerId}.", player.Id);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            if (disposing)
                _log.Debug("{id} disposed.", Id);

            base.Dispose(disposing);
        }
    }
}
