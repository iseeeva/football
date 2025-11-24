using System.Numerics;
using Sobee.Common;
using Sobee.Messaging;
using Sobee.TestServer.Match;
using Sobee.TestServer.MatchComponents;
using Sobee.TestServer.Messages;
using Sobee.TestServer.Messages.Player;

namespace Sobee.TestServer.GameEvents
{
    public class MatchClientPlayerEvent
    {
        private static readonly Serilog.ILogger _log = Logging.Get<MatchClientPlayerEvent>();

        public static void HeartbeatMessageReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not MatchRoom matchRoom) return;
            if (e.handler is not MatchPlayer matchPlayer) return;
            if (e.message is not HeartbeatMessage heartbeatMessage) return;

            //matchPlayer.SendMessage(new Sobee.TestServer.Messages.HeartbeatMessage(heartbeatMessage.Timestamp));
            //_log.Debug("[HeartbeatMessageReceived] Heartbeat received from {playerId}", matchPlayer.Id);
        }

        public static void PlayerMoveKeyDownReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not MatchRoom match) return;
            if (e.handler is not MatchPlayer player) return;
            if (e.message is not PlayerMoveKeyDown moveKeyDown) return;

            var movementComponent = match.Components.GetComponent<MatchMovement>();
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

            match.Players.SendMessage(new PlayerMove(
                (sbyte)player.AuthInformation.Entry.ToSquad(),
                playerMatchInfo.Position,
                new Vector2(playerMatchInfo.Velocity.X, playerMatchInfo.Velocity.Y),
                moveKeyDown.IsSprint,
                false,
                (byte)playerMatchInfo.Stamina
            ));

            playerMatchInfo.Moving = true;
            _log.Debug("[PlayerMoveKeyDownReceived] Player {playerId} moving to {direction}.", player.Id, playerMatchInfo.Direction);
        }

        public static void PlayerMoveKeyUpReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not MatchRoom matchRoom) return;
            if (e.handler is not MatchPlayer matchPlayer) return;
            if (e.message is not PlayerMoveKeyUp moveReleased) return;

            var playerInformation = matchRoom.MatchInformation.GetPlayer(matchPlayer.Id);
            if (playerInformation == null)
            {
                _log.Warning("[PlayerMoveKeyUpReceived] MatchPlayer {playerId} not found in MatchRoom {matchId}.", matchPlayer.Id, matchRoom.Id);
                return;
            }

            matchRoom.Players.SendMessage(new PlayerStop(
                (sbyte)matchPlayer.AuthInformation.Entry.ToSquad(),
                playerInformation.Position,
                playerInformation.Direction,
                false,
                (byte)playerInformation.Stamina
            ));

            playerInformation.Direction = Vector2.Zero;
            playerInformation.Velocity = Vector3.Zero;

            playerInformation.Moving = false;
            _log.Debug("[PlayerMoveKeyUpReceived] Player {playerId} stopped moving.", matchPlayer.Id);
        }
    }
}
