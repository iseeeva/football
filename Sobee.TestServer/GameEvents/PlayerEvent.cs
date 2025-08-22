using Serilog;
using Sobee.Common;
using Sobee.Messaging;

namespace Sobee.TestServer.GameEvents
{
    public class PlayerEvent
    {
        private static readonly ILogger _log = Logging.Get<PlayerEvent>();

        public static void Information(object? sender, MessageEventArgs e)
        {
            if (sender is not Hub Hub) return;
            if (e.handler is not Player Client) return;
            if (e.message is not Messages.Player.PlayerInformation ReceivedInfo) return;

            Client.Information = ReceivedInfo;
            //Hub.Players.Remove(Client);

            //var Room = Hub.Rooms.Create();
            //Room.Players.Add(Client._socket);

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

            //client.sendmessage(room.ınformation);

            //_ = task.delay(2500).continuewith(_ =>
            //{
            //    client.sendmessage(new messages.chat.chatsystemmessage($"room: {room.ıd}", messages.chat.chatsystemmessagetype.general));
            //    client.sendmessage(new messages.chat.chatsystemmessage($"client: {client.ıd}", messages.chat.chatsystemmessagetype.general));
            //});

            //_ = Task.Delay(10000).ContinueWith(_ =>
            //{
            //    Room.Clients.Broadcast(new Messages.Chat.ChatSystemMessage("Match Beginning...", Messages.Chat.ChatSystemMessageType.SCT));
            //    MatchFieldPosition.ChangePosition(Room, MatchFieldPositioning.Kickoff);
            //});

            _log.Information($"{Client.Id} - {ReceivedInfo}");
        }

        public static void MovePressed(object? sender, MessageEventArgs e)
        {
            if (sender is not Hub Hub) return;
            if (e.handler is not Player Client) return;
            if (e.message is not Messages.Player.PlayerMovePressed Move1) return;
        }
    }
}
