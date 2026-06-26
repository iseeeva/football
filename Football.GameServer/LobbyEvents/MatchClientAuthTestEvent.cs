using Football.Common;
using Football.GameServer.Messages.Auth;
using Football.Network;
using Football.Network.Messaging;
using Serilog;

namespace Football.GameServer.LobbyEvents
{
    public class MatchClientAuthTestEvent
    {
        private static readonly ILogger _log = LogFactory.GetContextForType<MatchClientAuthTestEvent>();

        public static void GlobalTest(object? sender, MessageEventArgs e)
        {
            //if (sender is not AuthCommunication Hub) return;
            if (e.Handler is not Session Session) return;
            if (e.Message is not MatchClientAuthInfoRxMessage Message) return;

            _log.Information($"[GlobalTest] {Session.Id} - {Message}");
        }

        public static void SessionTest(object? sender, MessageEventArgs e)
        {
            //if (sender is not AuthCommunication Hub) return;
            if (e.Handler is not Session Session) return;
            if (e.Message is not MatchClientAuthInfoRxMessage Message) return;

            _log.Information($"[SessionTest] {Session.Id} - {Message}");
        }
    }
}
