using Serilog;
using Sobee.Common;

namespace Sobee.System.Common.Events
{
    public class Chat
    {
        private static ILogger log = Logging.Get<Chat>();

        public static void Messaging(object sender, MessageEventArgs e)
        {
            var Sender = (Hub)sender;
            var Client = (Client)e.handler;
            var Message = (Messages.Common.Chat.Messaging)e.message;

            log.Information($"{Client.Id} {Message}");
        }
    }
}
