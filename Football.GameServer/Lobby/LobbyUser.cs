using Football.Database;
using Football.GameServer.Game;
using Football.GameServer.Messages.Auth;
using Football.Network;
using Football.Network.Messaging;

namespace Football.GameServer.Lobby
{
    public class LobbyUser : GameUser
    {
        public MatchClientAuthInfoRxMessage? AuthInformation;

        public LobbyUser(SocketWrapper userSocket, MessageCommunication communication)
            : base(userSocket, communication)
        {

        }

        public async Task<ServerBrowserDbContext.UserContext?> GetDbUserDetailsAsync()
        {
            if (Socket == null)
                return null;

            using var context = new ServerBrowserDbContext();
            var session = await context.GetSessionBySocketAsync(Socket.Id);
            if (session == null)
                return null;

            var user = await context.GetUserAsync(session.UserId);
            return user;
        }

        #region Dispose
        protected override void OnDispose()
        {
            AuthInformation = null;

            Task.Run(async () =>
            {
                if (Socket == null)
                    return;

                using var context = new ServerBrowserDbContext();
                var session = await context.GetSessionBySocketAsync(Socket.Id);
                if (session == null)
                    return;

                var isRemoved = await context.RemoveSessionAsync(session.Id);
                if (isRemoved)
                    _log.Information("session succesfully removed from db linkage");
                else
                    _log.Error("session can't removed from db linkage");
            });

            base.OnDispose();
        }
        #endregion
    }
}