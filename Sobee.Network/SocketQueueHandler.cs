using System.Collections.Concurrent;
using System.ComponentModel;
using System.Net.Sockets;
using Serilog;
using Sobee.Common;

public class SocketQueueHandler : Component, IDisposable
{
    private readonly ILogger log = Logging.Get<SocketQueueHandler>();
    private readonly SemaphoreSlim sendSemaphore = new SemaphoreSlim(1, 1);

    public static int MaxSendingSize { get; private set; } = 32768;
    public static int MaxReceivingSize { get; private set; } = 4096;

    protected Socket clientSocket;

    private readonly ConcurrentQueue<byte[]> sendQueue = new ConcurrentQueue<byte[]>();
    private readonly ConcurrentQueue<byte[]> receiveQueue = new ConcurrentQueue<byte[]>();

    protected byte[] receiveBuffer = new byte[MaxReceivingSize];
    protected int receiveBufferOffset;

    protected long totalBytesSent;
    protected long totalBytesReceived;
    protected long totalMessagesSent;
    protected long totalMessagesQueued => sendQueue.Count + receiveQueue.Count;

    public bool IsSocketAlive => clientSocket?.Connected == true;

    protected SocketQueueHandler(Socket socket)
    {
        clientSocket = socket ?? throw new ArgumentNullException(nameof(socket));
        Initialize();
    }

    private void Initialize()
    {
        StartReceivingAsync();
        log.Information("SocketQueueHandler initialized.");
    }

    public void EnqueueSendMessage(byte[] data)
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

    public async Task ProcessSendQueueAsync()
    {
        if (!IsSocketAlive) return;

        await sendSemaphore.WaitAsync();
        try
        {
            while (sendQueue.TryDequeue(out var message))
            {
                try
                {
                    await clientSocket.SendAsync(new ArraySegment<byte>(message), SocketFlags.None);
                    totalBytesSent += message.Length;
                    totalMessagesSent++;
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                    break;
                }
            }
        }
        finally
        {
            sendSemaphore.Release();
        }
    }

    public void EnqueueReceiveMessage(byte[] data)
    {
        if (data == null || data.Length == 0)
        {
            log.Warning("Attempted to enqueue an empty or null received message.");
            return;
        }

        receiveQueue.Enqueue(data);
    }

    public byte[] DequeueReceiveMessage()
    {
        return receiveQueue.TryDequeue(out var message) ? message : null;
    }

    public async Task StartReceivingAsync(CancellationToken cancellationToken = default)
    {
        if (!IsSocketAlive) return;

        try
        {
            while (IsSocketAlive && !cancellationToken.IsCancellationRequested)
            {
                cancellationToken.ThrowIfCancellationRequested();

                int bytesRead = await clientSocket.ReceiveAsync(
                    new ArraySegment<byte>(receiveBuffer, receiveBufferOffset, receiveBuffer.Length - receiveBufferOffset),
                    SocketFlags.None,
                    cancellationToken
                );

                if (bytesRead == 0)
                {
                    Disconnect();
                    break;
                }

                receiveBufferOffset += bytesRead;
                while (receiveBufferOffset > 4)
                {
                    int messageLength = BitConverter.ToInt32(receiveBuffer, 0);
                    if (messageLength > MaxReceivingSize || messageLength <= 0)
                    {
                        HandleException(new Exception("Message length out of range"));
                        return;
                    }

                    int totalLength = 4 + messageLength;
                    if (receiveBufferOffset < totalLength) break;

                    byte[] message = new byte[messageLength];
                    Array.Copy(receiveBuffer, 4, message, 0, messageLength);
                    Array.Copy(receiveBuffer, totalLength, receiveBuffer, 0, receiveBufferOffset - totalLength);
                    receiveBufferOffset -= totalLength;

                    EnqueueReceiveMessage(message);
                }
            }
        }
        catch (OperationCanceledException)
        {
            log.Information("Receiving operation canceled.");
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
        finally
        {
            Disconnect();
        }
    }

    public void Disconnect()
    {
        if (clientSocket != null)
        {
            clientSocket.Close();
            clientSocket = null;
            log.Information("Socket disconnected.");
        }
    }

    private void HandleException(Exception error)
    {
        log.Error(error, "An error occurred.");

        if (error is SocketException socketEx && socketEx.SocketErrorCode == SocketError.TimedOut)
        {
            log.Warning("Socket timeout occurred. Retrying...");
            return; // Bağlantıyı kesmeden devam edebilir.
        }

        Disconnect();
    }

    public new void Dispose()
    {
        clientSocket?.Dispose();
        log.Information("SocketQueueHandler disposed.");
        base.Dispose();
    }
}
