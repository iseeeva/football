using Sobee.Common;
using System.Collections.Concurrent;
using System.Reflection;

namespace Sobee.Network.Messaging
{
    public class MessageCommunication : ComponentManager<IComponent>
    {
        public SessionType CommunicationType { get; set; }

        private readonly MessageDispatch _dispatcher = new();
        private readonly ConcurrentDictionary<Type, EventHandler<MessageEventArgs>> _globalHandlers = new();
        private readonly ConcurrentDictionary<Session, ConcurrentDictionary<Type, EventHandler<MessageEventArgs>>> _sessionHandlers = new();

        public MessageCommunication()
        {
            // === Components ===
            AddComponent(_dispatcher);
        }

        #region Message Registration
        public void RegisterMessages(string assemblyName)
            => _dispatcher.RegisterMessagesFromAssemblyName(assemblyName);

        public void RegisterMessages(Assembly assembly)
            => _dispatcher.RegisterMessagesFromAssembly(assembly);

        public void RegisterMessage<T>() where T : Message
        {
            if (!_dispatcher.IsMessageTypeRegistered<T>(out _))
                _dispatcher.RegisterMessageType(typeof(T));

            if (!_dispatcher.IsMessageEventRegistered<T>(OnReceivedMessage))
                _dispatcher.RegisterMessageEvent<T>(OnReceivedMessage);
        }

        public DispatchToMessageDelegate GetMessageConstructor()
            => _dispatcher.DispatchToMessageConstructor();

        public MessageIdFromTypeDelegate GetMessageIdFromType()
            => _dispatcher.GetMessageIdFromType();
        #endregion

        #region Global Handlers
        public void AddGlobalMessageHandler<T>(EventHandler<MessageEventArgs> handler) where T : Message
        {
            _globalHandlers.AddOrUpdate(
                typeof(T),
                handler,
                (_, existing) => (EventHandler<MessageEventArgs>)Delegate.Combine(existing, handler)
            );
        }

        public void RemoveGlobalMessageHandler<T>(EventHandler<MessageEventArgs> handler) where T : Message
        {
            if (_globalHandlers.TryGetValue(typeof(T), out var existing))
            {
                var newHandler = (EventHandler<MessageEventArgs>?)Delegate.Remove(existing, handler);
                if (newHandler == null)
                    _globalHandlers.TryRemove(typeof(T), out _);
                else
                    _globalHandlers[typeof(T)] = newHandler;
            }
        }

        public void RemoveGlobalMessageHandlers()
        {
            _globalHandlers.Clear();
        }
        #endregion

        #region Session Handlers
        public void AddSessionMessageHandler<T>(Session session, EventHandler<MessageEventArgs> handler) where T : Message
        {
            var handlers = _sessionHandlers.GetOrAdd(session, _ => new());
            handlers.AddOrUpdate(
                typeof(T),
                handler,
                (_, existing) => (EventHandler<MessageEventArgs>)Delegate.Combine(existing, handler)
            );
        }

        public void RemoveSessionMessageHandler<T>(Session session, EventHandler<MessageEventArgs> handler) where T : Message
        {
            if (_sessionHandlers.TryGetValue(session, out var handlers))
            {
                if (handlers.TryGetValue(typeof(T), out var existing))
                {
                    var newHandler = (EventHandler<MessageEventArgs>?)Delegate.Remove(existing, handler);
                    if (newHandler == null)
                        handlers.TryRemove(typeof(T), out _);
                    else
                        handlers[typeof(T)] = newHandler;
                }
            }
        }

        public void RemoveSessionMessageHandlers(Session session)
        {
            if (_sessionHandlers.TryRemove(session, out var sessionHandlers))
                _log.Information("removed {handlerCount} handlers for session ({id}).", sessionHandlers.Count, session.Id);
            else if (_sessionHandlers.TryGetValue(session, out var list) && !list.IsEmpty)
                _log.Warning("failed to remove handlers for session ({id}).", session.Id);
            else
                _log.Information("no handlers to remove for session ({id}).", session.Id);
        }

        public bool HasMessageHandler<T>(Session? session = null) where T : Message
        {
            if (session == null)
                return _globalHandlers.ContainsKey(typeof(T));

            return _sessionHandlers.TryGetValue(session, out var map)
                && map.ContainsKey(typeof(T));
        }
        #endregion

        #region OneShot Handlers
        public void AddOneShotMessageHandler<T>(Session? session, Action<Session, T> handler) where T : Message
        {
            if (!_dispatcher.IsMessageTypeRegistered<T>(out _))
            {
                _log.Error("AddOneShotMessageHandler failed: MessageType {name} is not registered.", typeof(T).Name);
                return;
            }

            EventHandler<MessageEventArgs>? wrapper = null;
            wrapper = (sender, args) =>
            {
                if (session != null && args.Handler != session) return;
                if (args.Message is not T msg) return;

                if (session == null)
                    RemoveGlobalMessageHandler<T>(wrapper!);
                else
                    RemoveSessionMessageHandler<T>(session, wrapper!);

                handler(args.Handler, msg);
            };

            if (session == null)
                AddGlobalMessageHandler<T>(wrapper);
            else
                AddSessionMessageHandler<T>(session, wrapper);
        }
        #endregion

        #region Dispatch
        public bool DispatchToMessageEvent(MessageEventArgs eventArgs)
        {
            Type messageType = eventArgs.Message.GetType();

            bool globalInvoked = false;
            if (_globalHandlers.TryGetValue(messageType, out var global))
            {
                global.Invoke((object?)Owner ?? this, eventArgs);
                _log.Information("invoked global handler {type} for session ({id}).", messageType.Name, eventArgs.Handler.Id);
                globalInvoked = true;
            }

            bool sessionInvoked = false;
            if (_sessionHandlers.TryGetValue(eventArgs.Handler, out var map))
            {
                if (map.TryGetValue(messageType, out var handler))
                {
                    handler.Invoke((object?)Owner ?? this, eventArgs);
                    _log.Information("invoked session handler {type} for session ({id}).", messageType.Name, eventArgs.Handler.Id);
                    sessionInvoked = true;
                }
            }

            return globalInvoked || sessionInvoked;
        }

        protected virtual void OnReceivedMessage<T>(Session sender, T message) where T : Message
        {
            DispatchToMessageEvent(new MessageEventArgs(sender, message));
        }
        #endregion

        #region Dispose
        protected override void OnDispose()
        {
            _globalHandlers.Clear();
            _sessionHandlers.Clear();
            _dispatcher.Dispose();
        }
        #endregion
    }
}