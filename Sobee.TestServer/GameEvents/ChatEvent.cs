using Serilog;
using Sobee.Common;
using Sobee.Messaging;

namespace Sobee.TestServer.GameEvents
{
    public class ChatEvent
    {
        private static readonly ILogger _log = Logging.Get<ChatEvent>();

        public static void Messaging(object sender, MessageEventArgs e)
        {
            if (sender is not Hub Hub) return;
            if (e.handler is not Player Client) return;
            if (e.message is not Messages.Chat.ChatMessage Message) return;

            _log.Information($"{Client.Id} - {Message.Text}");
        }
    }
}
