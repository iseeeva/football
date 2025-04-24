using System.Reflection;
using System.Runtime.Serialization;
using Serilog;
using Sobee.Common;
using Sobee.Messaging;

public sealed class MessageDispatcher
{
    private readonly object owner;
    private readonly ILogger Log = Logging.Get<MessageDispatcher>();

    private readonly IDictionary<ushort, ConstructorInfo> messageConstructorsById = new SortedDictionary<ushort, ConstructorInfo>();
    private readonly IDictionary<Type, ushort> messageTypeToId = new Dictionary<Type, ushort>();
    private readonly IDictionary<ushort, MessageDelegate> messageIdToEvent = new Dictionary<ushort, MessageDelegate>();
    private readonly IDictionary<Type, int> messageTypeToIndex = new Dictionary<Type, int>();
    private readonly HashSet<ushort> usedMessageIds = new HashSet<ushort>();

    public MessageDispatcher(object owner)
    {
        this.owner = owner ?? throw new ArgumentNullException(nameof(owner));
    }

    public object Owner => owner;

    public IEnumerable<KeyValuePair<ushort, ConstructorInfo>> GetAllRegisteredConstructors() => messageConstructorsById;

    public GDelegate1 GetDispatcher() => new GDelegate1(DispatchToMessageConstructor);

    public GDelegate2 GetMessageTypeToIdDelegate() => new GDelegate2(GetMessageTypeId);

    public int GetRegisteredMessageCount() => messageTypeToId.Count;

    public IEnumerable<KeyValuePair<Type, int>> GetAllTypeIndexes() => messageTypeToIndex;

    public int GetMessageIndex(Type type)
    {
        if (messageTypeToIndex.TryGetValue(type, out var index))
            return index;

        throw new KeyNotFoundException($"Message type index not found for: {type.FullName}");
    }

    public ushort GetMessageTypeId(Type type)
    {
        if (messageTypeToId.TryGetValue(type, out var id))
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
            Log.Warning("Assembly not found: {AssemblyName} - {Message}", assemblyName, ex.Message);
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
                Log.Warning("Loader exception: {Message}", loaderException?.Message);
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

        if (messageIdToEvent.ContainsKey(id))
        {
            Log.Warning("Message event already registered for ID {Id}, overwriting...", id);
        }

        messageIdToEvent[id] = messageEvent;
    }

    public void RegisterMessageType(ushort messageId, Type type)
    {
        if (usedMessageIds.Contains(messageId))
        {
            Log.Warning("Message ID {MessageId} already used. Skipping type {Type}.", messageId, type.FullName);
            return;
        }

        if (messageTypeToId.ContainsKey(type))
        {
            Log.Warning("Type {Type} already registered with ID {MessageId}. Skipping.", type.FullName, messageTypeToId[type]);
            return;
        }

        var constructor = type.GetConstructor(new[] { typeof(BinaryReader) });
        if (constructor == null)
        {
            throw new NotImplementedException($"Missing BinaryReader constructor for: {type.FullName}");
        }

        messageConstructorsById[messageId] = constructor;
        messageTypeToId[type] = messageId;
        messageTypeToIndex[type] = messageTypeToIndex.Count + 1;
        usedMessageIds.Add(messageId);

        Log.Information("Registered message type: {Type} with ID: {MessageId}", type.FullName, messageId);
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
        if (!messageConstructorsById.TryGetValue(messageId, out var constructor))
        {
            throw new SerializationException($"ClassID: {messageId} - Not registered.");
        }

        try
        {
            return constructor.Invoke(new object[] { reader });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Deserialization failed for message ID: {MessageId}", messageId);
            throw new SerializationException($"ClassID: {messageId} - Failed to deserialize.");
        }
    }

    public void DispatchToMessageEvent(Type messageType, MessageDelegateArgs e)
    {
        var id = GetMessageTypeId(messageType);
        if (!messageIdToEvent.TryGetValue(id, out var eventDelegate))
        {
            throw new SerializationException($"ClassID: {id} - Not registered.");
        }

        try
        {
            eventDelegate.Invoke(e.method_0(), e.method_1());
            Log.Information("Invoked event for {Type} (ID: {Id})", messageType.FullName, id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Event invocation failed for ClassID: {Id}", id);
            throw new SerializationException($"ClassID: {id} - Event failed.");
        }
    }
}
