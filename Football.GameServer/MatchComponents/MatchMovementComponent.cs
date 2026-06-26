using Football.GameServer.Match;
using Football.GameServer.MatchHelpers;
using Football.GameServer.Messages.Match;
using Football.Serialization.GameServer;
using System.Numerics;

namespace Football.GameServer.MatchComponents
{
    internal class MatchMovementComponent : MatchComponent<MatchRoom>
    {
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
        public double MovementSprintSpeed => MovementWalkSpeed * MovementSprintFactor;

        public MatchMovementComponent()
        {

        }

        #region Lifecycle
        protected override void OnUpdate(double delta)
        {
            if (Owner == null)
            {
                _log.Warning("Owner is null. Component aborted.");
                Stop();

                return;
            }

            var matchInfo = Owner.MatchInformation;
            var ballComponent = Owner.GetComponent<MatchBallComponent>();

            foreach (var playerMatchInfo in matchInfo.GetPlayers())
            {
                var player = Owner.Players[playerMatchInfo.PlayerId];
                if (player == null)
                {
                    _log.Error("player ({playerId}) not found, disposing match.", playerMatchInfo.PlayerId);
                    Owner.Dispose();
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
        #endregion

        #region Movement
        private void HandlePlayerMovement(MatchUser player, double delta)
        {
            if (Owner == null)
            {
                _log.Warning("Owner is null. HandlePlayerMovement aborted.");
                return;
            }

            var playerMatchInfo = Owner.MatchInformation.GetPlayer(player.Id);
            if (playerMatchInfo == null)
            {
                _log.Error("player ({playerId}) match info missing.", player.Id);
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
                _log.Warning("player ({playerId}) velocity zero but moving flag set.", playerMatchInfo.PlayerId);
            }
        }
        #endregion

        #region Ball Collision
        private void HandleBallCollision(
            MatchUser player,
            MatchInformationMessage matchInfo,
            MatchBallComponent ballComponent)
        {
            if (Owner == null)
            {
                _log.Warning("Owner is null. HandleBallCollision aborted.");
                return;
            }

            var playerMatchInfo = matchInfo.GetPlayer(player.Id);
            if (playerMatchInfo == null)
            {
                _log.Error("player ({playerId}) match info missing.", player.Id);
                player.Disconnect();
                return;
            }

            var playerPos = new Vector2(playerMatchInfo.Position.X, playerMatchInfo.Position.Y);
            var ballPos = new Vector2(matchInfo.BallPosition.X, matchInfo.BallPosition.Y);

            bool inRange =
                Vector2.DistanceSquared(playerPos, ballPos) <=
                Math.Pow(ballComponent.BallCollisionRadius + MovementCollisionRadius, 2)
                && matchInfo.BallPosition.Z <= 180; // TODO: Boy olcusu icin ekstra kontrol.

            bool ballFree = matchInfo.BallOwner == -1;

            if (inRange && ballFree)
            {
                if (MatchBallHelper.GetBall(Owner, playerMatchInfo.GetAbsoluteSquadNumber()))
                    _log.Information("player ({playerId}) took the ball.", player.Id);
                else
                    _log.Error("failed to assign ball to player ({playerId}).", player.Id);
            }
        }
        #endregion
    }
}