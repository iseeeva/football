using Serilog;
using Sobee.Common;
using Sobee.Messaging;
using Sobee.Network;

namespace Sobee.TestServer.Auth
{
    public class AuthRoom : Room<AuthUser>
    {
        private static readonly ILogger _log = Logging.Get<AuthRoom>();
        private bool _isDisposed;

        public readonly Hub ConnectedHub;

        public AuthRoom(Hub connectedHub) : base()
        {
            _log.Debug("{id} initializing.", Id);
            ConnectedHub = connectedHub;

            CommunicationType = SessionType.Authentication;

            // Bu handler ın bütün sessionlardan gelen mesajları işlemesi gerekiyor.
            RegisterMessageEvent<Messages.Auth.AuthInformation>(OnReceivedMessage);
            AddGlobalHandler<Messages.Auth.AuthInformation>(new EventHandler<MessageEventArgs>(GameEvents.AuthEvent.AuthInformation));

            _log.Debug("{id} initialized.", Id);
        }

        public bool TryAddUser(SocketWrapper socketWrap)
        {
            var authUser = new AuthUser(socketWrap, this);

            if (_sessions.TryAdd(authUser))
            {
                _log.Information("Auth user {userId} added to {roomId} (socket: {socketId}).", socketWrap.Id, Id, socketWrap.Id);
                return true;
            }
            else if (_sessions.Contains(socketWrap.Id))
            {
                _log.Warning("Auth user {userId} already exists in {roomId} (socket: {socketId}).", socketWrap.Id, Id, socketWrap.Id);
            }
            else
            {
                _log.Error("Failed to add auth user {userId} to {roomId} (socket: {socketId}).", socketWrap.Id, Id, socketWrap.Id);
            }

            return false;
        }

        public bool TryRemoveUser(Guid userId)
        {
            if (_sessions.TryRemove(Id, out var removedUser))
            {
                _log.Information("Auth user {userId} removed from {roomId}.", removedUser.Id, Id);
                return true;
            }
            else
            {
                _log.Warning("Auth user {userId} not found in {roomId} for removal.", userId, Id);
                return false;
            }
        }

        public override async Task Update(double delta)
        {
            if (_sessions != null)
            {
                await _sessions.Update(delta);
            }
        }

        public int UserCount => _sessions.SessionCount;

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                if (disposing)
                {
                    _log.Debug("{id} disposing.", Id);
                    _sessions.Dispose();
                    _log.Debug("{id} disposed.", Id);
                }
            }

            base.Dispose(disposing);
        }
    }
}
