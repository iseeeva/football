using Serilog;
using Sobee.Common;
using Sobee.Messaging;
using Sobee.Network;
using Sobee.TestServer.Messages;
using Sobee.TestServer.Messages.Auth;
using Sobee.TestServer.Messages.Chat;

namespace Sobee.TestServer.Match
{
    public class MatchPlayer : User
    {
        private static readonly ILogger _log = Logging.Get<MatchPlayer>();
        private bool _isDisposed;

        public AuthInformation? AuthInformation;

        // Client, mac ekranina geldiginda true olacak.
        // Maci etkileyen baska birsey yapilmadigi surece true kalacak.
        public bool IsReadyForMatch;

        public MatchPlayer(
            AuthInformation authInformation,
            SocketWrapper userSocket,
            Communication communication
        ) : base(userSocket, communication)
        {
            SessionType = SessionType.User;
            AuthInformation = authInformation;

            communication.AddSessionHandler<HeartbeatMessage>(this, new EventHandler<MessageEventArgs>(GameEvents.MatchPlayerEvent.HeartbeatMessageReceived));
            communication.AddSessionHandler<ChatMessage>(this, new EventHandler<MessageEventArgs>(GameEvents.MatchPlayerEvent.ChatMessageReceived));

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
