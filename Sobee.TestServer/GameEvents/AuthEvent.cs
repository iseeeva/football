using Serilog;
using Sobee.Common;
using Sobee.Messaging;

namespace Sobee.TestServer.GameEvents
{
    public class AuthEvent
    {
        private static readonly ILogger _log = Logging.Get<AuthEvent>();

        public static void AuthInformation(object? sender, MessageEventArgs e)
        {
            //if (sender is not Hub Hub) return;
            if (e.handler is not Session Session) return;
            if (e.message is not Messages.Player.PlayerInformation Message) return;

            _log.Information($"{Session.Id} - {Message}");
        }
    }
}
