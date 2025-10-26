using Serilog;
using Sobee.Common;
using Sobee.Network;
using Sobee.TestServer.Messages.Auth;

namespace Sobee.TestServer.Match
{
    public class MatchPlayer : User
    {
        private static readonly ILogger _log = Logging.Get<MatchPlayer>();
        private bool _isDisposed;

        public AuthInformation? AuthInformation;

        public MatchPlayer(
            AuthInformation authInformation,
            SocketWrapper userSocket,
            Communication communication
        ) : base(userSocket, communication)
        {
            SessionType = SessionType.User;
            AuthInformation = authInformation;

            //userComm.AddSessionHandler<Messages.Auth.AuthInformation>(this, new EventHandler<MessageEventArgs>(GameEvents.TestEvent.SessionTest));
            _log.Debug("{id} initialized.", Id);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                if (disposing)
                {
                    _log.Debug("{id} disposing.", Id);

                    _log.Debug("{id} disposed.", Id);
                }
            }

            base.Dispose(disposing);
        }
    }
}
