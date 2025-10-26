using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Sobee.Common;
using Sobee.Messaging;

namespace Sobee.TestServer
{
    public class SessionManager<T> : Component where T : Session
    {
        private static readonly Serilog.ILogger _log = Logging.Get<SessionManager<T>>();
        private bool _isDisposed;

        private readonly ConcurrentDictionary<Guid, T> _sessions = new();
        protected readonly Communication _communication;

        public event Action<T>? SessionAdded;
        public event Action<T>? SessionRemoved;

        public SessionManager(Communication communication)
        {
            _communication = communication;
            _log.Information("{id} initializing.", Id);
            _log.Information("{id} initialized.", Id);
        }

        public virtual bool TryGetSession(Guid sessionId, [MaybeNullWhen(false)] out T session)
        {
            return _sessions.TryGetValue(sessionId, out session);
        }

        public virtual bool TryAdd(T session)
        {
            if (_sessions.TryAdd(session.Id, session))
            {
                session.Start();
                _log.Information("Session {id} added.", session.Id);
                SessionAdded?.Invoke(session);
                return true;
            }
            else if (_sessions.ContainsKey(session.Id))
            {
                _log.Warning("Session {id} already exists, skipping add.", session.Id);
            }

            _log.Warning("Failed to add session {id}.", session.Id);
            return false;
        }

        //public virtual bool Add(SocketWrapper socketWrap)
        //{
        //    var session = (T)Activator.CreateInstance(typeof(T), socketWrap, _communication)!; // WARN: Dinamik tipler için tehlikeli

        //    if (!TryAdd(session))
        //    {
        //        session.Disconnect();
        //        session.Dispose();

        //        _log.Warning("Failed to create session from socket {socketId}.", socketWrap.Id);
        //        return false;
        //    }

        //    _log.Information("Session ({sessionTypeName}) {id} created from socket {socketId}.", session.GetType().Name, session.Id, socketWrap.Id);
        //    return true;
        //}

        public virtual bool TryRemove(Guid sessionId, [MaybeNullWhen(false)] out T session)
        {
            if (_sessions.TryRemove(sessionId, out session))
            {
                _communication.RemoveAllSessionHandlers(session);
                session.Dispose();

                _log.Information("Session {id} removed.", sessionId);
                SessionRemoved?.Invoke(session);
                return true;
            }

            _log.Information("Session {id} not found for removal.", sessionId);
            return false;
        }

        public bool Contains(Guid sessionId) => _sessions.ContainsKey(sessionId);

        public virtual void SendMessage(Message message)
        {
            foreach (var session in _sessions.Values)
            {
                if (session.IsActive)
                    session.SendMessage(message);
            }
        }

        public override async Task Update(double delta)
        {
            var sessionsToRemove = new ConcurrentBag<Guid>();

            await Task.WhenAll(_sessions.Values.Select(async session =>
            {
                try
                {
                    if (session.IsConnected)
                        await session.Update(delta);
                    else
                        sessionsToRemove.Add(session.Id);
                }
                catch (Exception ex)
                {
                    _log.Error($"Failed to update session {session.Id}: {ex.GetBaseException()}", ex);
                }
            }));

            foreach (var id in sessionsToRemove)
            {
                if (TryRemove(id, out _))
                    _log.Information("Session {id} disconnected or failed, removing.", id);
            }
        }

        public int SessionCount => _sessions.Count;

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            _isDisposed = true;
            if (disposing)
            {
                _log.Debug("{id} disposing.", Id);

                foreach (var session in _sessions.Values.ToList())
                {
                    session.Disconnect();
                    session.Dispose();
                }

                _sessions.Clear();
            }

            base.Dispose(disposing);
        }
    }
}
