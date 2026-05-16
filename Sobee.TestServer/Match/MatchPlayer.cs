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
        public readonly AuthInformationRxMessage AuthInformation;

        public MatchPlayer(
            SocketWrapper socket,
            AuthInformationRxMessage authInformation,
            MessageCommunication communication
        ) : base(socket, communication)
        {
            SessionType = SessionType.User;
            AuthInformation = authInformation;

            communication.AddSessionMessageHandler<PlayerHeartbeatRxMessage>(this, MatchClientPlayerEvent.PlayerHeartbeatReceived);
            communication.AddSessionMessageHandler<PlayerMatchStateAlertRxMessage>(this, MatchClientPlayerEvent.PlayerMatchStateAlertReceived);
            communication.AddSessionMessageHandler<PlayerMoveKeyDownRxMessage>(this, MatchClientPlayerMovementEvent.PlayerMoveKeyDownReceived);
            communication.AddSessionMessageHandler<PlayerMoveKeyUpRxMessage>(this, MatchClientPlayerMovementEvent.PlayerMoveKeyUpReceived);
            communication.AddSessionMessageHandler<ChatPlayerTextRxMessage>(this, MatchClientPlayerChatEvent.ChatPlayerInputReceived);
        }

        #region Lifecycle
        protected override void OnUpdate(double delta)
        {
            base.OnUpdate(delta);
            SendMessage(new LatencyMessage((float)delta));
        }
        #endregion
    }
}