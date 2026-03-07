using Serilog;
using Sobee.Common;
using Sobee.Network;
using Sobee.Network.Messaging;
using Sobee.TestServer.MatchEvents;
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

        public AuthInformationRxMessage AuthInformation;

        public MatchPlayer(
            SocketWrapper userSocket,
            AuthInformationRxMessage authInformation,
            MessageCommunication communication
        ) : base(userSocket, communication)
        {
            SessionType = SessionType.User;
            AuthInformation = authInformation;

            communication.AddSessionHandler<PlayerHeartbeatRxMessage>(this, new EventHandler<MessageEventArgs>(MatchClientPlayerEvent.PlayerHeartbeatReceived));
            communication.AddSessionHandler<PlayerMatchStateAlertRxMessage>(this, new EventHandler<MessageEventArgs>(MatchClientPlayerEvent.PlayerMatchStateAlertReceived));
            communication.AddSessionHandler<PlayerMoveKeyDownRxMessage>(this, new EventHandler<MessageEventArgs>(MatchClientPlayerMovementEvent.PlayerMoveKeyDownReceived));
            communication.AddSessionHandler<PlayerMoveKeyUpRxMessage>(this, new EventHandler<MessageEventArgs>(MatchClientPlayerMovementEvent.PlayerMoveKeyUpReceived));
            communication.AddSessionHandler<ChatPlayerTextRxMessage>(this, new EventHandler<MessageEventArgs>(MatchClientPlayerChatEvent.ChatPlayerInputReceived));

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
