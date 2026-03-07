using Serilog;
using Sobee.Common;
using Sobee.Network;
using Sobee.Network.Messaging;
using Sobee.TestServer.Messages.Auth;

namespace Sobee.TestServer.AuthEvents
{
    public class AuthClientTestEvent
    {
        private static readonly ILogger _log = Logging.Get<AuthClientTestEvent>();

        public static void GlobalTest(object? sender, MessageEventArgs e)
        {
            //if (sender is not AuthCommunication Hub) return;
            if (e.handler is not Session Session) return;
            if (e.message is not AuthInformationRxMessage Message) return;

            _log.Information($"[GlobalTest] {Session.Id} - {Message}");
        }

        public static void SessionTest(object? sender, MessageEventArgs e)
        {
            //if (sender is not AuthCommunication Hub) return;
            if (e.handler is not Session Session) return;
            if (e.message is not AuthInformationRxMessage Message) return;

            _log.Information($"[SessionTest] {Session.Id} - {Message}");
        }
    }
}
