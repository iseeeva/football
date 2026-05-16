using Serilog;
using Sobee.Common;
using Sobee.Network;
using Sobee.Network.Messaging;
using Sobee.TestServer.Messages.Auth;

namespace Sobee.TestServer.AuthEvents
{
    public class AuthClientTestEvent
    {
        private static readonly ILogger _log = LogFactory.GetContextForType<AuthClientTestEvent>();

        public static void GlobalTest(object? sender, MessageEventArgs e)
        {
            //if (sender is not AuthCommunication Hub) return;
            if (e.Handler is not Session Session) return;
            if (e.Message is not AuthInformationRxMessage Message) return;

            _log.Information($"[GlobalTest] {Session.Id} - {Message}");
        }

        public static void SessionTest(object? sender, MessageEventArgs e)
        {
            //if (sender is not AuthCommunication Hub) return;
            if (e.Handler is not Session Session) return;
            if (e.Message is not AuthInformationRxMessage Message) return;

            _log.Information($"[SessionTest] {Session.Id} - {Message}");
        }
    }
}
