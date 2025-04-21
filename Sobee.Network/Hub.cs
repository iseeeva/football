using System.Net;
using System.Net.Sockets;
using Sobee.Common;
using Sobee.Messages.Hub;

namespace Sobee.Network
{
    public class Hub : IDisposable
    {
        public readonly int Port;
        private readonly Socket Server = new(SocketType.Stream, ProtocolType.Tcp);
        private readonly CancellationTokenSource Cancellation = new();
        private bool IsDisposed = false;

        private List<GClass300> Guests = new();
        private DispatchHelper Dispatch = new();

        private static Serilog.ILogger Log = Logging.Get<Hub>();

        public Hub(int Port)
        {
            try
            {
                if (Port > IPEndPoint.MinPort && Port < IPEndPoint.MaxPort)
                {
                    //DispatchHelper.RegisterMessagesFromAssemblyName("Sobee.Messages.Hub");

                    this.Port = Port;
                    Initialize(this.Port);
                }
                else
                    Log.Error($"{nameof(Port)} ({Port}) must be in port range.");
            }
            catch (Exception ex)
            {
                Log.Error($"Failed on construction: {ex.Message}");
                Dispose();
            }
        }

        private void Initialize(int Port)
        {
            try
            {
                Server.Bind(new IPEndPoint(IPAddress.Loopback, Port));
                Server.Listen();

                Dispatch.RegisterMessagesFromAssemblyName("Sobee.Messages.Hub");
                Dispatch.RegisterMessageEvent(typeof(PlayerInitializeMessage), PlayerInitializeMessage.TestEvent);

                Task.Run(() => Start(Cancellation.Token));
                Task.Run(() => Tick(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()));

                Log.Information("Initialized.");
            }
            catch (Exception ex)
            {
                Log.Error($"Failed on initialize: {ex.Message}");
                Dispose();
            }
        }

        private async Task Start(CancellationToken Cancellation)
        {
            try
            {
                while (!Cancellation.IsCancellationRequested)
                {
                    Socket Client = await Server.AcceptAsync(Cancellation);
                    await Connection(Client);
                }
            }
            catch (OperationCanceledException)
            {
                Log.Error("Shutting down.");
            }
            catch (Exception ex)
            {
                Log.Error($"Error in loop: {ex.Message}");
            }
        }

        private async Task Connection(Socket Socket)
        {
            Log.Information($"Connection from {Socket.RemoteEndPoint}");

            var Guest = new GClass300(Socket);
            Guest.SetDispatchSource(Dispatch);

            Guests.Add(Guest);
        }

        private async Task Update()
        {
            foreach (GClass300 Guest in Guests)
            {
                Guest.Update();
            }
        }

        private async Task Tick(double Previous)
        {
            try
            {
                var Starting = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                var Delta = (Starting - Previous) / 1000.0;

                await Update();

                // Update all rooms
                // Rooms.Update(delta);

                var Elapsed = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - Starting;
                var Delay = Math.Max(0, 1000.0 / 100 - Elapsed); // TODO: FPS Needs Environment.

                if (Delay > 0)
                {
                    await Task.Delay((int)Delay);
                }

                await Tick(Starting);
            }
            catch (Exception ex)
            {
                Log.Error("Error in Tick function:", ex);
                await Tick(Previous);
            }
        }

        public void Stop()
        {
            if (!Cancellation.IsCancellationRequested)
            {
                Cancellation.Cancel();
                Cancellation.Dispose();

                Log.Information("Server is stopping...");
            }
        }

        public void Dispose()
        {
            if (IsDisposed) return;

            Stop();
            Server.Dispose();
            GC.SuppressFinalize(this);

            IsDisposed = true;
            Log.Information("Resources disposed.");
        }

    }
}
