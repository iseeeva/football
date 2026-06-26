using Football.GameServer.Game;
using Football.GameServer.MatchEvents;
using Football.GameServer.Messages;
using Football.GameServer.Messages.Auth;
using Football.GameServer.Messages.Chat;
using Football.GameServer.Messages.Player;
using Football.Network;
using Football.Network.Messaging;

namespace Football.GameServer.Match
{
    public class MatchUser : GameUser
    {
        public readonly MatchClientAuthInfoRxMessage AuthInformation;

        public MatchUser(
            SocketWrapper socket,
            MatchClientAuthInfoRxMessage authInformation,
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