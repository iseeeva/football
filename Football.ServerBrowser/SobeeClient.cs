using Football.Network;
using Football.Network.Messaging;
using Football.Serialization;
using Football.ServerBrowser.Messages;
using System.Net.Sockets;

namespace Football.ServerBrowser
{
    public sealed class SobeeClient : IDisposable
    {
        private TcpClient _tcpClient;
        private SocketWrapper _socketWrapper;
        private readonly MessageCommunication _communication = new();

        private TaskCompletionSource<Network.Messaging.Message>? _receiveTcs;
        private readonly object _tcsLock = new();
        private readonly SemaphoreSlim _networkSemaphore = new SemaphoreSlim(1, 1);
        private CancellationTokenSource _loopCts;

        public string Host { get; }
        public int Port { get; }
        public bool IsConnected => _socketWrapper != null && _socketWrapper.IsConnected;

        public SobeeClient(string host, int port)
        {
            Host = host;
            Port = port;

            _communication.RegisterMessages("Football.ServerBrowser.Messages");
            _communication.RegisterMessages("Football.GameServer.Messages");

        }

        public async Task ConnectAsync()
        {
            _tcpClient = new TcpClient();
            await _tcpClient.ConnectAsync(Host, Port);

            _socketWrapper = new SocketWrapper(_tcpClient.Client);

            _socketWrapper.DisconnectHandler += (s, e) => CancelPendingRequests("Connection closed by host server.");
            _socketWrapper.ErrorHandler += (s, e) => CancelPendingRequests($"Socket network failure: {e.SocketError}");

            _socketWrapper.Start();

            _loopCts = new CancellationTokenSource();
            _ = Task.Run(ProcessIncomingPacketsLoop, _loopCts.Token);
        }

        public void SendMessage(Network.Messaging.Message message)
        {
            if (!IsConnected)
                throw new InvalidOperationException("Infrastructure pipeline channel offline.");

            using (var ms = new MemoryStream())
            using (var serializer = new MessageSerialization(ms, _communication.GetMessageConstructor(), _communication.GetMessageIdFromType()))
            {
                serializer.WriteMessage(message);
                byte[] packetBytes = ms.ToArray();
                _socketWrapper.QueueSend(packetBytes, 0, packetBytes.Length);
            }

            _socketWrapper.BeginSendBuffered();
        }

        public async Task<BrowserUserDetailsResponseMessage?> GetUserDetailsFromDb()
        {
            if (!IsConnected)
                return null;

            var response = await SendRequestAsync(new BrowserUserDetailsRequestMessage());
            if (response is not BrowserUserDetailsResponseMessage authResp)
                return null;

            return authResp;
        }

        public async Task<IMessage> SendRequestAsync(Network.Messaging.Message requestMessage, int timeoutMilliseconds = 7000)
        {
            await _networkSemaphore.WaitAsync();
            try
            {
                lock (_tcsLock)
                {
                    _receiveTcs = new TaskCompletionSource<Network.Messaging.Message>(TaskCreationOptions.RunContinuationsAsynchronously);
                }

                SendMessage(requestMessage);

                using (var cts = new CancellationTokenSource(timeoutMilliseconds))
                {
                    using (cts.Token.Register(() =>
                    {
                        lock (_tcsLock) _receiveTcs?.TrySetException(new TimeoutException("Server identity verification frame response timed out."));
                    }))
                    {
                        return await _receiveTcs.Task;
                    }
                }
            }
            finally
            {
                lock (_tcsLock) _receiveTcs = null;
                _networkSemaphore.Release();
            }
        }

        private void ProcessIncomingPacketsLoop()
        {
            while (_loopCts != null && !_loopCts.IsCancellationRequested && IsConnected)
            {
                try
                {
                    byte[] packetData = _socketWrapper.DequeuePacket();

                    if (packetData == null)
                    {
                        Thread.Sleep(5);
                        continue;
                    }

                    using (var ms = new MemoryStream(packetData))
                    using (var serializer = new MessageSerialization(ms, _communication.GetMessageConstructor(), _communication.GetMessageIdFromType()))
                    {
                        var incomingMessage = serializer.ReadMessage() as Network.Messaging.Message;
                        if (incomingMessage != null)
                        {
                            lock (_tcsLock)
                            {
                                _receiveTcs?.TrySetResult(incomingMessage);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    {
                        throw new Exception("[ProcessIncomingPacketsLoop] Packet processing failed, skipping packet", ex);
                    }
                }
            }
        }

        private void CancelPendingRequests(string reason)
        {
            lock (_tcsLock)
            {
                _receiveTcs?.TrySetException(new IOException(reason));
            }
        }

        public void Dispose()
        {
            if (_loopCts != null && !_loopCts.IsCancellationRequested)
                _loopCts?.Cancel();

            CancelPendingRequests("Authentication client engine component disposed.");
            _socketWrapper?.Stop();
            _socketWrapper?.Dispose();
            _tcpClient?.Dispose();
            _loopCts?.Dispose();
            _networkSemaphore?.Dispose();
        }
    }
}