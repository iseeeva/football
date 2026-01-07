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

        public AuthInformationMessage AuthInformation;

        public MatchPlayer(
            SocketWrapper userSocket,
            AuthInformationMessage authInformation,
            MessageCommunication communication
        ) : base(userSocket, communication)
        {
            SessionType = SessionType.User;
            AuthInformation = authInformation;

            communication.AddSessionHandler<HeartbeatMessage>(this, new EventHandler<MessageEventArgs>(GameEvents.MatchClientPlayerEvent.HeartbeatReceived));
            communication.AddSessionHandler<PlayerMoveKeyDownMessage>(this, new EventHandler<MessageEventArgs>(GameEvents.MatchClientPlayerEvent.PlayerMoveKeyDownReceived));
            communication.AddSessionHandler<PlayerMoveKeyUpMessage>(this, new EventHandler<MessageEventArgs>(GameEvents.MatchClientPlayerEvent.PlayerMoveKeyUpReceived));
            communication.AddSessionHandler<ChatMessage>(this, new EventHandler<MessageEventArgs>(GameEvents.MatchClientPlayerChatEvent.ChatMessageReceived));

            _log.Debug("{id} initialized.", Id);
        }

        public override void Update(double delta)
        {
            SendMessage(new LatencyMessage((float)delta));
            base.Update(delta);
        }

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            if (disposing)
            {
                _log.Debug("{id} disposed.", Id);
            }

            base.Dispose(disposing);
        }
    }
}
