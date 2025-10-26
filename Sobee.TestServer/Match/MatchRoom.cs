using Serilog;
using Sobee.Common;
using Sobee.Messaging;
using Sobee.Network;
using Sobee.TestServer.Auth;
using Sobee.TestServer.Messages.Match;

namespace Sobee.TestServer.Match
{
    public class MatchRoom : Room<MatchPlayer>
    {
        private static readonly ILogger _log = Logging.Get<MatchRoom>();
        private bool _isDisposed;

        private readonly MatchHelper _matchHelper;
        public readonly MatchInformation MatchInformation = new();

        public static readonly TimeSpan MAX_IDLE_TIME = new(0, 1, 0); // 1 dakika
        public static readonly int MAX_PLAYER = 11;

        public Action<MatchRoom, MatchPlayer>? PlayerJoined;
        public Action<MatchRoom, MatchPlayer>? PlayerLeft;

        public MatchRoom() : base()
        {
            _log.Debug("{id} initializing.", Id);

            _matchHelper = new MatchHelper(this);
            CommunicationType = SessionType.Game;

            PlayerJoined += GameEvents.MatchRoomEvent.PlayerJoined;

            //RegisterMessageEvent<PlayerJoined>(OnReceivedMessage);
            //AddGlobalHandler<PlayerJoined>(new EventHandler<MessageEventArgs>(GameEvents.MatchRoomEvent.PlayerJoined));

            _log.Debug("{id} initialized.", Id);
        }

        public bool TryAddPlayer(AuthUser authUser)
        {
            #region Temporary
            if (_matchHelper.TryGeneratePlayer(authUser, out var matchPlayer, out var playerMatchInformation))
            {
                // Dispose auth user after transferring socket to match player
                authUser.Dispose();
            }
            else
            {
                _log.Warning("[TEMPORARY] {roomId} failed to generate match user for auth {authId}.", Id, authUser.Id);
                return false;
            }

            if (_sessions.TryAdd(matchPlayer) && _matchHelper.TryAssignPlayerInfoToMatchInfo(playerMatchInformation))
            {
                _log.Information("[TEMPORARY] Match user {playerId} added to {managerId}.", matchPlayer.Id, Id);
                PlayerJoined?.Invoke(this, matchPlayer);
                return true;
            }
            else if (_sessions.Contains(matchPlayer.Id))
            {
                _log.Warning("[TEMPORARY] Match user {playerId} already exists in {managerId}.", matchPlayer.Id, Id);
            }
            else
            {
                _log.Warning("[TEMPORARY] Failed to add match user {playerId} to {managerId}.", matchPlayer.Id, Id);
                matchPlayer.Disconnect();
            }

            return false;
            #endregion
        }

        public bool TryRemovePlayer(Guid playerId)
        {
            if (!_sessions.TryGetSession(playerId, out var matchPlayer))
            {
                _log.Warning("[TEMPORARY] Match user {playerId} not found in {managerId} for removal.", playerId, Id);
                return false;
            }

            if (_sessions.TryRemove(matchPlayer.Id, out _) && _matchHelper.TryRemovePlayerInfoFromMatchInfo(matchPlayer))
            {
                _log.Information("[TEMPORARY] Match user {playerId} removed from {managerId}.", playerId, Id);
                PlayerLeft?.Invoke(this, matchPlayer);
                return true;
            }
            else if (!_sessions.Contains(playerId))
                _log.Information("[TEMPORARY] Match user {playerId} not found in {managerId} for removal.", playerId, Id);
            else
                _log.Warning("[TEMPORARY] Failed to remove match user {playerId} from {managerId}.", playerId, Id);

            return false;
        }

        protected override void OnReceivedMessage<T>(Session sender, T message)
        {
            //if (((MatchPlayer)sender).MatchInformation == null)
            //{
            //    _log.Warning("MatchPlayer {playerId} tried sent a message before having MatchInformation set.", sender.Id);
            //    return;
            //}

            base.OnReceivedMessage(sender, message);
        }

        public override async Task Update(double delta)
        {
            if (_sessions != null)
            {
                await _sessions.Update(delta);
            }

            await base.Update(delta);
        }

        public int PlayerCount => _sessions.SessionCount;

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