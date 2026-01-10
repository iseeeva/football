using System.Numerics;
using Sobee.Common;
using Sobee.TestServer.Match;
using Sobee.TestServer.Messages;
using Sobee.TestServer.Messages.Ball;
using Sobee.TestServer.Messages.Chat;
using Sobee.TestServer.Messages.Player;

namespace Sobee.TestServer.GameEvents
{
    public class MatchServerEvent
    {
        private static readonly Serilog.ILogger _log = Logging.Get<MatchServerEvent>();

        public static void PlayerJoin(MatchRoom matchRoom, MatchPlayer matchPlayer)
        {
            // TODO: Additional logic for when a player joins can be added here.

            if (matchRoom == null || (matchPlayer == null || matchPlayer.AuthInformation == null))
            {
                _log.Warning("[PlayerJoin] Match or player is null.");
                return;
            }

            var matchInformation = matchRoom.MatchInformation.GetPlayer(matchPlayer.Id);
            if (matchInformation == null)
            {
                _log.Warning("[PlayerJoin] Player {playerId} not found in Match {matchId}.", matchPlayer.Id, matchRoom.Id);
                matchRoom.Players.TryRemove(matchPlayer.Id, out _);
                return;
            }

            matchRoom.Players.SendMessage(new PlayerJoinMessage(matchInformation));
            matchRoom.Players.SendMessage(new ChatSystemMessage(
                $"{matchInformation.PlayerName} connected. (total player: {matchRoom.Players.Count})",
                ChatSystemMessageType.Anounce
            ));

            // INFO: Set camera and clientId to the joining player for their own view
            matchRoom.MatchInformation.Actor.Camera = (sbyte)matchPlayer.AuthInformation.Entry.ToSquad();
            matchRoom.MatchInformation.Actor.ClientId = matchInformation.PlayerId;
            matchPlayer.SendMessage(matchRoom.MatchInformation);

            _log.Information("[PlayerJoin] Player {playerId} joined to match {matchId}.", matchPlayer.Id, matchRoom.Id);

            // Wait for heartbeat message before invoke the match event.
            // Heartbeat is the first message sent by the client after going match screen.
            matchPlayer.WaitForMessage<HeartbeatMessage>((player, heartbeat) =>
            {
                player.SendMessage(new ChatSystemMessage(
                    $"[PlayerJoin] Room Id: {matchRoom.Id}",
                    ChatSystemMessageType.General));

                player.SendMessage(new ChatSystemMessage(
                    $"[PlayerJoin] Your Id: {player.Id}",
                    ChatSystemMessageType.General));

                player.SendMessage(new ChatSystemMessage(
                    $"[PlayerJoin] (SessionType: {player.SessionType})",
                    ChatSystemMessageType.General));

                player.SendMessage(new ChatSystemMessage(
                    $"[PlayerJoin] (PlayerMatchInformation: {matchInformation})",
                    ChatSystemMessageType.General));
            });
        }

        public static void PlayerLeave(MatchRoom matchRoom, MatchPlayer matchPlayer, PlayerMatchInformationMessage matchPlayerInformation)
        {
            if (matchRoom == null || (matchPlayer == null || matchPlayer.AuthInformation == null))
            {
                _log.Warning("[PlayerLeave] Match or player is null.");
                return;
            }

            if (matchRoom.MatchInformation.GetPlayer(matchPlayer.Id) != null)
            {
                _log.Warning("[PlayerLeave] Player {playerId} still in match {matchId}.", matchPlayer.Id, matchRoom.Id);
                matchRoom.Players.TryRemove(matchPlayer.Id, out _);
                return;
            }

            // TODO: If actioner is disconnected?
            if (matchRoom.MatchInformation.Actor.BallOwner == matchPlayer.AuthInformation.Entry.ToSquad())
            {
                matchRoom.MatchInformation.Actor.BallOwner = -1;
                matchRoom.MatchInformation.BallVelocity = Vector3.Zero;
                matchRoom.Players.SendMessage(new BallUpdateMessage(matchRoom.MatchInformation.BallPosition, new Vector3(0, 0, 0)));
            }

            matchRoom.Players.SendMessage(new PlayerLeaveMessage((sbyte)matchPlayer.AuthInformation.Entry.ToSquad()));
            matchRoom.Players.SendMessage(new ChatSystemMessage(
                $"{matchPlayerInformation.PlayerName} disconnected. (total player: {matchRoom.Players.Count})",
                ChatSystemMessageType.Anounce
            ));

            _log.Information("[PlayerLeave] Player {playerId} left from match {matchId}.", matchPlayer.Id, matchRoom.Id);
        }
    }
}
