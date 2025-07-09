using System.Net;
using System.Net.Sockets;
using Sobee.Common;
using Sobee.Messaging;

namespace Sobee.System.Common
{
    public class Hub : IDisposable
    {
        public readonly int Port;
        private readonly Socket Server = new(SocketType.Stream, ProtocolType.Tcp);
        private readonly CancellationTokenSource Cancellation = new();
        private bool IsDisposed = false;

        public ClientManager? Clients { get; private set; }
        private MessageDispatch? ClientsDispatch;

        public RoomManager? Rooms { get; private set; }
        private MessageDispatch? RoomsDispatch;

        private static readonly Serilog.ILogger Log = Logging.Get<Hub>();

        public Hub(int port)
        {
            if (port <= IPEndPoint.MinPort || port >= IPEndPoint.MaxPort)
            {
                Log.Error($"{nameof(port)} ({port}) must be in the valid port range.");
                throw new ArgumentOutOfRangeException(nameof(port), "Port must be between MinPort and MaxPort.");
            }

            Port = port;

            try
            {
                Initialize(Port);
            }
            catch (Exception ex)
            {
                Log.Error($"Failed during construction: " + ex.GetBaseException(), ex);
                Dispose();
                throw;
            }
        }

        private void Initialize(int port)
        {
            try
            {
                Server.Bind(new IPEndPoint(IPAddress.Loopback, port));
                Server.Listen();

                ClientsDispatch = new MessageDispatch(this);
                ClientsDispatch.RegisterMessagesFromAssemblyName("Sobee.Messages.Common");
                ClientsDispatch.RegisterMessageEvent(typeof(Messages.Common.Player.Information), Events.Player.Information);

                RoomsDispatch = new MessageDispatch(this);
                RoomsDispatch.RegisterMessagesFromAssemblyName("Sobee.Messages.Common");
                RoomsDispatch.RegisterMessageEvent(typeof(Messages.Common.Chat.Messaging), Events.Chat.Messaging);

                Clients = new ClientManager(ClientsDispatch);
                Rooms = new RoomManager(RoomsDispatch);

                _ = Task.Run(() => Start(Cancellation.Token));
                _ = Task.Run(() => Tick());

                Log.Information("Hub initialized successfully.");
            }
            catch (Exception ex)
            {
                Log.Error($"Failed during initialization: {ex.Message}", ex);
                Dispose();
                throw;
            }
        }

        private async Task Start(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var clientSocket = await Server.AcceptAsync(cancellationToken);
                    _ = Task.Run(() => Connection(clientSocket));
                }
            }
            catch (OperationCanceledException)
            {
                Log.Information("Server is shutting down.");
            }
            catch (Exception ex)
            {
                Log.Error($"Error in Start loop: {ex.Message}", ex);
            }
        }

        private async Task Connection(Socket socket)
        {
            try
            {
                Log.Information($"Connection established from {socket.RemoteEndPoint}");
                Clients?.Add(socket);
            }
            catch (Exception ex)
            {
                Log.Error($"Error handling connection: " + ex.GetBaseException(), ex);
            }
        }

        private async Task Update()
        {
            if (Clients != null)
            {
                await Clients.Update();
            }

            if (Rooms != null)
            {
                await Rooms.Update();
            }
        }

        private async Task Tick()
        {
            double previous = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            while (true)
            {
                try
                {
                    var starting = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    var delta = (starting - previous) / 1000.0;

                    await Update();

                    // Rooms.Update(delta); // varsa

                    var elapsed = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - starting;
                    var delay = Math.Max(0, 1000.0 / 100 - elapsed); // 100 FPS hedef

                    if (delay > 0)
                    {
                        await Task.Delay((int)delay);
                    }

                    previous = starting;
                }
                catch (Exception ex)
                {
                    Log.Error($"Error in tick: {ex.GetBaseException()}", ex);
                    // İsterseniz burada kısa bir bekleme ekleyebilirsiniz:
                    await Task.Delay(10);
                }
            }
        }

        public void Stop()
        {
            if (!Cancellation.IsCancellationRequested)
            {
                Cancellation.Cancel();
                Log.Information("Server is stopping...");
            }
        }

        public void Dispose()
        {
            if (IsDisposed) return;

            Stop();
            Server.Dispose();
            Cancellation.Dispose();
            GC.SuppressFinalize(this);

            IsDisposed = true;
            Log.Information("Resources disposed.");
        }
    }
}
