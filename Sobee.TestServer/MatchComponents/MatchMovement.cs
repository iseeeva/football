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

        /// <summary>Movement sprint speed (per frame)</summary>
        public double MovementSprintSpeed => MovementWalkSpeed * MovementSprintFactor;
        public double MovementSprintFactor { get; set; } = 1.5;

        public MatchMovement(MatchRoom room) : base(room)
        {
            _log.Information("{id} initialized.", Id);
        }

        public override Task Update(double delta)
        {
            var matchInformation = _matchRoom.MatchInformation;
            var ballComponent = _matchRoom.Components.GetComponent<MatchBall>();

            foreach (var playerMatchInfo in matchInformation.GetPlayers())
            {
                var player = _matchRoom.Players[playerMatchInfo.PlayerId];
                if (player == null)
                {
                    _log.Error("[MatchMovement] Player {playerId} not found in MatchRoom players.", playerMatchInfo.PlayerId);
                    _matchRoom.Dispose();
                    break;
                }

                HandlePlayerMovement(player, delta);

                if (matchInformation.FieldPositioning == MatchFieldPositioning.Running)
                {
                    if (ballComponent != null)
                    {
                        HandleBallCollision(player, matchInformation, ballComponent);
                    }
                }
            }

            return base.Update(delta);
        }

        private void HandlePlayerMovement(MatchPlayer player, double delta)
        {
            var playerMatchInfo = _matchRoom.MatchInformation.GetPlayer(player.Id);
            if (playerMatchInfo == null)
            {
                _log.Error("[MatchMovement] Player {playerId} match information not found.", player.Id);
                player.Disconnect();
                return;
            }

            if (!playerMatchInfo.IsMoving) return;

            if (playerMatchInfo.Velocity.Length() > 0)
            {
                playerMatchInfo.Position.X += (float)(playerMatchInfo.Velocity.X * delta);
                playerMatchInfo.Position.Y += (float)(playerMatchInfo.Velocity.Y * delta);
            }
            else
            {
                playerMatchInfo.IsMoving = false;
                playerMatchInfo.Velocity = Vector3.Zero;
                _log.Warning("[MatchMovement] Player {playerId} velocity is below zero but moving is true.", playerMatchInfo.PlayerId);
            }
        }

        private void HandleBallCollision(
            MatchPlayer player,
            MatchInformationMessage matchInformation,
            MatchBall ballComponent)
        {
            var playerMatchInfo = matchInformation.GetPlayer(player.Id);
            if (playerMatchInfo == null)
            {
                _log.Error("[MatchMovement] Player {playerId} match information not found.", player.Id);
                player.Disconnect();
                return;
            }

            var playerPos2D = new Vector2(playerMatchInfo.Position.X, playerMatchInfo.Position.Y);
            var ballPos2D = new Vector2(matchInformation.BallPosition.X, matchInformation.BallPosition.Y);

            bool isPlayerInCollision = Vector2.Distance(ballPos2D, playerPos2D) <= (ballComponent.BallCollisionRadius + MovementCollisionRadius);
            bool isActionerUnset = matchInformation.Actor.BallOwner == -1;

            //if (isPlayerInCollision)
            //{
            //    player.SendMessage(new Messages.Chat.ChatSystemMessage(
            //        $"[MatchMovement] Player {player.Id} is in ball collision range.",
            //        Messages.Chat.ChatSystemMessageType.General));
            //}

            if (isPlayerInCollision && isActionerUnset)
            {
                if (BallHelper.GetBall(_matchRoom, (sbyte)player.AuthInformation.Entry.ToSquad()))
                {
                    _matchRoom.Players.SendMessage(new Messages.Chat.ChatSystemMessage($"[MatchMovement] Player {player.Id} took the ball.", Messages.Chat.ChatSystemMessageType.General));
                    _log.Information("[MatchMovement] Player {player.Id} took the ball.", player.Id);
                }
                else
                {
                    _log.Error("[MatchMovement] Failed to assign ball to player {playerId}.", player.Id);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                if (disposing)
                {
                    _log.Debug("{id} disposed.", Id);
                }
            }

            base.Dispose(disposing);
        }
    }
}
