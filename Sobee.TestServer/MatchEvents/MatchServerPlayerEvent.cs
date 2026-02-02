using System.Numerics;
using Sobee.Common;
using Sobee.TestServer.Match;
using Sobee.TestServer.Messages.Ball;
using Sobee.TestServer.Messages.Chat;
using Sobee.TestServer.Messages.Player;

namespace Sobee.TestServer.MatchEvents
{
    public class MatchServerPlayerEvent
    {
        private static readonly Serilog.ILogger _log = Logging.Get<MatchServerPlayerEvent>();

        public static void PlayerJoinReceived(MatchRoom matchRoom, MatchPlayer matchPlayer)
        {
            // TODO: Additional logic for when a player joins can be added here.

            if (matchRoom == null || matchPlayer == null || matchPlayer.AuthInformation == null)
            {
                _log.Warning("[PlayerJoinReceived] Match or player is null.");
                return;
            }

            var matchInformation = matchRoom.MatchInformation.GetPlayer(matchPlayer.Id);
            if (matchInformation == null)
            {
                _log.Warning("[PlayerJoinReceived] Player {playerId} not found in Match {matchId}.", matchPlayer.Id, matchRoom.Id);
                matchRoom.Players.TryRemove(matchPlayer.Id, out _);
                return;
            }

            matchRoom.Players.SendMessage(new PlayerJoinMessage(matchInformation));
            matchRoom.Players.SendMessage(new ChatSystemMessage(
                $"{matchInformation.PlayerName} connected. (total player: {matchRoom.Players.Count})",
                ChatSystemMessageType.Anounce
            ));

            // INFO: Set camera and clientId to the joining player for their own view
            matchRoom.MatchInformation.ClientCamera = matchInformation.GetAbsoluteSquadNumber();
            matchRoom.MatchInformation.ClientId = matchInformation.PlayerId;
            matchPlayer.SendMessage(matchRoom.MatchInformation);

            _log.Information("[PlayerJoinReceived] Player {playerId} joined to match {matchId}.", matchPlayer.Id, matchRoom.Id);

            // Wait for heartbeat message before invoke the match event.
            // Heartbeat is the first message sent by the client after going match screen.
            matchPlayer.WaitForMessage<PlayerHeartbeatMessage>((player, heartbeat) =>
            {
                player.SendMessage(new ChatSystemMessage(
                    $"[PlayerJoinReceived] Room Id: {matchRoom.Id}",
                    ChatSystemMessageType.General));

                player.SendMessage(new ChatSystemMessage(
                    $"[PlayerJoinReceived] Your Id: {player.Id}",
                    ChatSystemMessageType.General));

                player.SendMessage(new ChatSystemMessage(
                    $"[PlayerJoinReceived] (SessionType: {player.SessionType})",
                    ChatSystemMessageType.General));

                player.SendMessage(new ChatSystemMessage(
                    $"[PlayerJoinReceived] (PlayerMatchInformation: {matchInformation})",
                    ChatSystemMessageType.General));
            });
        }

        public static void PlayerLeaveReceived(MatchRoom matchRoom, MatchPlayer matchPlayer, PlayerMatchInformationMessage matchPlayerInformation)
        {
            if (matchRoom == null || matchPlayer == null || matchPlayer.AuthInformation == null)
            {
                _log.Warning("[PlayerLeaveReceived] Match or player is null.");
                return;
            }

            if (matchRoom.MatchInformation.GetPlayer(matchPlayer.Id) != null)
            {
                _log.Warning("[PlayerLeaveReceived] Player {playerId} still in match {matchId}.", matchPlayer.Id, matchRoom.Id);
                matchRoom.Players.TryRemove(matchPlayer.Id, out _);
                return;
            }

            // TODO: If actioner is disconnected?
            if (matchRoom.MatchInformation.BallOwner == matchPlayerInformation.GetAbsoluteSquadNumber())
            {
                matchRoom.MatchInformation.BallOwner = -1;
                matchRoom.MatchInformation.BallVelocity = Vector3.Zero;
                matchRoom.Players.SendMessage(new BallUpdateMessage(matchRoom.MatchInformation.BallPosition, new Vector3(0, 0, 0)));
            }

            matchRoom.Players.SendMessage(new PlayerLeaveMessage(matchPlayerInformation.GetAbsoluteSquadNumber()));
            matchRoom.Players.SendMessage(new ChatSystemMessage(
                $"{matchPlayerInformation.PlayerName} disconnected. (total player: {matchRoom.Players.Count})",
                ChatSystemMessageType.Anounce
            ));

            _log.Information("[PlayerLeaveReceived] Player {playerId} left from match {matchId}.", matchPlayer.Id, matchRoom.Id);
        }
    }
}
