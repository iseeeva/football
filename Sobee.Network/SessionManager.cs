using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Sobee.Common;
using Sobee.Network.Messaging;
namespace Sobee.Network;

public class SessionManager<T> : Component where T : Session
{
    private static readonly Serilog.ILogger _log = Logging.Get<SessionManager<T>>();
    private bool _isDisposed;

    private readonly ConcurrentDictionary<Guid, T> _sessions = new();
    private readonly MessageCommunication _communication;

    public event Action<T>? SessionAdded;
    public event Action<T>? SessionRemoved;

    public SessionManager(MessageCommunication communication)
    {
        _communication = communication;
        _log.Information("{id} initialized.", Id);
    }

    public T this[Guid id] => _sessions[id];

    public bool TryGet(Guid id, [NotNullWhen(true)] out T? session)
        => _sessions.TryGetValue(id, out session);

    public virtual bool TryAdd(T session)
    {
        if (!_sessions.TryAdd(session.Id, session))
        {
            _log.Warning("Failed to add session {id} (already exists?).", session.Id);
            return false;
        }

        session.Start();
        SessionAdded?.Invoke(session);

        _log.Information("Session {id} added.", session.Id);
        return true;
    }

    public virtual bool TryRemove(Guid sessionId, [NotNullWhen(true)] out T? session)
    {
        if (!_sessions.TryRemove(sessionId, out session))
        {
            _log.Information("Session {id} not found for removal.", sessionId);
            return false;
        }

        _communication.RemoveAllSessionHandlers(session);
        session.Dispose();
        SessionRemoved?.Invoke(session);

        _log.Information("Session {id} removed.", sessionId);
        return true;
    }

    public void SendMessage(Message message)
    {
        foreach (var session in _sessions.Values)
        {
            if (session.IsActive)
                session.SendMessage(message);
        }
    }

    public override async Task Update(double delta)
    {
        var deadSessions = new ConcurrentBag<Guid>();

        await Task.WhenAll(_sessions.Values.Select(async session =>
        {
            try
            {
                if (session.IsConnected)
                    await session.Update(delta);
                else
                    deadSessions.Add(session.Id);
            }
            catch (Exception ex)
            {
                _log.Error($"Failed to update session {session.Id}: {ex.GetBaseException()}", ex);
                deadSessions.Add(session.Id);
            }
        }));

        foreach (var id in deadSessions)
        {
            if (TryRemove(id, out _))
                _log.Information("Session {id} removed due disconnection.", id);
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
            try
            {
                foreach (var session in _sessions.Values)
                {
                    // WARN: Session.Dispose'un ne yaptigini kontrol et.
                    TryRemove(session.Id, out _);
                }

                // Events
                SessionAdded = null;
                SessionRemoved = null;

                _log.Debug("{id} disposed.", Id);
            }
            catch (Exception ex)
            {
                _log.Error(ex, "{id} dispose error.", Id);
            }
        }

        base.Dispose(disposing);
    }
}
