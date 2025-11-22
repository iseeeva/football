using Sobee.Common;
using Sobee.Messaging;
using Sobee.TestServer.Match;
using Sobee.TestServer.Messages;
using Sobee.TestServer.Messages.Player;

namespace Sobee.TestServer.GameEvents
{
    public class MatchClientPlayerEvent
    {
        private static readonly Serilog.ILogger _log = Logging.Get<MatchClientPlayerEvent>();

        public static void HeartbeatMessageReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not MatchRoom matchRoom) return;
            if (e.handler is not MatchPlayer matchPlayer) return;
            if (e.message is not HeartbeatMessage heartbeatMessage) return;

            //matchPlayer.SendMessage(new Sobee.TestServer.Messages.HeartbeatMessage(heartbeatMessage.Timestamp));
            //_log.Debug("[HeartbeatMessageReceived] Heartbeat received from {playerId}", matchPlayer.Id);
        }

        public static void PlayerMovePressedReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not MatchRoom matchRoom) return;
            if (e.handler is not MatchPlayer matchPlayer) return;
            if (e.message is not PlayerMovePressed movePressed) return;

            // TODO: You need stamina system
            //      Client.Information.Match.Content.Direction = Received.Content.Velocity

            //Client.Information.Match.Content.Velocity = new Binary.Types.Vector3(
            //  Client.Information.Match.Content.Direction.X * (Received.Content.Sprint ? Moving.Sprint : Moving.Speed),
            //  Client.Information.Match.Content.Direction.Y * (Received.Content.Sprint ? Moving.Sprint : Moving.Speed),
            //  0,
            //)

            //Room.Clients.Broadcast(new Messages.Player.Move({
            //  Alerted: false,
            //  Sprint: Received.Content.Sprint,
            //  Squad: Client.Information.Initialize.Content.Entry.toSquad(),
            //  Position: Client.Information.Match.Content.Position,
            //  Velocity: Client.Information.Match.Content.Velocity,
            //  Stamina: Client.Information.Match.Content.Stamina,
            //}))
            //_log.Debug("[PlayerMovePressedReceived] Player {playerId} moved to {position}.", matchPlayer.Id, movePressed.NewPosition);
        }
    }
}
