using Serilog;
using Sobee.Common;
using Sobee.Network.Messaging;
using Sobee.TestServer.Auth;
using Sobee.TestServer.Messages.Auth;

namespace Sobee.TestServer.AuthEvents
{
    public class AuthClientEvent
    {
        private static readonly ILogger _log = LogFactory.GetContextForType<AuthClientEvent>();

        public static void AuthInformationReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not AuthRoom authRoom) return;
            if (e.Handler is not AuthUser authUser) return;
            if (e.Message is not AuthInformationRxMessage authInformation) return;

            if (authRoom.Owner is not Hub hub) return;

            // TODO: Process auth information here when database integration is done
            authUser.AuthInformation = authInformation;

            // TODO: Remove this temporary code after implementing proper matchmaking flow
            #region Temporary
            if (authUser.Socket == null)
            {
                _log.Error("[AuthInformationReceived] Auth session socket is null.");
                return;
            }

            if (hub.RoomManager.TryCreate(out var matchRoom))
            {
                if (!matchRoom.Players.TryCreate(authUser))
                    _log.Error("[AuthInformationReceived] Failed to add auth user {authId} to match room.", authUser.Id);
            }
            #endregion

            _log.Information($"{authUser.Id} - {authInformation}");
        }
    }
}
