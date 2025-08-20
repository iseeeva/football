using Serilog;
using Sobee.Common;
using Sobee.Messaging;
using Sobee.TestServer.Common;

namespace Sobee.TestServer.Events
{
    public class Player
    {
        private static readonly ILogger _log = Logging.Get<Player>();

        public static void Information(object sender, MessageEventArgs e)
        {
            if (sender is not Hub Hub) return;
            if (e.handler is not Client Client) return;
            if (e.message is not Messages.Player.PlayerInformation ReceivedInfo) return;

            Client.Information = ReceivedInfo;
            Hub.Clients.Remove(Client);

            var Room = Hub.Rooms.Create();
            Room.Clients.Add(Client);

            //// TODO: Temporary player match information
            //var matchInfo = new Messages.Player.PlayerMatchInformation(
            //    Client.Information.Entry.EntryId,
            //    Client.Information.Entry.EntryId,
            //    $"Test",
            //    new Messages.Player.PlayerAppearance(),
            //    StadiumSitting.HomePlayer,
            //    (sbyte)Client.Information.Entry.ToSquad(true),
            //    new Vector2(0, 0),
            //    new Vector3(0, 0, 0),
            //    new Vector2(0, 0),
            //    MatchCard.None,
            //    "<XMLData><Script></Script></XMLData>"
            //);

            //Room.Information.HomeTeam.Add(matchInfo);

            //Room.Broadcast(new Messages.Player.PlayerJoined(matchInfo));
            //Room.Broadcast(new Messages.Chat.ChatSystemMessage(
            //    $"{matchInfo.PlayerName} connected. ({Room.Clients.Count / 22})",
            //    Messages.Chat.ChatSystemMessageType.SCT
            //));

            //Room.Information.Actor.Camera = (sbyte)Client.Information.Entry.ToSquad(true);
            //Room.Information.Actor.Mark = (sbyte)Client.Information.Entry.EntryId;

            Client.Broadcast(Room.Information);

            _ = Task.Delay(2500).ContinueWith(_ =>
            {
                Client.Broadcast(new Messages.Chat.ChatSystemMessage($"Room: {Room.Id}", Messages.Chat.ChatSystemMessageType.General));
                Client.Broadcast(new Messages.Chat.ChatSystemMessage($"Client: {Client.Id}", Messages.Chat.ChatSystemMessageType.General));
            });

            //_ = Task.Delay(10000).ContinueWith(_ =>
            //{
            //    Room.Clients.Broadcast(new Messages.Chat.ChatSystemMessage("Match Beginning...", Messages.Chat.ChatSystemMessageType.SCT));
            //    MatchFieldPosition.ChangePosition(Room, MatchFieldPositioning.Kickoff);
            //});

            _log.Information($"{Client.Id} - {ReceivedInfo}");
        }

        public static void MovePressed(object sender, MessageEventArgs e)
        {
            if (sender is not Hub Hub) return;
            if (e.handler is not Client Client) return;
            if (e.message is not Messages.Player.PlayerMovePressed Move1) return;
        }
    }
}
