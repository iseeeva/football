using System.Collections.Concurrent;
using System.Net.Sockets;
using Sobee.Common;
using Sobee.Messaging;
using Sobee.Network;
using Sobee.TestServer.Common;

namespace Sobee.TestServer
{
    public class AuthManager : Component
    {
        private readonly AuthCommunication Communication = new();
        private readonly ConcurrentDictionary<Guid, Session> _sessions = new();

        private static readonly Serilog.ILogger _log = Logging.Get<AuthManager>();
        private bool _isDisposed;

        public AuthManager()
        {
            _log.Information("{id} initializing...", Id);
            Communication.SubscribePlayerInformation(new EventHandler<MessageEventArgs>(GameEvents.AuthEvent.AuthInformation));
            _log.Information("{id} initialized.", Id);
        }

        public void Add(Socket clientSocket)
        {
            try
            {
                var session = new Session(new SocketWrapper(clientSocket), Communication);

                if (_sessions.TryAdd(session.Id, session))
                {
                    session.Start();
                    _log.Information("New session {id} added for {remoteEp}.", session.Id, clientSocket.RemoteEndPoint);
                }
                else
                {
                    _log.Warning("Failed to add new session {id} or guid duplicated.", session.Id);
                    session.Dispose();
                }
            }
            catch (Exception ex)
            {
                _log.Error($"Failed to create session for {clientSocket.RemoteEndPoint}: {ex.GetBaseException()}", ex);
                clientSocket.Dispose();
            }
        }

        public override async Task Update(double delta)
        {
            var sessionsToRemove = new List<Guid>();

            foreach (var session in _sessions.Values)
            {
                try
                {
                    if (session.IsActive)
                        await session.Update(delta);
                    else
                        sessionsToRemove.Add(session.Id);
                }
                catch (Exception ex)
                {
                    _log.Error($"Failed to update session {session.Id}: {ex.GetBaseException()}", ex);
                }
            }

            foreach (var id in sessionsToRemove)
            {
                if (_sessions.TryRemove(id, out var session))
                {
                    _log.Information("Session {id} disconnected or failed, removing.", id);
                    session.Dispose();
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                if (disposing)
                {
                    _log.Debug("{id} disposing.", Id);

                    foreach (var session in _sessions.Values)
                    {
                        session.Dispose();
                    }
                    _sessions.Clear();
                }
            }
            base.Dispose(disposing);
        }
    }
}