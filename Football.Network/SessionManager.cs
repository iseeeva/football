using Football.Common;
using Football.Network.Messaging;
using System.Diagnostics.CodeAnalysis;

namespace Football.Network
{
    public class SessionManager<TOwner, TSession> : Component<TOwner>
        where TOwner : IComponentOwner
        where TSession : Session
    {
        private readonly Dictionary<Guid, TSession> _sessions = new();
        private readonly List<TSession> _sessionUpdateList = new();

        public TSession? this[Guid sessionId] => _sessions.TryGetValue(sessionId, out var s) ? s : null;
        public int Count => _sessions.Count;

        public event Action<TSession>? SessionAdded;
        public event Action<TSession>? SessionRemoved;

        #region Constructor
        public SessionManager()
        {

        }
        #endregion

        #region Sessions
        public bool TryGet(Guid sessionId, [NotNullWhen(true)] out TSession? session)
            => _sessions.TryGetValue(sessionId, out session);

        public virtual bool TryAdd(TSession session)
        {
            if (_sessions.ContainsKey(session.Id))
            {
                _log.Warning("session ({id}) already exists.", session.Id);
                return false;
            }

            _sessions.Add(session.Id, session);
            _sessionUpdateList.Add(session);

            session.Start();
            SessionAdded?.Invoke(session);

            _log.Information("session ({id}) added.", session.Id);
            return true;
        }

        public virtual bool TryRemove(Guid sessionId, [NotNullWhen(true)] out TSession? session)
        {
            if (!_sessions.Remove(sessionId, out session))
            {
                _log.Warning("session ({id}) not found.", sessionId);
                return false;
            }

            _sessionUpdateList.Remove(session);
            session.Dispose();

            SessionRemoved?.Invoke(session);
            _log.Information("session ({id}) removed.", session.Id);
            return true;
        }
        #endregion

        #region Lifecycle
        protected override void OnUpdate(double delta)
        {
            for (int i = _sessionUpdateList.Count - 1; i >= 0; i--)
            {
                var session = _sessionUpdateList[i];
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
                    _log.Error(ex, "session ({id}) update failed.", session.Id);
                    TryRemove(session.Id, out _);
                }
            }
        }
        #endregion

        #region Messaging
        public void SendMessage(Message message)
        {
            for (int i = 0; i < _sessionUpdateList.Count; i++)
            {
                var session = _sessionUpdateList[i];
                if (session.IsConnected)
                    session.SendMessage(message);
            }
        }
        #endregion

        #region Dispose
        protected override void OnDispose()
        {
            for (int i = _sessionUpdateList.Count - 1; i >= 0; i--)
                TryRemove(_sessionUpdateList[i].Id, out _);

            _sessions.Clear();
            _sessionUpdateList.Clear();

            SessionAdded = null;
            SessionRemoved = null;
        }
        #endregion
    }
}