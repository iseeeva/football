using Serilog;
using Sobee.Common;

namespace Sobee.System.Common.Events.Common
{
    public class Player
    {
        private static ILogger Log = Logging.Get<Player>();

        public static void Initialize(object sender, MessageEventArgs e)
        {
            var Sender = ((Hub)sender);
            var Client = ((Client)e.handler);
            var Message = ((Messages.Common.Player.Information)e.message);

            Client.Information = Message;
            Client.SendMessage(Messages.Common.Match.Information.testMethod());
            Log.Information($"{Sender.Port} {Client.Id} {Message.ToString()}");
        }
    }
}
