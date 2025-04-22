using Serilog;
using Sobee.Common;

namespace Sobee.System.Common.Events.Common
{
    public class Player
    {
        private static ILogger Log = Logging.Get<Player>();

        public static void Initialize(object sender, MessageEventArgs e)
        {
            Log.Information($"{((Hub)sender).Port}");
        }
    }
}
