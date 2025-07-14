using System.Reflection;
using System.Runtime.Serialization;
using Serilog;
using Sobee.Common;

namespace Sobee.Messaging
{
    // TODO: Dispose pattern
    public sealed class MessageDispatch
    {
        public object Owner { get; private set; }
        private readonly ILogger _log = Logging.Get<MessageDispatch>();

        private readonly SortedDictionary<ushort, ConstructorInfo> _messageConstructorsById = [];
        private readonly Dictionary<Type, ushort> _messageTypeToId = [];
        private readonly Dictionary<ushort, MessageDelegate> _messageIdToEvent = [];
        private readonly Dictionary<Type, int> _messageTypeToIndex = [];
        private readonly HashSet<ushort> _usedMessageIds = [];

        public MessageDispatch(object owner)
        {
            this.Owner = owner ?? throw new ArgumentNullException(nameof(owner));
        }

        public IEnumerable<KeyValuePair<ushort, ConstructorInfo>> GetAllRegisteredConstructors() => _messageConstructorsById;

        public GDelegate1 GetDispatcher() => new(DispatchToMessageConstructor);

        public GDelegate2 GetMessageTypeToIdDelegate() => new(GetMessageTypeId);

        public int GetRegisteredMessageCount() => _messageTypeToId.Count;

        public IEnumerable<KeyValuePair<Type, int>> GetAllTypeIndexes() => _messageTypeToIndex;

        public int GetMessageIndex(Type type)
        {
            if (_messageTypeToIndex.TryGetValue(type, out var index))
                return index;

            throw new KeyNotFoundException($"Message type index not found for: {type.FullName}");
        }

        public ushort GetMessageTypeId(Type type)
        {
            if (_messageTypeToId.TryGetValue(type, out var id))
                return id;

            throw new KeyNotFoundException($"Message ID not found for type: {type.FullName}");
        }

        public void RegisterMessagesFromAllAssemblies()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                RegisterMessagesFromAssembly(assembly);
            }
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
                _log.Warning("Assembly not found: {AssemblyName} - {Message}", assemblyName, ex.Message);
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
                types = ex.Types.Where(t => t != null).ToArray();
                foreach (var loaderException in ex.LoaderExceptions)
                {
                    _log.Warning("Loader exception: {Message}", loaderException?.Message);
                }
            }

            foreach (var type in types)
            {
                var attribute = type.GetCustomAttribute<GAttribute0>();
                if (attribute != null)
                {
                    RegisterMessageType(attribute.method_0(), type);
                }
            }
        }

        public void RegisterMessageEvent(Type messageType, MessageDelegate messageEvent)
        {
            var id = GetMessageTypeId(messageType);

            if (_messageIdToEvent.ContainsKey(id))
            {
                _log.Warning("Message event already registered for ID {Id}, overwriting...", id);
            }

            _messageIdToEvent[id] = messageEvent;
        }

        public void RegisterMessageType(ushort messageId, Type type)
        {
            if (_usedMessageIds.Contains(messageId))
            {
                _log.Warning("Message ID {MessageId} already used. Skipping type {Type}.", messageId, type.FullName);
                return;
            }

            if (_messageTypeToId.ContainsKey(type))
            {
                _log.Warning("Type {Type} already registered with ID {MessageId}. Skipping.", type.FullName, _messageTypeToId[type]);
                return;
            }

            var constructor = type.GetConstructor(new[] { typeof(BinaryReader) });
            if (constructor == null)
            {
                throw new NotImplementedException($"Missing BinaryReader constructor for: {type.FullName}");
            }

            _messageConstructorsById[messageId] = constructor;
            _messageTypeToId[type] = messageId;
            _messageTypeToIndex[type] = _messageTypeToIndex.Count + 1;
            _usedMessageIds.Add(messageId);

            _log.Information("Registered message type: {Type} with ID: {MessageId}", type.FullName, messageId);
        }

        public void RegisterMessageType(Type type)
        {
            var attribute = type.GetCustomAttribute<GAttribute0>();
            if (attribute == null)
            {
                throw new NotImplementedException($"Missing [GAttribute0] on message class: {type.FullName}");
            }

            RegisterMessageType(attribute.method_0(), type);
        }

        public object DispatchToMessageConstructor(ushort messageId, BinaryReader reader)
        {
            if (!_messageConstructorsById.TryGetValue(messageId, out var constructor))
            {
                throw new SerializationException($"ClassID: {messageId} - Not registered.");
            }

            try
            {
                return constructor.Invoke(new object[] { reader });
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Deserialization failed for message ID: {MessageId}", messageId);
                throw new SerializationException($"ClassID: {messageId} - Failed to deserialize.");
            }
        }

        public void DispatchToMessageEvent(MessageDelegateArgs args)
        {
            var id = GetMessageTypeId(args.eventArgs.message.GetType());
            if (!_messageIdToEvent.TryGetValue(id, out var eventDelegate))
            {
                throw new SerializationException($"ClassID: {id} - Not registered.");
            }

            try
            {
                eventDelegate.Invoke(args.sender, args.eventArgs);
                _log.Information("Invoked event for {Type} (ID: {Id})", args.eventArgs.message.GetType().FullName, id);
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Event invocation failed for ClassID: {Id}", id);
                throw new SerializationException($"ClassID: {id} - Event failed.");
            }
        }
    }
}