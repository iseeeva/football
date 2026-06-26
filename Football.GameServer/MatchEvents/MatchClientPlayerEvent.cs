using Football.Common;
using Football.GameServer.Match;
using Football.GameServer.Messages.Chat;
using Football.GameServer.Messages.Player;
using Football.Network.Messaging;

namespace Football.GameServer.MatchEvents
{
    public class MatchClientPlayerEvent
    {
        private static readonly Serilog.ILogger _log = LogFactory.GetContextForType<MatchClientPlayerEvent>();

        public static void PlayerHeartbeatReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not MatchRoom matchRoom) return;
            if (e.Handler is not MatchUser matchPlayer) return;
            if (e.Message is not PlayerHeartbeatRxMessage heartbeatMessage) return;

            //matchPlayer.SendMessage(new Sobee.TestServer.Messages.HeartbeatMessage(heartbeatMessage.Timestamp));
            //_log.Debug("[HeartbeatMessageReceived] Heartbeat received from {playerId}", matchPlayer.Id);
        }

        public static void PlayerMatchStateAlertReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not MatchRoom matchRoom) return;
            if (e.Handler is not MatchUser matchPlayer) return;
            if (e.Message is not PlayerMatchStateAlertRxMessage matchAlert) return;

            // TODO: Ek kontroller gerekebilir.
            matchRoom.Players.SendMessage(new ChatSystemTextTxMessage($"[MatchStateAlertReceived] Player {matchPlayer.Id} reported his match state is changed.", ChatSystemMessageType.General));
        }
    }
}
