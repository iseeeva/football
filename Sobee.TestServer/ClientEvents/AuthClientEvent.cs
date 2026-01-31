using Serilog;
using Sobee.Common;
using Sobee.Network.Messaging;
using Sobee.TestServer.Auth;
using Sobee.TestServer.Messages.Auth;

namespace Sobee.TestServer.ClientEvents
{
    public class AuthClientEvent
    {
        private static readonly ILogger _log = Logging.Get<AuthClientEvent>();

        public static void AuthInformationReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not AuthRoom authRoom) return;
            if (e.handler is not AuthUser authUser) return;
            if (e.message is not AuthInformationMessage authInformation) return;

            if (authRoom.ConnectedHub is not Hub hub) return;

            // TODO: Process auth information here
            authUser.AuthInformation = authInformation;

            #region Temporary
            if (authUser.Socket == null)
            {
                _log.Error("[TEMPORARY] Auth session socket is null.");
                return;
            }

            if (hub.RoomManager.TryCreate(out var matchRoom))
            {
                if (!matchRoom.Players.TryCreate(authUser))
                    _log.Error("[TEMPORARY] Failed to add auth user {authId} to match room.", authUser.Id);
            }
            #endregion

            _log.Information($"{authUser.Id} - {authInformation}");
        }
    }
}
