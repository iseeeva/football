using Sobee.Common;
using Sobee.TestServer.Match;
using Sobee.TestServer.Messages;

namespace Sobee.TestServer.GameEvents
{
    public class MatchClientPlayerEvent
    {
        private static readonly Serilog.ILogger _log = Logging.Get<MatchClientPlayerEvent>();

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
