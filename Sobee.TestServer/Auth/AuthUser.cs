using Serilog;
using Sobee.Common;
using Sobee.Network;
using Sobee.TestServer.Messages.Auth;

namespace Sobee.TestServer.Auth
{
    public class AuthUser : User
    {
        private static readonly ILogger _log = Logging.Get<AuthUser>();
        private bool _isDisposed;

        public AuthInformation? AuthInformation;
        // public object DatabaseRecord; // TODO: Placeholder for the actual database record type

        public AuthUser(
            SocketWrapper userSocket,
            Communication communication
        ) : base(userSocket, communication)
        {
            SessionType = SessionType.User;
            //userComm.AddSessionHandler<Messages.Auth.AuthInformation>(this, new EventHandler<MessageEventArgs>(GameEvents.TestEvent.SessionTest));
            _log.Debug("{id} initialized.", Id);
        }

        //public bool TryFetchFromDatabase()
        //{
        //    // TODO: Implement database fetching logic here.
        //    return true;
        //}

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
