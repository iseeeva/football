using Sobee.Common;
using Sobee.Messaging;
using Sobee.TestServer.Match;
using Sobee.TestServer.Messages.Match;

namespace Sobee.TestServer.GameEvents
{
    public class MatchClientEvent
    {
        private static readonly Serilog.ILogger _log = Logging.Get<MatchClientEvent>();

        public static void MatchRunningAlertReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not MatchRoom matchRoom) return;
            if (e.handler is not MatchPlayer matchPlayer) return;
            if (e.message is not MatchRunningAlert matchAlert) return;

            // TODO: Ek kontroller gerekebilir.
            matchRoom.Players.SendMessage(new Messages.Chat.ChatSystemMessage($"[MatchRunningAlertReceived] Player {matchPlayer.Id} reported his match is running.", Messages.Chat.ChatSystemMessageType.General));
        }
    }
}
