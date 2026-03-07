using Serilog;
using Sobee.Common;
using Sobee.Network;
using Sobee.Network.Messaging;
using Sobee.TestServer.AuthEvents;

namespace Sobee.TestServer.Auth
{
    public class AuthRoom : Room<AuthUser>
    {
        private static readonly ILogger _log = Logging.Get<AuthRoom>();
        private bool _isDisposed;

        public AuthUserManager Users => (AuthUserManager)_sessions;
        public readonly Hub ConnectedHub;

        public AuthRoom(Hub connectedHub) : base()
        {
            _log.Debug("{id} initializing.", Id);
            CommunicationType = SessionType.Authentication;
            ConnectedHub = connectedHub;

            // Bu handler ın bütün sessionlardan gelen mesajları işlemesi gerekiyor.
            RegisterMessageEvent<Messages.Auth.AuthInformationRxMessage>(OnReceivedMessage);
            AddGlobalHandler<Messages.Auth.AuthInformationRxMessage>(new EventHandler<MessageEventArgs>(AuthClientEvent.AuthInformationReceived));

            _log.Debug("{id} initialized.", Id);
        }

        protected override SessionManager<AuthUser> CreateSessionManager()
        {
            return new AuthUserManager(this);
        }

        public override void Update(double delta)
        {
            if (_sessions != null)
            {
                Users.Update(delta);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                if (disposing)
                {
                    _log.Debug("{id} disposed.", Id);
                }
            }

            base.Dispose(disposing);
        }
    }
}
