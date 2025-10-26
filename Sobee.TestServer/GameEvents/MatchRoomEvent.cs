using Sobee.Common;
using Sobee.TestServer.Match;

namespace Sobee.TestServer.GameEvents
{
    public class MatchRoomEvent
    {
        public static Serilog.ILogger _log = Logging.Get<MatchRoomEvent>();

        public static void PlayerJoined(MatchRoom matchRoom, MatchPlayer matchPlayer)
        {
            // TODO: Additional logic for when a player joins can be added here.
            _log.Information("Player {playerId} joined MatchRoom {roomId}.", matchPlayer.Id, matchRoom.Id);
        }
    }
}
