using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using Serilog;
using Sobee.Common;
using Sobee.Network;

public class SocketQueueHandler : Component
{
    private ILogger log = Logging.Get<SocketQueueHandler>();

    private IContainer components;
    private static GClass325 taskScheduler = new GClass325();

    private EventHandler onDisconnected;
    private EventHandler<GEventArgs10> onSocketError;
    private EventHandler<GEventArgs10> onConnectionError;

    public static int BufferSize = 32768;
    public static int MaxMessageSize = 4096;

    protected Socket clientSocket;
    protected IPEndPoint remoteEndPoint;

    protected MemoryStream sendBuffer = new MemoryStream(BufferSize);
    protected Queue<byte[]> receivedMessagesQueue = new Queue<byte[]>();
    protected byte[] receiveBuffer = new byte[MaxMessageSize];
    protected int receiveBufferOffset;

    protected long totalBytesSent;
    protected long totalBytesReceived;
    protected long totalMessagesSent;
    protected long totalMessagesQueued;

    protected static byte[] messageHeaderBuffer = new byte[4];
    protected static byte[] peekBuffer = new byte[MaxMessageSize];

    public SocketQueueHandler()
    {
        Initialize();
    }

    protected SocketQueueHandler(Socket socket, IPEndPoint endPoint)
    {
        Initialize();
        this.clientSocket = socket;
        this.remoteEndPoint = endPoint;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void Initialize() { }

    public static void SetTaskDelay(double seconds)
    {
        taskScheduler.method_34(seconds);
    }

    private void QueueSendCallback(object state)
    {
        lock (sendBuffer)
        {
            var data = (QueuedData)state;
            data.socket.BeginSend(data.memoryStream.GetBuffer(), 0, (int)sendBuffer.Length, SocketFlags.None, SendCallback, data.socket);
            data.memoryStream.SetLength(0);
            data.memoryStream.Position = 0;
        }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void AddDisconnectedHandler(EventHandler handler) => onDisconnected += handler;

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void RemoveDisconnectedHandler(EventHandler handler) => onDisconnected -= handler;

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void AddSocketErrorHandler(EventHandler<GEventArgs10> handler) => onSocketError += handler;

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void RemoveSocketErrorHandler(EventHandler<GEventArgs10> handler) => onSocketError -= handler;

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void AddConnectionErrorHandler(EventHandler<GEventArgs10> handler) => onConnectionError += handler;

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void RemoveConnectionErrorHandler(EventHandler<GEventArgs10> handler) => onConnectionError -= handler;

    public static int GetMaxMessageSize() => MaxMessageSize;

    public static void SetMaxMessageSize(int size)
    {
        MaxMessageSize = size;
        peekBuffer = new byte[size];
    }

    public IPEndPoint GetRemoteEndPoint() => remoteEndPoint;

    public void SetRemoteEndPoint(IPEndPoint endPoint) => remoteEndPoint = endPoint;

    public IPEndPoint GetLocalEndPoint() => (IPEndPoint)clientSocket.LocalEndPoint;

    public bool GetIsConnected() => this.clientSocket.Connected;

    public long GetTotalBytesSent() => totalBytesSent;
    public long GetTotalBytesReceived() => totalBytesReceived;
    public long GetTotalMessagesSent() => totalMessagesSent;
    public long GetTotalMessagesQueued() => totalMessagesQueued;

    public virtual void EnqueueMessage(byte[] data, int offset, int length)
    {
        lock (sendBuffer)
        {
            var header = BitConverter.GetBytes(length);
            sendBuffer.Write(header, 0, header.Length);
            sendBuffer.Write(data, offset, length);
            totalMessagesSent++;
        }
    }

    public void Disconnect()
    {
        if (clientSocket != null)
        {
            clientSocket.Close();
            clientSocket = null;
            log.Information($"{GetRemoteEndPoint()} closed");
            OnDisconnected();
        }
    }

    public void Clear()
    {
        totalBytesSent = 0;
        totalBytesReceived = 0;
        totalMessagesSent = 0;
        totalMessagesQueued = 0;
    }

    protected virtual void OnDisconnected() => onDisconnected?.Invoke(this, EventArgs.Empty);
    protected virtual void OnSocketError(SocketError error) => onSocketError?.Invoke(this, new GEventArgs10(error));
    protected virtual void OnConnectionError(ConnectionError error) => onSocketError?.Invoke(this, new GEventArgs10(error));
    protected virtual void OnReceiveError(SocketError error) => onConnectionError?.Invoke(this, new GEventArgs10(error));

    public int FlushSendBuffer()
    {
        int sent = 0;
        if (clientSocket == null) return sent;

        lock (sendBuffer)
        {
            try
            {
                if (sendBuffer.Length > 0)
                {
                    sent = clientSocket.Send(sendBuffer.GetBuffer(), 0, (int)sendBuffer.Length, SocketFlags.None);
                    totalBytesSent += sent;
                    sendBuffer.SetLength(0);
                    sendBuffer.Position = 0;
                }
            }
            catch (SocketException ex)
            {
                OnReceiveError(ex.SocketErrorCode);
                Disconnect();
            }
            catch (ObjectDisposedException)
            {
                OnDisconnected();
            }
        }
        return sent;
    }

    public virtual byte[] Receive()
    {
        if (clientSocket == null || !clientSocket.Connected)
        {
            Disconnect();
            return null;
        }

        try
        {
            if (clientSocket.Poll(0, SelectMode.SelectRead))
            {
                int peekSize = clientSocket.Receive(peekBuffer, clientSocket.Available, SocketFlags.Peek);
                if (peekSize > MaxMessageSize || peekSize <= 0)
                {
                    Disconnect();
                    return null;
                }

                if (clientSocket.Available > 4)
                {
                    clientSocket.Receive(messageHeaderBuffer, 4, SocketFlags.Peek);
                    int messageLength = BitConverter.ToInt32(messageHeaderBuffer, 0);

                    if (clientSocket.Available >= 4 + messageLength && messageLength > 0 && messageLength <= MaxMessageSize)
                    {
                        byte[] message = new byte[messageLength];
                        clientSocket.Receive(messageHeaderBuffer, 4, SocketFlags.None);
                        clientSocket.Receive(message, messageLength, SocketFlags.None);
                        receiveBufferOffset += 4 + messageLength;
                        totalBytesReceived += 4 + messageLength;
                        totalMessagesQueued++;
                        return message;
                    }
                }
            }
        }
        catch (SocketException ex)
        {
            OnSocketError(ex.SocketErrorCode);
            Disconnect();
        }
        catch (ObjectDisposedException)
        {
            OnDisconnected();
        }

        return null;
    }

    public byte[] DequeueMessage()
    {
        lock (receivedMessagesQueue)
        {
            return receivedMessagesQueue.Count > 0 ? receivedMessagesQueue.Dequeue() : null;
        }
    }

    public long QueueBufferedMessage()
    {
        long result = 0;
        if (clientSocket == null) return result;

        lock (sendBuffer)
        {
            try
            {
                if (sendBuffer.Length > 0)
                {
                    var data = new QueuedData { memoryStream = sendBuffer, socket = clientSocket };
                    taskScheduler.method_23(new GDelegate4(QueueSendCallback), data);
                    result = sendBuffer.Length;
                }
            }
            catch (SocketException ex)
            {
                OnReceiveError(ex.SocketErrorCode);
                Disconnect();
            }
        }

        return result;
    }

    public long SendAsync()
    {
        long result = 0;
        if (clientSocket == null) return result;

        lock (sendBuffer)
        {
            try
            {
                if (sendBuffer.Length > 0)
                {
                    clientSocket.BeginSend(sendBuffer.GetBuffer(), 0, (int)sendBuffer.Length, SocketFlags.None, SendCallback, clientSocket);
                    result = sendBuffer.Length;
                    sendBuffer.SetLength(0);
                    sendBuffer.Position = 0;
                }
            }
            catch (SocketException ex)
            {
                OnReceiveError(ex.SocketErrorCode);
                Disconnect();
            }
        }

        return result;
    }

    private void StartReceive(int offset)
    {
        if (clientSocket != null)
        {
            try
            {
                clientSocket.BeginReceive(receiveBuffer, offset, receiveBuffer.Length - offset, SocketFlags.None, ReceiveCallback, clientSocket);
            }
            catch (SocketException ex)
            {
                OnSocketError(ex.SocketErrorCode);
                Disconnect();
            }
        }
    }

    protected void BeginReceive() => StartReceive(0);

    private void ReceiveCallback(IAsyncResult result)
    {
        try
        {
            Socket socket = (Socket)result.AsyncState;
            int bytesRead = socket.EndReceive(result);
            totalBytesReceived += bytesRead;
            receiveBufferOffset += bytesRead;

            while (receiveBufferOffset > 4)
            {
                int msgLength = BitConverter.ToInt32(receiveBuffer, 0);
                if (msgLength > receiveBuffer.Length || msgLength <= 0)
                {
                    OnConnectionError(ConnectionError.BufferLengthTooLong);
                    Disconnect();
                    return;
                }

                int totalLength = 4 + msgLength;
                if (receiveBufferOffset < totalLength) break;

                byte[] msg = new byte[msgLength];
                Array.Copy(receiveBuffer, 4, msg, 0, msgLength);
                Array.Copy(receiveBuffer, totalLength, receiveBuffer, 0, receiveBufferOffset - totalLength);
                receiveBufferOffset -= totalLength;

                lock (receivedMessagesQueue)
                {
                    receivedMessagesQueue.Enqueue(msg);
                    totalMessagesQueued++;
                }
            }

            log.Information($"{GetRemoteEndPoint()} has new messages in queue (total: {totalMessagesQueued})");
            StartReceive(receiveBufferOffset);
        }
        catch (SocketException ex)
        {
            OnSocketError(ex.SocketErrorCode);
            Disconnect();
        }
    }

    private void SendCallback(IAsyncResult result)
    {
        lock (sendBuffer)
        {
            try
            {
                Socket socket = (Socket)result.AsyncState;
                int bytesSent = socket.EndSend(result);
                totalBytesSent += bytesSent;
            }
            catch (SocketException ex)
            {
                OnReceiveError(ex.SocketErrorCode);
                Disconnect();
            }
        }
    }

    private class QueuedData
    {
        public Socket socket;
        public MemoryStream memoryStream;
    }
}
