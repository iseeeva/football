using System.Net;
using System.Net.Sockets;
using Sobee.Common;

namespace Sobee.System.Common
{
    public class Hub : IDisposable
    {
        public readonly int Port;
        private readonly Socket Server = new(SocketType.Stream, ProtocolType.Tcp);
        private readonly CancellationTokenSource Cancellation = new();
        private bool IsDisposed = false;

        private ClientManager? Clients;
        private MessageDispatch? Dispatch;

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

                Dispatch = new MessageDispatch(this);
                Dispatch.RegisterMessagesFromAssemblyName("Sobee.Messages.Common");
                Dispatch.RegisterMessageEvent(typeof(Messages.Common.Player.Information), Events.Common.Player.Initialize);

                Clients = new ClientManager(Dispatch);

                _ = Task.Run(() => Start(Cancellation.Token));
                _ = Task.Run(() => Tick(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()));

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
                Clients?.AddClient(socket);
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
        }

        private async Task Tick(double previous)
        {
            try
            {
                var starting = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                var delta = (starting - previous) / 1000.0;

                await Update();

                // Update all rooms (if applicable)
                // Rooms.Update(delta);

                var elapsed = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - starting;
                var delay = Math.Max(0, 1000.0 / 100 - elapsed); // TODO: FPS Needs Environment.

                if (delay > 0)
                {
                    await Task.Delay((int)delay);
                }

                await Tick(starting);
            }
            catch (Exception ex)
            {
                Log.Error($"Error in tick: " + ex.GetBaseException(), ex);
                await Tick(previous);
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
