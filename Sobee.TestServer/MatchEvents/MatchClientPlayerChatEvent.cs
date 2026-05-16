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
        private static readonly Serilog.ILogger _log = LogFactory.GetContextForType<MatchClientPlayerChatEvent>();

        public static void ChatPlayerInputReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not MatchRoom matchRoom) return;
            if (e.Handler is not MatchPlayer matchPlayer) return;
            if (e.Message is not ChatPlayerTextRxMessage chatPlayerMessage) return;

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
            switch (chatPlayerMessage.MessageText)
            {
                case "kickoff":
                    MatchPositioningHelper.ChangePosition(matchRoom, MatchFieldPositioning.Kickoff);
                    return;
                case "phase":
                    matchPlayer.SendMessage(new ChatSystemTextTxMessage($"Phase: {matchRoom.MatchInformation.PhaseInfo.MatchPhase}", ChatSystemMessageType.General));
                    return;
                case "actioner":
                    var actionerInfo = matchRoom.MatchInformation.GetPlayer(matchRoom.MatchInformation.BallOwner);
                    matchPlayer.SendMessage(new ChatSystemTextTxMessage($"Actioner: {(actionerInfo != null ? actionerInfo.PlayerName : "Unknown")}", ChatSystemMessageType.General));
                    return;
            }

            // "chatInputMessage.TeamId" yada "matchRoom.MatchInformation.SittingIdInfo.GetIdFromSitting(playerMatchInfo.StadiumSitting)"
            matchRoom.Players.SendMessage(new ChatPlayerTextTxMessage(chatPlayerMessage.TeamId, playerMatchInfo.PlayerId, chatPlayerMessage.MessageText, 0));
            _log.Information("[ChatPlayerInputReceived] {playerId}: {messageText}", playerMatchInfo.PlayerId, chatPlayerMessage);
        }
    }
}
