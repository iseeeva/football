using System.Collections.Concurrent;
using Sobee.Common;
using Sobee.Messaging;

namespace Sobee.TestServer
{
    public class Communication : MessageDispatch
    {
        private readonly static Serilog.ILogger _log = Logging.Get<Communication>();
        private bool _isDisposed;

        // Nasil calisir: Global handlerlar tum sessionlar icin gecerli olur.
        // Session handlerlari ise sadece belirli bir session icin gecerli olur.
        // Mesaj geldiginde once global handlerlar kontrol edilir, sonra ilgili session handlerlari kontrol edilir.
        // Eger hem global hem de session handler varsa, ikisi de cagrilir.

        private readonly ConcurrentDictionary<Type, EventHandler<MessageEventArgs>> _globalHandlers = new();
        private readonly ConcurrentDictionary<Session, ConcurrentDictionary<Type, EventHandler<MessageEventArgs>>> _sessionHandlers = new();

        public Communication() : base()
        {
            RegisterMessagesFromAssemblyName("Sobee.TestServer.Messages");
            RegisterMessageEvent<Messages.LatencyMessage>(OnReceivedMessage);
        }

        /// <summary>
        /// Odadaki herkes için geçerli olan handlerlar
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        public void AddGlobalHandler<T>(EventHandler<MessageEventArgs> handler) where T : Message
        {
            _globalHandlers.AddOrUpdate(
                typeof(T),
                handler,
                (type, existing) => (EventHandler<MessageEventArgs>)Delegate.Combine(existing, handler)
            );
        }

        public void RemoveGlobalHandler<T>(EventHandler<MessageEventArgs> handler) where T : Message
        {
            if (_globalHandlers.TryGetValue(typeof(T), out var existing))
            {
                var newHandler = (EventHandler<MessageEventArgs>)Delegate.Remove(existing, handler);
                if (newHandler == null)
                    _globalHandlers.TryRemove(typeof(T), out _);
                else
                    _globalHandlers[typeof(T)] = newHandler;
            }
        }

        /// <summary>
        /// Odadaki belirli bir session için geçerli olan handlerlar
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="handler"></param>
        public void AddSessionHandler<T>(Session session, EventHandler<MessageEventArgs> handler) where T : Message
        {
            var handlers = _sessionHandlers.GetOrAdd(session, _ => new());
            handlers.AddOrUpdate(
                typeof(T),
                handler,
                (type, existing) => (EventHandler<MessageEventArgs>)Delegate.Combine(existing, handler)
            );
        }

        public void RemoveSessionHandler<T>(Session session, EventHandler<MessageEventArgs> handler) where T : Message
        {
            if (_sessionHandlers.TryGetValue(session, out var handlers))
            {
                if (handlers.TryGetValue(typeof(T), out var existing))
                {
                    var newHandler = (EventHandler<MessageEventArgs>)Delegate.Remove(existing, handler);
                    if (newHandler == null)
                        handlers.TryRemove(typeof(T), out _);
                    else
                        handlers[typeof(T)] = newHandler;
                }
            }
        }

        public void RemoveAllSessionHandlers(Session session)
        {
            if (_sessionHandlers.TryRemove(session, out _))
                _log.Information("Session {id} handlers removed.", session.Id);
            else if (_sessionHandlers.TryGetValue(session, out var list) && !list.IsEmpty)
                _log.Warning("Failed to remove handlers for session {id}.", session.Id);
            else
                _log.Information("No handlers to remove for session {id}.", session.Id);
        }

        public void DispatchTo<T>(Session sender, T message) where T : Message
        {
            if (_globalHandlers.TryGetValue(typeof(T), out var global))
            {
                global.Invoke(this, new MessageEventArgs(sender, message));
                _log.Debug("{commId}, invoked global handler ({type}) for session {id}.", Id, typeof(T).Name, sender.Id);
            }

            if (_sessionHandlers.TryGetValue(sender, out var map))
            {
                if (map.TryGetValue(typeof(T), out var handler))
                {
                    handler.Invoke(this, new MessageEventArgs(sender, message));
                    //_log.Debug("{commId}, invoked session handler ({type}) for session {id}.", Id, typeof(T).Name, sender.Id);
                }
            }
        }

        protected virtual void OnReceivedMessage<T>(Session sender, T message) where T : Message
        {
            DispatchTo<T>(sender, message);
        }

        //public override Session CreateSession(SocketWrapper socketConnection)
        //{
        //    return new Session(socketConnection, this);
        //}

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                if (disposing)
                {
                    _globalHandlers.Clear();
                    _sessionHandlers.Clear();
                }
            }
            base.Dispose(disposing);
        }
    }
}
