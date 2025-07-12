using System.Collections.Concurrent;
using System.Net.Sockets;
using Serilog;
using Sobee.Common;
using Sobee.Network;

public class SocketQueueHandle : SocketBase
{
    private readonly ILogger log = Logging.Get<SocketQueueHandle>();
    private readonly SemaphoreSlim sendSemaphore = new SemaphoreSlim(1, 1);

    public static int MaxSendingSize { get; private set; } = 32768;
    public static int MaxReceivingSize { get; private set; } = 4096;

    private readonly ConcurrentQueue<byte[]> sendQueue = new ConcurrentQueue<byte[]>();
    private readonly ConcurrentQueue<byte[]> receiveQueue = new ConcurrentQueue<byte[]>();

    private byte[] receiveBuffer = new byte[MaxReceivingSize];
    private int receiveBufferOffset;

    public long totalReceive { get; private set; }
    public long totalBytesReceive { get; private set; }
    public long totalSent { get; private set; }
    public long totalBytesSent { get; private set; }
    public long totalQueued => sendQueue.Count + receiveQueue.Count;

    public SocketQueueHandle(Socket socket) : base(socket)
    {
        try
        {
            _ = StartReceivingAsync();
            log.Debug("{id} initialized.", Socket.RemoteEndPoint);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public override async Task Update()
    {
        try
        {
            await ProcessSendQueueAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    private async Task ProcessSendQueueAsync()
    {
        if (!IsConnected) return;

        await sendSemaphore.WaitAsync();

        try
        {
            while (sendQueue.TryDequeue(out var message))
            {
                try
                {
                    await Socket.SendAsync(new ArraySegment<byte>(message), SocketFlags.None);
                    totalBytesSent += message.Length;
                    totalSent++;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
        finally
        {
            sendSemaphore.Release();
        }
    }

    public void EnqueueSendData(byte[] data)
    {
        if (data == null || data.Length == 0)
        {
            log.Warning("Attempted to enqueue an empty or null message.");
            return;
        }

        var header = BitConverter.GetBytes(data.Length);
        var messageWithHeader = new byte[header.Length + data.Length];
        Buffer.BlockCopy(header, 0, messageWithHeader, 0, header.Length);
        Buffer.BlockCopy(data, 0, messageWithHeader, header.Length, data.Length);
        sendQueue.Enqueue(messageWithHeader);
    }

    public byte[]? DequeueSendData()
    {
        return sendQueue.TryDequeue(out var message) ? message : null;
    }

    private async Task StartReceivingAsync(CancellationToken cancellationToken = default)
    {
        if (!IsConnected) return;

        try
        {
            while (IsConnected && !cancellationToken.IsCancellationRequested)
            {
                int bytesRead = await Socket.ReceiveAsync(
                    new ArraySegment<byte>(receiveBuffer, receiveBufferOffset, receiveBuffer.Length - receiveBufferOffset),
                    SocketFlags.None,
                    cancellationToken
                );

                if (bytesRead == 0)
                {
                    break;
                }

                receiveBufferOffset += bytesRead;
                while (receiveBufferOffset > 4)
                {
                    int messageLength = BitConverter.ToInt32(receiveBuffer, 0);
                    if (messageLength > MaxReceivingSize || messageLength <= 0)
                    {
                        //throw new Exception("Message length out of range"));
                        return;
                    }

                    int totalLength = 4 + messageLength;
                    if (receiveBufferOffset < totalLength) break;

                    byte[] message = new byte[messageLength];
                    Array.Copy(receiveBuffer, 4, message, 0, messageLength);
                    Array.Copy(receiveBuffer, totalLength, receiveBuffer, 0, receiveBufferOffset - totalLength);

                    receiveBufferOffset -= totalLength;
                    totalBytesReceive += message.Length;
                    totalReceive++;

                    EnqueueReceiveData(message);
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public void EnqueueReceiveData(byte[] data)
    {
        if (data == null || data.Length == 0)
        {
            log.Warning("Attempted to enqueue an empty or null received message.");
            return;
        }

        receiveQueue.Enqueue(data);
    }

    public byte[]? DequeueReceiveData()
    {
        return receiveQueue.TryDequeue(out var message) ? message : null;
    }

    public override void Dispose()
    {
        log.Debug("{id} disposing.", Socket.RemoteEndPoint);

        Socket?.Dispose();
        GC.SuppressFinalize(this);
    }
}
