using Serilog;
using Sobee.Common;
using Sobee.Messaging;
using Sobee.Network;
using Sobee.TestServer.Messages;
using Sobee.TestServer.Messages.Chat;
using Sobee.TestServer.Messages.Match;

namespace Sobee.TestServer.Match
{
    public class MatchRoom : Room<MatchPlayer>
    {
        private static readonly ILogger _log = Logging.Get<MatchRoom>();
        private bool _isDisposed;

        public MatchPlayerManager Players => (MatchPlayerManager)_sessions;
        public readonly MatchInformation MatchInformation = new();

        // Constants
        public static readonly TimeSpan MAX_IDLE_TIME = new(0, 1, 0); // 1 dakika
        public static readonly int MAX_PLAYER = ScenarioInfo.MAX_TEAM_SIZE; // TODO: Senaryoya göre ayarlanmalı

        public MatchRoom() : base()
        {
            _log.Debug("{id} initializing.", Id);

            // Events
            Players.PlayerJoined += GameEvents.MatchRoomEvent.PlayerJoined;

            // Communication 
            CommunicationType = SessionType.Game;
            RegisterMessageEvent<ChatMessage>(OnReceivedMessage);
            RegisterMessageEvent<HeartbeatMessage>(OnReceivedMessage);

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