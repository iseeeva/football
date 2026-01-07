using System.Diagnostics.CodeAnalysis;
using Sobee.Common;
using Sobee.Network.Messaging;

namespace Sobee.Network
{
    public class SessionManager<T> : Component where T : Session
    {
        private static readonly Serilog.ILogger _log = Logging.Get<SessionManager<T>>();
        private bool _isDisposed;

        private readonly Dictionary<Guid, T> _sessions = new();
        private readonly List<T> _updateList = new();

        private readonly MessageCommunication _communication;

        public event Action<T>? SessionAdded;
        public event Action<T>? SessionRemoved;

        public SessionManager(MessageCommunication communication)
        {
            _communication = communication;
            _log.Information("{id} initialized.", Id);
        }

        public T this[Guid id] => _sessions[id];

        public bool TryGet(Guid id, out T? session)
            => _sessions.TryGetValue(id, out session);

        public virtual bool TryAdd(T session)
        {
            if (_sessions.ContainsKey(session.Id))
            {
                _log.Warning("Session {id} already exists.", session.Id);
                return false;
            }

            _sessions.Add(session.Id, session);
            _updateList.Add(session);

            session.Start();
            SessionAdded?.Invoke(session);

            _log.Information("Session {id} added.", session.Id);
            return true;
        }

        public virtual bool TryRemove(Guid sessionId, [NotNullWhen(true)] out T? session)
        {
            if (!_sessions.Remove(sessionId, out session))
            {
                _log.Information("Session {id} not found.", sessionId);
                return false;
            }

            _updateList.Remove(session);

            _communication.RemoveAllSessionHandlers(session);
            session.Dispose();

            SessionRemoved?.Invoke(session);
            _log.Information("Session {id} removed.", sessionId);
            return true;
        }

        public override void Update(double delta)
        {
            for (int i = _updateList.Count - 1; i >= 0; i--)
            {
                var session = _updateList[i];

                if (!session.IsConnected)
                {
                    TryRemove(session.Id, out _);
                    continue;
                }

                try
                {
                    session.Update(delta);
                }
                catch (Exception ex)
                {
                    _log.Error(ex, "Session {id} update failed.", session.Id);
                    TryRemove(session.Id, out _);
                }
            }
        }

        public void SendMessage(Message message)
        {
            foreach (var session in _updateList)
            {
                if (session.IsRunning)
                    session.SendMessage(message);
            }
        }

        public int Count => _sessions.Count;

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            if (disposing)
            {
                // WARN: Session.Dispose'un ne yaptigini kontrol et.
                foreach (var session in _updateList)
                    session.Dispose();

                _sessions.Clear();
                _updateList.Clear();

                SessionAdded = null;
                SessionRemoved = null;

                _log.Debug("{id} disposed.", Id);
            }

            base.Dispose(disposing);
        }
    }
}
