using Serilog;
using Sobee.Common;
using Sobee.Messaging;
using Sobee.TestServer.Common;

namespace Sobee.TestServer.Events
{
    public class Chat
    {
        private static readonly ILogger _log = Logging.Get<Chat>();

        public static void Messaging(object sender, MessageEventArgs e)
        {
            if (sender is not Hub Hub) return;
            if (e.handler is not Client Client) return;
            if (e.message is not Messages.Chat.ChatMessage Message) return;

            _log.Information($"{Client.Id} - {Message.Text}");
        }
    }
}
