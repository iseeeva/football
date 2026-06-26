using Football.Common;
using Football.GameServer.Game;
using Football.GameServer.Lobby;
using Football.GameServer.Messages.Auth;
using Football.Network.Messaging;
using Serilog;

namespace Football.GameServer.LobbyEvents
{
    public class MatchClientAuthEvent
    {
        private static readonly ILogger _log = LogFactory.GetContextForType<MatchClientAuthEvent>();

        public static async void AuthInformationReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not LobbyRoom authRoom) return;
            if (e.Handler is not LobbyUser authUser) return;
            if (e.Message is not MatchClientAuthInfoRxMessage authInformation) return;

            if (authRoom.Owner is not GameHub hub) return;

            if (authUser.Socket == null)
            {
                _log.Error("[AuthInformationReceived] Session ({sessionId}) socket is null.", authUser.Id);
                return;
            }

            var authDbUser = await authUser.GetDbUserDetailsAsync();
            if (authDbUser == null)
            {
                _log.Error("[AuthInformationReceived] Session ({sessionId}) doesn't have user linkage from database.", authUser.Id);
                return;
            }

            // TODO: Process auth information here when database integration is done
            authUser.AuthInformation = authInformation;

            // TODO: Remove this temporary code after implementing proper matchmaking flow
            #region Temporary
            //if (hub.RoomManager.TryCreate(out var matchRoom))
            //{
            //    if (!matchRoom.Players.TryCreate(authUser))
            //        _log.Error("[AuthInformationReceived] Failed to add auth user {authId} to match room.", authUser.Id);
            //}
            #endregion

            _log.Information($"{authUser.Id} - {authInformation}");
        }
    }
}
