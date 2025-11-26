using Serilog;
using Sobee.Common;
using Sobee.Network;
using Sobee.Network.Messaging;
using Sobee.TestServer.Messages;
using Sobee.TestServer.Messages.Auth;
using Sobee.TestServer.Messages.Chat;
using Sobee.TestServer.Messages.Player;

namespace Sobee.TestServer.Match
{
    public class MatchPlayer : User
    {
        private static readonly ILogger _log = Logging.Get<MatchPlayer>();
        private bool _isDisposed;

        public AuthInformation AuthInformation;

        // Client, mac ekranina geldiginda true olacak.
        // Maci etkileyen baska birsey yapilmadigi surece true kalacak.
        public bool IsReadyForMatch;

        public MatchPlayer(
            SocketWrapper userSocket,
            AuthInformation authInformation,
            MessageCommunication communication
        ) : base(userSocket, communication)
        {
            SessionType = SessionType.User;
            AuthInformation = authInformation;

            communication.AddSessionHandler<HeartbeatMessage>(this, new EventHandler<MessageEventArgs>(GameEvents.MatchClientPlayerEvent.HeartbeatMessageReceived));
            communication.AddSessionHandler<PlayerMoveKeyDown>(this, new EventHandler<MessageEventArgs>(GameEvents.MatchClientPlayerEvent.PlayerMoveKeyDownReceived));
            communication.AddSessionHandler<PlayerMoveKeyUp>(this, new EventHandler<MessageEventArgs>(GameEvents.MatchClientPlayerEvent.PlayerMoveKeyUpReceived));
            communication.AddSessionHandler<ChatMessage>(this, new EventHandler<MessageEventArgs>(GameEvents.MatchClientPlayerChatEvent.ChatMessageReceived));

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
