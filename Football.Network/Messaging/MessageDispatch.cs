using Football.Common;
using Football.Serialization;
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.Serialization;

namespace Football.Network.Messaging
{
    public class MessageDispatch : Component
    {
        private readonly SortedDictionary<ushort, ConstructorInfo> _messageConstructorsById = [];
        private readonly Dictionary<Type, ushort> _messageTypeToId = [];
        private readonly ConcurrentDictionary<int, Delegate> _messageIdToEvent = new();
        private readonly HashSet<ushort> _usedMessageIds = [];

        public int RegisteredMessageCount => _messageTypeToId.Count;

        #region Register
        public void RegisterMessagesFromAllAssemblies()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                RegisterMessagesFromAssembly(assembly);
        }

        public void RegisterMessagesFromCurrentAssembly()
        {
            RegisterMessagesFromAssembly(Assembly.GetExecutingAssembly());
        }

        public void RegisterMessagesFromAssemblyName(string assemblyName)
        {
            try
            {
                var assembly = Assembly.Load(assemblyName);
                RegisterMessagesFromAssembly(assembly);
            }
            catch (FileNotFoundException ex)
            {
                _log.Warning("assembly not found: {AssemblyName} - {Message}", assemblyName, ex.Message);
            }
        }

        public void RegisterMessagesFromAssembly(Assembly assembly)
        {
            if (assembly == null) return;

            Type[] types;
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                types = ex.Types.Where(t => t != null).ToArray()!;
                foreach (var loaderException in ex.LoaderExceptions)
                    _log.Warning("loader exception: {Message}", loaderException?.Message);
            }

            foreach (var type in types)
            {
                var attribute = type.GetCustomAttribute<MessageAttribute>();
                if (attribute != null)
                    RegisterMessageType(attribute.MessageId, type);
            }
        }

        public void RegisterMessageType(ushort messageId, Type type)
        {
            if (_usedMessageIds.Contains(messageId))
            {
                _log.Warning("message id {MessageId} already used. Skipping type {Type}.", messageId, type.FullName);
                return;
            }

            if (_messageTypeToId.ContainsKey(type))
            {
                _log.Warning("type {Type} already registered with ID {MessageId}. Skipping.", type.FullName, _messageTypeToId[type]);
                return;
            }

            var constructor = type.GetConstructor([typeof(BinaryReader)]);
            if (constructor == null)
                throw new NotImplementedException($"Missing BinaryReader constructor for: {type.FullName}");

            _messageConstructorsById[messageId] = constructor;
            _messageTypeToId[type] = messageId;
            _usedMessageIds.Add(messageId);

            _log.Information("registered message type: {Type} with ID: {MessageId}", type.FullName, messageId);
        }

        public void RegisterMessageType(Type type)
        {
            var attribute = type.GetCustomAttribute<MessageAttribute>();
            if (attribute == null)
                throw new NotImplementedException($"Missing [MessageAttribute] on message class: {type.FullName}");

            RegisterMessageType(attribute.MessageId, type);
        }

        public bool IsMessageTypeRegistered<T>(out ushort id) where T : Message
        {
            return _messageTypeToId.TryGetValue(typeof(T), out id);
        }

        public void RegisterMessageEvent<T>(MessageDelegate<T> messageEvent) where T : Message
        {
            ArgumentNullException.ThrowIfNull(messageEvent);

            var id = GetMessageIdFromType(typeof(T));

            _messageIdToEvent.AddOrUpdate(
                id,
                _ => messageEvent,
                (_, existing) =>
                {
                    _log.Warning("message event already registered for ID {Id}, combining delegates...", id);
                    return Delegate.Combine(existing, messageEvent);
                }
            );
        }

        public bool IsMessageEventRegistered<T>(MessageDelegate<T> messageDelegate) where T : Message
        {
            if (!IsMessageTypeRegistered<T>(out ushort id))
                return false;

            if (!_messageIdToEvent.TryGetValue(id, out var registeredDelegate))
                return false;

            return registeredDelegate.GetInvocationList().Contains(messageDelegate);

        }
        #endregion

        #region Dispatch
        public MessageIdFromTypeDelegate GetMessageIdFromType() => new(GetMessageIdFromType);
        public ushort GetMessageIdFromType(Type type)
        {
            if (_messageTypeToId.TryGetValue(type, out var id))
                return id;

            throw new KeyNotFoundException($"Message ID not found for type: {type.FullName}");
        }

        public DispatchToMessageDelegate DispatchToMessageConstructor() => new(DispatchToMessageConstructor);
        public object DispatchToMessageConstructor(ushort messageId, BinaryReader reader)
        {
            if (!_messageConstructorsById.TryGetValue(messageId, out var constructor))
                throw new SerializationException($"ClassID: {messageId} - Not registered.");

            try
            {
                return constructor.Invoke([reader]);
            }
            catch (Exception ex)
            {
                _log.Error(ex, "deserialization failed for message ID: {MessageId}", messageId);
                throw new SerializationException($"ClassID: {messageId} - Failed to deserialize.");
            }
        }

        public void DispatchToMessageEvent(MessageEventArgs args)
        {
            var messageId = GetMessageIdFromType(args.Message.GetType());

            if (!_messageIdToEvent.TryGetValue(messageId, out var eventDelegate))
            {
                _log.Error("session ({sessionId}) tried invoke event for message ({messageId}) but its not registered.", args.Handler.Id, messageId);
                return;
            }

            try
            {
                eventDelegate.DynamicInvoke(args.Handler, args.Message);
                _log.Information("Invoked event for {Type} ({messageId})", args.Message.GetType().FullName, messageId);
            }
            catch (Exception ex)
            {
                _log.Error(ex, "event invocation failed for {messageId}", messageId);
                throw new SerializationException($"ClassID: {messageId} - Event failed.", ex);
            }
        }
        #endregion

        #region Dispose
        protected override void OnDispose()
        {
            _messageConstructorsById.Clear();
            _messageTypeToId.Clear();
            _messageIdToEvent.Clear();
            _usedMessageIds.Clear();
        }
        #endregion
    }
}