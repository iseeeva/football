using Sobee.Common;
using Sobee.Network.Messaging;
using Sobee.TestServer.Match;
using Sobee.TestServer.Messages.Match;

namespace Sobee.TestServer.GameEvents
{
    public class MatchClientEvent
    {
        private static readonly Serilog.ILogger _log = Logging.Get<MatchClientEvent>();

        public static void MatchStateAlertReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not MatchRoom matchRoom) return;
            if (e.handler is not MatchPlayer matchPlayer) return;
            if (e.message is not MatchStateAlertMessage matchAlert) return;

            // TODO: Ek kontroller gerekebilir.
            matchRoom.Players.SendMessage(new Messages.Chat.ChatSystemMessage($"[MatchStateAlertReceived] Player {matchPlayer.Id} reported his match state is changed.", Messages.Chat.ChatSystemMessageType.General));
        }
    }
}
