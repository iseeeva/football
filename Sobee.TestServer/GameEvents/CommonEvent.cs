using Serilog;
using Sobee.Common;
using Sobee.Messaging;

namespace Sobee.TestServer.GameEvents
{
    class CommonEvent
    {
        private static readonly ILogger _log = Logging.Get<CommonEvent>();

        public static void Latency(object? sender, MessageEventArgs e)
        {
            //if (sender is not AuthCommunication Hub) return;
            if (e.handler is not Session Session) return;
            //if (e.message is not Messages.Player.PlayerInformation Message) return;

            _log.Information($"[Latency] {Session.Id}");
        }
    }
}
