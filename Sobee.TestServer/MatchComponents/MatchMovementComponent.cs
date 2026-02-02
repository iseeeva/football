using System.Numerics;
using Sobee.Common;
using Sobee.Serialization.GameServer;
using Sobee.TestServer.Match;
using Sobee.TestServer.MatchHelpers;
using Sobee.TestServer.Messages.Match;

namespace Sobee.TestServer.MatchComponents
{
    internal class MatchMovementComponent : MatchComponent
    {
        private readonly Serilog.ILogger _log = Logging.Get<MatchMovementComponent>();
        private bool _isDisposed;

        /// <summary>Movement collision radius</summary>
        public double MovementCollisionRadius { get; set; } = 15;

        private double _baseMovementWalkSpeed = 250;
        /// <summary>Movement walk speed (per frame)</summary>
        public double MovementWalkSpeed
        {
            get => _baseMovementWalkSpeed * MovementWalkFactor;
            set => _baseMovementWalkSpeed = value;
        }

        private double _baseMovementWalkFactor = 1;
        /// <summary>Movement walk factor</summary>
        public double MovementWalkFactor
        {
            // INFO: Eger mac akisina esitlemek istersen TimeMultiplier kullan.
            get => _baseMovementWalkFactor; // * _matchRoom.MatchInformation.TimeMultiplier;
            set => _baseMovementWalkFactor = value;
        }

        public double _baseMovementSprintFactor = 1.5;
        /// <summary>Movement sprint factor (for sprintSpeed)</summary>
        public double MovementSprintFactor
        {
            get => _baseMovementSprintFactor;
            set => _baseMovementSprintFactor = value;
        }

        /// <summary>Movement sprint speed (per frame)</summary>
        public double MovementSprintSpeed
            => MovementWalkSpeed * MovementSprintFactor;

        public MatchMovementComponent(MatchRoom room) : base(room)
        {
            _log.Information("{id} initialized.", Id);
        }

        public override void Update(double delta)
        {
            var matchInfo = _matchRoom.MatchInformation;
            var ballComponent = _matchRoom.Components.GetComponent<MatchBallComponent>();

            foreach (var playerMatchInfo in matchInfo.GetPlayers())
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
            MatchBallComponent ballComponent)
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

            bool ballFree = matchInfo.BallOwner == -1;

            if (inRange && ballFree)
            {
                if (MatchBallHelper.GetBall(_matchRoom, playerMatchInfo.GetAbsoluteSquadNumber()))
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
