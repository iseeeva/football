using Sobee.Common;
using Sobee.Network.Messaging;
using Sobee.TestServer.Match;
using Sobee.TestServer.Messages.Player;

namespace Sobee.TestServer.MatchEvents
{
    public class MatchClientPlayerEvent
    {
        private static readonly Serilog.ILogger _log = Logging.Get<MatchClientPlayerEvent>();

        public static void PlayerHeartbeatReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not MatchRoom matchRoom) return;
            if (e.handler is not MatchPlayer matchPlayer) return;
            if (e.message is not PlayerHeartbeatRxMessage heartbeatMessage) return;

            //matchPlayer.SendMessage(new Sobee.TestServer.Messages.HeartbeatMessage(heartbeatMessage.Timestamp));
            //_log.Debug("[HeartbeatMessageReceived] Heartbeat received from {playerId}", matchPlayer.Id);
        }

        public static void PlayerMatchStateAlertReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not MatchRoom matchRoom) return;
            if (e.handler is not MatchPlayer matchPlayer) return;
            if (e.message is not PlayerMatchStateAlertRxMessage matchAlert) return;

            // TODO: Ek kontroller gerekebilir.
            matchRoom.Players.SendMessage(new Messages.Chat.ChatSystemTextTxMessage($"[MatchStateAlertReceived] Player {matchPlayer.Id} reported his match state is changed.", Messages.Chat.ChatSystemMessageType.General));
        }
    }
}
