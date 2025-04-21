using System.Reflection;
using System.Runtime.Serialization;
using Serilog;
using Sobee.Common;
using Sobee.Messaging;

public sealed class DispatchHelper
{
    private readonly object owner;
    private ILogger Log = Logging.Get<DispatchHelper>();

    private readonly IDictionary<ushort, ConstructorInfo> messageConstructorsById = new SortedDictionary<ushort, ConstructorInfo>();
    private readonly IDictionary<Type, ushort> messageTypeToId = new Dictionary<Type, ushort>();
    private readonly IDictionary<ushort, GDelegate5> messageIdToEvent = new Dictionary<ushort, GDelegate5>();
    private readonly IDictionary<Type, int> messageTypeToIndex = new Dictionary<Type, int>();
    private readonly HashSet<ushort> usedMessageIds = new HashSet<ushort>();

    public IEnumerable<KeyValuePair<ushort, ConstructorInfo>> GetAllRegisteredConstructors() => messageConstructorsById;
    public GDelegate1 GetDispatcher() => new GDelegate1(this.DispatchToMessageConstructor);
    public GDelegate2 GetMessageTypeToIdDelegate() => new GDelegate2(this.GetMessageTypeId);
    public int GetRegisteredMessageCount() => messageTypeToId.Count;
    public IEnumerable<KeyValuePair<Type, int>> GetAllTypeIndexes() => messageTypeToIndex;

    public DispatchHelper(object owner)
    {
        this.owner = owner;
    }

    public object GetDispatchOwner()
    {
        return this.owner;
    }

    public int GetMessageIndex(Type type)
    {
        if (messageTypeToIndex.TryGetValue(type, out int index))
            return index;

        throw new KeyNotFoundException($"Message type index not found for: {type.FullName}");
    }

    public ushort GetMessageTypeId(Type type)
    {
        if (messageTypeToId.TryGetValue(type, out ushort id))
            return id;

        throw new KeyNotFoundException($"Message ID not found for type: {type.FullName}");
    }

    public void RegisterMessagesFromAllAssemblies()
    {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
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
            Assembly assembly = Assembly.Load(assemblyName);
            RegisterMessagesFromAssembly(assembly);
        }
        catch (FileNotFoundException) { }
    }

    public void RegisterMessagesFromAssembly(Assembly assembly)
    {
        foreach (Type type in assembly.GetTypes())
        {
            var attributes = type.GetCustomAttributes(typeof(GAttribute0), false) as GAttribute0[];
            if (attributes != null && attributes.Length > 0)
            {
                RegisterMessageType(attributes[0].method_0(), type);
            }
        }
    }

    public void RegisterMessageEvent(Type messageType, GDelegate5 messageEvent)
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
            Log.Warning("Message ID {MessageId} is already used for another message. Skipping registration of {Type}.", messageId, type.FullName);
            return;
        }

        if (messageTypeToId.ContainsKey(type))
        {
            Log.Warning("Type {Type} is already registered with ID {MessageId}. Skipping duplicate registration.", type.FullName, messageTypeToId[type]);
            return;
        }

        var constructor = type.GetConstructor(new[] { typeof(BinaryReader) });
        if (constructor == null)
        {
            throw new NotImplementedException($"Missing BinaryReader constructor: {type.FullName}");
        }

        messageConstructorsById[messageId] = constructor;
        messageTypeToId[type] = messageId;
        messageTypeToIndex[type] = messageTypeToIndex.Count + 1;
        usedMessageIds.Add(messageId);

        Log.Information("Message type registered: {Type} with ID {MessageId}", type.FullName, messageId);
    }

    public void RegisterMessageType(Type type)
    {
        var attributes = type.GetCustomAttributes(typeof(GAttribute0), false) as GAttribute0[];
        if (attributes == null || attributes.Length == 0)
        {
            throw new NotImplementedException($"Missing [GAttribute0] on message class: {type.FullName}");
        }

        var messageId = attributes[0].method_0();
        RegisterMessageType(messageId, type);
    }

    public object DispatchToMessageConstructor(ushort messageId, BinaryReader reader)
    {
        if (messageConstructorsById.TryGetValue(messageId, out ConstructorInfo constructor))
        {
            try
            {
                return constructor.Invoke(new object[] { reader });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to deserialize message for ID: {MessageId}", messageId);
                throw new SerializationException($"ClassID: {messageId} - Failed to deserialize message.");
            }
        }

        throw new SerializationException($"ClassID: {messageId} - Not registered.");
    }

    public void DispatchToMessageEvent(Type messageType, GEventArgs23 e)
    {
        var id = GetMessageTypeId(messageType);
        if (messageIdToEvent.TryGetValue(id, out GDelegate5 eventDelegate))
        {
            try
            {
                eventDelegate.Invoke(e.method_0(), e.method_1());
                Log.Information("Trying to invoke event for {type} (Id: {Id})", messageType, id);
                return;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to invoke event for ClassID: {Id}", id);
                throw new SerializationException($"ClassID: {id} - Failed to invoke event.");
            }
        }

        throw new SerializationException($"ClassID: {id} - Not registered.");
    }
}
