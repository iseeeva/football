using Serilog;
using Sobee.Common;
using Sobee.Messaging;
using Sobee.TestServer.Auth;
using Sobee.TestServer.Messages.Auth;

namespace Sobee.TestServer.GameEvents
{
    public class AuthEvent
    {
        private static readonly ILogger _log = Logging.Get<AuthEvent>();

        public static void AuthInformation(object? sender, MessageEventArgs e)
        {
            if (sender is not AuthRoom authRoom) return;
            if (e.handler is not AuthUser authUser) return;
            if (e.message is not AuthInformation authInformation) return;

            if (authRoom.ConnectedHub is not Hub hub) return;

            // TODO: Process auth information here
            authUser.AuthInformation = authInformation;

            #region Temporary
            if (authUser.Socket == null)
            {
                _log.Error("[TEMPORARY] Auth session socket is null.");
                return;
            }

            var matchRoom = hub.MatchRoomManager.Create();
            if (!matchRoom.Players.TryAddPlayer(authUser))
                _log.Error("[TEMPORARY] Failed to add auth user {authId} to match room.", authUser.Id);
            #endregion

            _log.Information($"{authUser.Id} - {authInformation}");
        }
    }
}
