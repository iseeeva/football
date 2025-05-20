using Serilog;
using Sobee.Common;
using Sobee.Messaging;

namespace Sobee.System.Common.Events
{
    public class Player
    {
        private static ILogger log = Logging.Get<Player>();

        public static void Information(object sender, MessageEventArgs e)
        {
            var Sender = (Hub)sender;
            var Client = (Client)e.handler;
            var Message = (Messages.Common.Player.Information)e.message;

            Client.Information = Message;
            Sender.Clients.Remove(Client);

            var roomTest = Sender.Rooms.Create();
            roomTest.Clients.Add(Client);

            Client.Broadcast(new Messages.Common.Match.Information());
            log.Information($"{Sender.Port} {Client.Id} {Message.ToString()}");
        }
    }
}
