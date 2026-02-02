using Sobee.Common;
using Sobee.Network.Messaging;
using Sobee.Serialization.GameServer;
using Sobee.TestServer.Match;
using Sobee.TestServer.MatchHelpers;
using Sobee.TestServer.Messages.Chat;

namespace Sobee.TestServer.MatchEvents
{
    public class MatchClientPlayerChatEvent
    {
        private static readonly Serilog.ILogger _log = Logging.Get<MatchClientPlayerChatEvent>();

        public static void ChatPlayerInputReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not MatchRoom matchRoom) return;
            if (e.handler is not MatchPlayer matchPlayer) return;
            if (e.message is not ChatPlayerInputMessage chatInputMessage) return;

            if (matchRoom.MatchInformation == null)
            {
                _log.Warning("[ChatPlayerInputReceived] MatchInformation is null");
                return;
            }

            var playerMatchInfo = matchRoom.MatchInformation.GetPlayer(matchPlayer.Id);
            if (playerMatchInfo == null)
            {
                _log.Warning("[ChatPlayerInputReceived] Chat sender not found (Id={playerId})", matchPlayer.Id);
                return;
            }

            // TODO: [TEST] Isin bitince sil
            switch (chatInputMessage.MessageText)
            {
                case "kickoff":
                    MatchPositioningHelper.ChangePosition(matchRoom, MatchFieldPositioning.Kickoff);
                    return;
                case "phase":
                    matchPlayer.SendMessage(new ChatSystemMessage($"Phase: {matchRoom.MatchInformation.PhaseInfo.MatchPhase}", ChatSystemMessageType.General));
                    return;
                case "actioner":
                    var actionerInfo = matchRoom.MatchInformation.GetPlayer(matchRoom.MatchInformation.BallOwner);
                    matchPlayer.SendMessage(new ChatSystemMessage($"Actioner: {(actionerInfo != null ? actionerInfo.PlayerName : "Unknown")}", ChatSystemMessageType.General));
                    return;
            }

            matchRoom.Players.SendMessage(new ChatPlayerMessage(matchRoom.MatchInformation.SittingIdInfo.GetIdFromSitting(playerMatchInfo.StadiumSitting), playerMatchInfo.PlayerId, chatInputMessage.MessageText, 0));
            _log.Information("[ChatPlayerInputReceived] {playerId}: {messageText}", playerMatchInfo.PlayerId, chatInputMessage);
        }
    }
}
