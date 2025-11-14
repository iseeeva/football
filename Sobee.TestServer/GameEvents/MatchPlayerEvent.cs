using Sobee.Common;
using Sobee.Serialization.GameServer;
using Sobee.TestServer.Match;
using Sobee.TestServer.Messages;
using Sobee.TestServer.Messages.Chat;

namespace Sobee.TestServer.GameEvents
{
    public class MatchPlayerEvent
    {
        private static readonly Serilog.ILogger _log = Logging.Get<MatchPlayerEvent>();

        public static void ChatMessageReceived(object? sender, Sobee.Messaging.MessageEventArgs e)
        {
            if (sender is not MatchRoom matchRoom) return;
            if (e.handler is not MatchPlayer matchPlayer) return;
            if (e.message is not ChatMessage chatMessage) return;

            if (matchRoom.MatchInformation == null)
            {
                _log.Warning("[ChatMessageReceived] MatchInformation is null");
                return;
            }

            var playerMatchInfo = matchRoom.MatchInformation.GetPlayer(matchPlayer.Id);
            if (playerMatchInfo == null)
            {
                _log.Warning("[ChatMessageReceived] Chat sender not found (Id={playerId})", matchPlayer.Id);
                return;
            }

            // TODO: [DEVELOPMENT] Isin bitince sil
            switch (chatMessage.MessageText)
            {
                case "kickoff":
                    MatchFieldPosition.ChangePosition(matchRoom, MatchFieldPositioning.Kickoff);
                    return;
            }

            matchRoom.Players.SendMessage(new ChatPlayerMessage(matchRoom.MatchInformation.TeamIdInfo.GetId(playerMatchInfo.StadiumSitting), playerMatchInfo.PlayerId, chatMessage.MessageText, 0));
            _log.Information("[ChatMessageReceived] {playerId}: {messageText}", playerMatchInfo.PlayerId, chatMessage);
        }

        public static void HeartbeatMessageReceived(object? sender, Sobee.Messaging.MessageEventArgs e)
        {
            if (sender is not MatchRoom matchRoom) return;
            if (e.handler is not MatchPlayer matchPlayer) return;
            if (e.message is not HeartbeatMessage heartbeatMessage) return;

            //matchPlayer.SendMessage(new Sobee.TestServer.Messages.HeartbeatMessage(heartbeatMessage.Timestamp));
            //_log.Debug("[HeartbeatMessageReceived] Heartbeat received from {playerId}", matchPlayer.Id);
        }
    }
}
