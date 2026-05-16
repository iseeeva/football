using Sobee.Common;
using Sobee.Network.Messaging;
using Sobee.TestServer.Match;
using Sobee.TestServer.MatchComponents;
using Sobee.TestServer.Messages.Player;
using System.Numerics;

namespace Sobee.TestServer.MatchEvents
{
    public class MatchClientPlayerMovementEvent
    {
        private static readonly Serilog.ILogger _log = LogFactory.GetContextForType<MatchClientPlayerMovementEvent>();

        public static void PlayerMoveKeyDownReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not MatchRoom matchRoom) return;
            if (e.Handler is not MatchPlayer matchPlayer) return;
            if (e.Message is not PlayerMoveKeyDownRxMessage moveKeyDown) return;

            var movementComponent = matchRoom.GetComponent<MatchMovementComponent>();
            if (movementComponent == null)
            {
                _log.Warning("[PlayerMoveKeyDownReceived] MatchMovement component not found in MatchRoom {matchId}.", matchRoom.Id);
                return;
            }

            var playerMatchInfo = matchRoom.MatchInformation.GetPlayer(matchPlayer.Id);
            if (playerMatchInfo == null)
            {
                _log.Warning("[PlayerMoveKeyDownReceived] MatchPlayer {playerId} not found in MatchRoom {matchId}.", matchPlayer.Id, matchRoom.Id);
                return;
            }

            float movementSpeed = (float)(moveKeyDown.IsSprint ? movementComponent.MovementSprintSpeed : movementComponent.MovementWalkSpeed);

            playerMatchInfo.Direction = Vector2.Normalize(moveKeyDown.Direction);
            playerMatchInfo.Velocity = new Vector3(
                playerMatchInfo.Direction.X * movementSpeed,
                playerMatchInfo.Direction.Y * movementSpeed,
                0f
            );

            matchRoom.Players.SendMessage(new PlayerMoveTxMessage(
                playerMatchInfo.GetAbsoluteSquadNumber(),
                playerMatchInfo.Position,
                new Vector2(playerMatchInfo.Velocity.X, playerMatchInfo.Velocity.Y),
                moveKeyDown.IsSprint,
                false,
                (byte)playerMatchInfo.Stamina
            ));

            playerMatchInfo.IsMoving = true;
            _log.Debug("[PlayerMoveKeyDownReceived] Player {playerId} moving to {direction}.", matchPlayer.Id, playerMatchInfo.Direction);
        }

        public static void PlayerMoveKeyUpReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not MatchRoom matchRoom) return;
            if (e.Handler is not MatchPlayer matchPlayer) return;
            if (e.Message is not PlayerMoveKeyUpRxMessage moveReleased) return;

            var playerInformation = matchRoom.MatchInformation.GetPlayer(matchPlayer.Id);
            if (playerInformation == null)
            {
                _log.Warning("[PlayerMoveKeyUpReceived] MatchPlayer {playerId} not found in MatchRoom {matchId}.", matchPlayer.Id, matchRoom.Id);
                return;
            }

            matchRoom.Players.SendMessage(new PlayerStopTxMessage(
                playerInformation.GetAbsoluteSquadNumber(),
                playerInformation.Position,
                playerInformation.Direction,
                false,
                (byte)playerInformation.Stamina
            ));

            playerInformation.Direction = Vector2.Zero;
            playerInformation.Velocity = Vector3.Zero;

            playerInformation.IsMoving = false;
            _log.Information("[PlayerMoveKeyUpReceived] Player {playerId} stopped moving.", matchPlayer.Id);
        }
    }
}
