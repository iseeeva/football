using Sobee.Common;
using Sobee.TestServer.Match;
using Sobee.TestServer.Messages;

namespace Sobee.TestServer.GameEvents
{
    public class MatchRoomEvent
    {
        private static readonly Serilog.ILogger _log = Logging.Get<MatchRoomEvent>();

        public static void PlayerJoined(MatchRoom matchRoom, MatchPlayer matchPlayer)
        {
            // TODO: Additional logic for when a player joins can be added here.

            if (matchRoom == null || (matchPlayer == null || matchPlayer.AuthInformation == null))
            {
                _log.Warning("[PlayerJoined] matchRoom or matchPlayer is null.");
                return;
            }

            var matchInformation = matchRoom.MatchInformation.GetPlayer(matchPlayer.Id);
            if (matchInformation == null)
            {
                _log.Warning("[PlayerJoined] MatchPlayer {playerId} not found in MatchRoom {matchId}.", matchPlayer.Id, matchRoom.Id);
                matchRoom.Players.TryRemovePlayer(matchPlayer.Id);
                return;
            }

            matchRoom.Players.SendMessage(new Messages.Player.PlayerJoined(matchInformation));
            matchRoom.Players.SendMessage(new Messages.Chat.ChatSystemMessage(
                $"{matchInformation.PlayerName} connected. (total player: {matchRoom.Players.Count})",
                Messages.Chat.ChatSystemMessageType.SCT
            ));

            // TODO: Set camera and mark to the joining player for their own view
            matchRoom.MatchInformation.Actor.Camera = (sbyte)matchPlayer.AuthInformation.Entry.ToSquad(false);
            matchRoom.MatchInformation.Actor.Mark = matchPlayer.AuthInformation.Entry.EntryNumber;
            matchPlayer.SendMessage(matchRoom.MatchInformation);

            _log.Information("[PlayerJoined] MatchPlayer {playerId} joined to MatchRoom {matchId}.", matchPlayer.Id, matchRoom.Id);

            // Wait for heartbeat message before invoke the match event.
            // Heartbeat is the first message sent by the client after going match screen.
            _ = matchPlayer.WaitForMessage(s => s is HeartbeatMessage).ContinueWith(_ =>
            {
                matchPlayer.SendMessage(new Messages.Chat.ChatSystemMessage($"[PlayerJoined] Room Id: {matchRoom.Id}", Messages.Chat.ChatSystemMessageType.General));
                matchPlayer.SendMessage(new Messages.Chat.ChatSystemMessage($"[PlayerJoined] Your Id: {matchPlayer.Id}", Messages.Chat.ChatSystemMessageType.General));
                matchPlayer.SendMessage(new Messages.Chat.ChatSystemMessage($"[PlayerJoined] (SessionType: {matchPlayer.SessionType})", Messages.Chat.ChatSystemMessageType.General));
                matchPlayer.SendMessage(new Messages.Chat.ChatSystemMessage($"[PlayerJoined] {matchInformation}", Messages.Chat.ChatSystemMessageType.General));
                matchPlayer.IsReadyForMatch = true;
                //MatchBeginning(matchRoom);
            });
        }

        //public static void MatchBeginning(MatchRoom matchRoom)
        //{
        //    if (matchRoom == null)
        //    {
        //        _log.Warning("[MatchBeginning] matchRoom is null.");
        //        return;
        //    }

        //    _log.Information($"[MatchBeginning] invoked.");
        //}
    }
}
