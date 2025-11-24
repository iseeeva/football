using Serilog;
using Sobee.Common;
using Sobee.Messaging;
using Sobee.Network;
using Sobee.TestServer.MatchComponents;
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
        public readonly MatchInformation MatchInformation = new();

        // Constants
        public static readonly TimeSpan MAX_IDLE_TIME = new(0, 1, 0); // 1 dakika
        public static readonly int MAX_PLAYER = ScenarioInfo.MAX_TEAM_SIZE; // TODO: Senaryoya göre ayarlanmalı

        public MatchRoom() : base()
        {
            _log.Debug("{id} initializing.", Id);

            // Events
            Players.PlayerJoined += GameEvents.MatchServerEvent.PlayerJoined;

            // Communication 
            CommunicationType = SessionType.Game;

            // Component
            Components.AddComponent(new MatchMovement(this));
            Components.AddComponent(new MatchBall(this));

            // === Player Messages ===
            RegisterMessageEvent<ChatMessage>(OnReceivedMessage);
            RegisterMessageEvent<HeartbeatMessage>(OnReceivedMessage);
            RegisterMessageEvent<PlayerMoveKeyUp>(OnReceivedMessage);
            RegisterMessageEvent<PlayerMoveKeyDown>(OnReceivedMessage);

            // === Match Messages ===
            RegisterMessageEvent<MatchStateAlert>(OnReceivedMessage);
            AddGlobalHandler<MatchStateAlert>(new EventHandler<MessageEventArgs>(GameEvents.MatchClientEvent.MatchStateAlertReceived));
            RegisterMessageEvent<BallActionerHit>(OnReceivedMessage);
            AddGlobalHandler<BallActionerHit>(new EventHandler<MessageEventArgs>(GameEvents.MatchClientBallEvent.ActionerHitReceived));

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

        public override async Task Update(double delta)
        {
            if (_sessions != null)
            {
                await Players.Update(delta);
                Players.SendMessage(new LatencyMessage((float)delta)); // TODO: Calisiyor ama dogru yer mi emin degilim
            }

            await Components.Update(delta);
            await base.Update(delta);
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