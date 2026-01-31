using System.Numerics;
using Sobee.Common;
using Sobee.Network.Messaging;
using Sobee.TestServer.Match;
using Sobee.TestServer.MatchComponents;
using Sobee.TestServer.Messages.Player;

namespace Sobee.TestServer.ClientEvents
{
    public class MatchClientPlayerMovementEvent
    {
        private static readonly Serilog.ILogger _log = Logging.Get<MatchClientPlayerMovementEvent>();

        public static void PlayerMoveKeyDownReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not MatchRoom match) return;
            if (e.handler is not MatchPlayer player) return;
            if (e.message is not PlayerMoveKeyDownMessage moveKeyDown) return;

            var movementComponent = match.Components.GetComponent<MatchMovementComponent>();
            if (movementComponent == null)
            {
                _log.Warning("[PlayerMoveKeyDownReceived] MatchMovement component not found in MatchRoom {matchId}.", match.Id);
                return;
            }

            var playerMatchInfo = match.MatchInformation.GetPlayer(player.Id);
            if (playerMatchInfo == null)
            {
                _log.Warning("[PlayerMoveKeyDownReceived] MatchPlayer {playerId} not found in MatchRoom {matchId}.", player.Id, match.Id);
                return;
            }

            float movementSpeed = (float)(moveKeyDown.IsSprint ? movementComponent.MovementSprintSpeed : movementComponent.MovementWalkSpeed);

            playerMatchInfo.Direction = Vector2.Normalize(moveKeyDown.Velocity);
            playerMatchInfo.Velocity = new Vector3(
                playerMatchInfo.Direction.X * movementSpeed,
                playerMatchInfo.Direction.Y * movementSpeed,
                0f
            );

            match.Players.SendMessage(new PlayerMoveMessage(
                playerMatchInfo.GetAbsoluteSquadNumber(),
                playerMatchInfo.Position,
                new Vector2(playerMatchInfo.Velocity.X, playerMatchInfo.Velocity.Y),
                moveKeyDown.IsSprint,
                false,
                (byte)playerMatchInfo.Stamina
            ));

            playerMatchInfo.IsMoving = true;
            _log.Debug("[PlayerMoveKeyDownReceived] Player {playerId} moving to {direction}.", player.Id, playerMatchInfo.Direction);
        }

        public static void PlayerMoveKeyUpReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not MatchRoom matchRoom) return;
            if (e.handler is not MatchPlayer matchPlayer) return;
            if (e.message is not PlayerMoveKeyUpMessage moveReleased) return;

            var playerInformation = matchRoom.MatchInformation.GetPlayer(matchPlayer.Id);
            if (playerInformation == null)
            {
                _log.Warning("[PlayerMoveKeyUpReceived] MatchPlayer {playerId} not found in MatchRoom {matchId}.", matchPlayer.Id, matchRoom.Id);
                return;
            }

            matchRoom.Players.SendMessage(new PlayerStopMessage(
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
