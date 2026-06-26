using Football.Common;
using Football.Database;
using Football.GameServer.Game;
using Football.GameServer.Lobby;
using Football.GameServer.Match;
using Football.Network.Messaging;
using Football.ServerBrowser.Messages;
using Serilog;

namespace Football.GameServer.LobbyEvents
{
    public class BrowserClientInfoEvent
    {
        private static readonly ILogger _log = LogFactory.GetContextForType<BrowserClientInfoEvent>();

        public static async void ServerListRequestReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not LobbyRoom authRoom) return;
            if (e.Handler is not LobbyUser authUser) return;
            if (e.Message is not BrowserServerListRequestMessage authInformation) return;

            if (authRoom.Owner is not GameHub hub) return;

            if (authUser.Socket == null)
            {
                _log.Error("[ServerListRequestReceived] Session ({sessionId}) socket is null.", authUser.Id);
                return;
            }

            var authDbUser = await authUser.GetDbUserDetailsAsync();
            if (authDbUser == null)
            {
                _log.Error("[ServerListRequestReceived] Session ({sessionId}) doesn't have user linkage from database.", authUser.Id);
                return;
            }

            var rooms = hub.RoomManager.GetAll();
            var list = rooms
                .Where(x => x != null)
                .ToList();

            List<BrowserServerListItem> test = new List<BrowserServerListItem>();
            using var t = new ServerBrowserDbContext();
            foreach (var item in list)
            {
                var ta = await t.GetMatchByMatchIdAsync(item.Id);
                if (ta == null)
                    continue;

                test.Add(new BrowserServerListItem(item.Id, ta.MatchName, item.MatchInformation));
            }

            authUser.SendMessage(new BrowserServerListResponseMessage(test));
        }

        public static async void UserDetailsRequestReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not LobbyRoom authRoom) return;
            if (e.Handler is not LobbyUser authUser) return;
            if (e.Message is not BrowserUserDetailsRequestMessage authInformation) return;

            if (authRoom.Owner is not GameHub hub) return;

            if (authUser.Socket == null)
            {
                _log.Error("[ServerListRequestReceived] Session ({sessionId}) socket is null.", authUser.Id);
                return;
            }

            var authDbUser = await authUser.GetDbUserDetailsAsync();
            if (authDbUser == null)
            {
                _log.Error("[ServerListRequestReceived] Session ({sessionId}) doesn't have user linkage from database.", authUser.Id);
                return;
            }

            authUser.SendMessage(new BrowserUserDetailsResponseMessage(authDbUser.PlayerName, authDbUser.UserName));
        }

        public static async void BrowserCreateMatchRequestReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not LobbyRoom authRoom) return;
            if (e.Handler is not LobbyUser authUser) return;
            if (e.Message is not BrowserCreateMatchRequestMessage createMatchRequest) return;

            if (authRoom.Owner is not GameHub hub) return;

            if (authUser.Socket == null)
            {
                _log.Error("[BrowserCreateMatchRequestReceived] Session ({sessionId}) socket is null.", authUser.Id);
                return;
            }

            var authDbUser = await authUser.GetDbUserDetailsAsync();
            if (authDbUser == null)
            {
                _log.Error("[BrowserCreateMatchRequestReceived] Session ({sessionId}) doesn't have user linkage from database.", authUser.Id);
                return;
            }

            var _genMatchRoom = new MatchRoom();

            var _genMatchInfo = _genMatchRoom.MatchInformation;
            _genMatchInfo.HomePlayer.Capacity = createMatchRequest.HomeCapacity;
            _genMatchInfo.AwayPlayer.Capacity = createMatchRequest.AwayCapacity;

            var _genScenario = _genMatchInfo.ScenarioInfo;
            _genScenario.ScenarioType = createMatchRequest.ScenarioType;
            _genScenario.team1Size = createMatchRequest.HomeCapacity;
            _genScenario.team1Name = createMatchRequest.HomeFullName;
            _genScenario.team1ShortName = createMatchRequest.HomeShortName;
            _genScenario.team2Size = createMatchRequest.AwayCapacity;
            _genScenario.team2Name = createMatchRequest.AwayFullName;
            _genScenario.team2ShortName = createMatchRequest.AwayShortName;


            var test232 = hub.RoomManager.TryAdd(_genMatchRoom);
            if (test232)
            {
                using var db = new ServerBrowserDbContext();
                await db.AddMatchAsync(new ServerBrowserDbContext.MatchContext(authDbUser.Id, _genMatchRoom.Id, createMatchRequest.MatchName, createMatchRequest.MatchPassword));

                authUser.SendMessage(new BrowserStatusResponseMessage(true, "Match successfully created."));
                _log.Information("[BrowserCreateMatchRequestReceived] New match ({matchId}) created for session ({userId}).", _genMatchRoom.Id, authUser.Id);
            }
            else
            {
                authUser.SendMessage(new BrowserStatusResponseMessage(false, "Match creation failed."));
                _log.Error("[BrowserCreateMatchRequestReceived] New match creation for session ({userId}) is failed.", authUser.Id);
            }
        }
    }
}
