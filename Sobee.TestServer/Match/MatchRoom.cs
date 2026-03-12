using Serilog;
using Sobee.Common;
using Sobee.Network;
using Sobee.Network.Messaging;
using Sobee.TestServer.MatchComponents;
using Sobee.TestServer.MatchEvents;
using Sobee.TestServer.Messages;
using Sobee.TestServer.Messages.Ball;
using Sobee.TestServer.Messages.Chat;
using Sobee.TestServer.Messages.Match;
using Sobee.TestServer.Messages.Player;

namespace Sobee.TestServer.Match
{
    public class MatchRoom : Room<MatchPlayer>
    {
        private static readonly ILogger _log = Logging.Get<MatchRoom>();
        private bool _isDisposed;

        public MatchPlayerManager Players => (MatchPlayerManager)_sessions;
        public readonly ComponentManager<MatchComponent> Components = new();
        public readonly MatchInformationMessage MatchInformation = new();

        // Constants
        public static readonly TimeSpan MAX_IDLE_TIME = new(0, 1, 0); // 1 dakika
        public static readonly int MAX_PLAYER = ScenarioInfo.MAX_TEAM_SIZE; // TODO: Senaryoya göre ayarlanmalı

        public MatchRoom() : base()
        {
            _log.Debug("{id} initializing.", Id);

            // Events
            Players.PlayerJoinEvent += MatchServerPlayerEvent.PlayerJoinReceived;
            Players.PlayerLeaveEvent += MatchServerPlayerEvent.PlayerLeaveReceived;

            // Communication 
            CommunicationType = SessionType.Game;

            // Component
            Components.AddComponent(new MatchMovementComponent(this));
            Components.AddComponent(new MatchBallComponent(this));
            Components.AddComponent(new MatchTimeComponent(this));

            // === Player Messages ===
            RegisterMessageEvent<PlayerHeartbeatRxMessage>(OnReceivedMessage);
            RegisterMessageEvent<PlayerMoveKeyUpRxMessage>(OnReceivedMessage);
            RegisterMessageEvent<PlayerMoveKeyDownRxMessage>(OnReceivedMessage);
            RegisterMessageEvent<PlayerMatchStateAlertRxMessage>(OnReceivedMessage);
            RegisterMessageEvent<ChatPlayerTextRxMessage>(OnReceivedMessage);

            // === Match Messages ===
            RegisterMessageEvent<BallPositioningRxMessage>(OnReceivedMessage);
            AddGlobalHandler<BallPositioningRxMessage>(new EventHandler<MessageEventArgs>(MatchClientBallEvent.BallPositioningReceived));
            RegisterMessageEvent<BallInterceptRxMessage>(OnReceivedMessage);
            AddGlobalHandler<BallInterceptRxMessage>(new EventHandler<MessageEventArgs>(MatchClientBallEvent.BallInterceptReceived));
            RegisterMessageEvent<BallPassNormalRxMessage>(OnReceivedMessage);
            AddGlobalHandler<BallPassNormalRxMessage>(new EventHandler<MessageEventArgs>(MatchClientBallEvent.BallPassNormalReceived));
            RegisterMessageEvent<BallPassThroughRxMessage>(OnReceivedMessage);
            AddGlobalHandler<BallPassThroughRxMessage>(new EventHandler<MessageEventArgs>(MatchClientBallEvent.BallPassThroughReceived));
            RegisterMessageEvent<BallPassLongRxMessage>(OnReceivedMessage);
            AddGlobalHandler<BallPassLongRxMessage>(new EventHandler<MessageEventArgs>(MatchClientBallEvent.BallPassLongReceived));
            RegisterMessageEvent<BallShootRxMessage>(OnReceivedMessage);
            AddGlobalHandler<BallShootRxMessage>(new EventHandler<MessageEventArgs>(MatchClientBallEvent.BallShootReceived));

            _log.Debug("{id} initialized.", Id);
        }

        protected override SessionManager<MatchPlayer> CreateSessionManager()
        {
            return new MatchPlayerManager(this);
        }

        protected override void OnReceivedMessage<T>(Session sender, T message)
        {
            //if (((MatchPlayer)sender).IsReadyForMatch == false)
            //{
            //    _log.Warning("MatchPlayer {playerId} tried sent a message before having IsReadyForMatch set.", sender.Id);
            //    return;
            //}

            base.OnReceivedMessage(sender, message);
        }

        public override void Update(double delta)
        {
            Players?.Update(delta);
            Components?.Update(delta);
            base.Update(delta);
        }

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            if (disposing)
            {
                Components.Dispose();
                _log.Debug("{id} disposed.", Id);
            }

            base.Dispose(disposing);
        }
    }
}