using System.Net;
using System.Net.Sockets;
using Net = System.Net.Sockets;

namespace Sobee.Network.Socket
{
    public class Hub : IDisposable
    {
        public readonly int Port;
        private readonly Net.Socket Server = new(SocketType.Stream, ProtocolType.Tcp);
        private readonly CancellationTokenSource Cancellation = new();
        private bool IsDisposed = false;

        public Hub(int Port)
        {
            if (Port > IPEndPoint.MinPort && Port < IPEndPoint.MaxPort)
            {
                this.Port = Port;
                Initialize(this.Port);
            }
            else
                Log.Error($"{nameof(Port)} ({Port}) must be in port range.");
        }

        private void Initialize(int Port)
        {
            try
            {
                Server.Bind(new IPEndPoint(IPAddress.Loopback, Port));
                Server.Listen();

                Task.Run(() => Start(Cancellation.Token));
                Task.Run(() => Tick(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()));

                Log.Information("Initialized.");
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to initialize: {ex.Message}");
                Dispose();
            }
        }

        private async Task Start(CancellationToken Cancellation)
        {
            try
            {
                while (!Cancellation.IsCancellationRequested)
                {
                    Net.Socket Client = await Server.AcceptAsync(Cancellation);
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

        private async Task Connection(Net.Socket Client)
        {
            Log.Information($"Connection from {Client.RemoteEndPoint}");
            var Buffer = new byte[4096]; // 4 Kilobayt

            try
            {
                while (true)
                {
                    int Received = await Client.ReceiveAsync(Buffer, SocketFlags.None);
                    if (Received > 0)
                    {
                        var Parsed = new Messaging.Events.Parser(Buffer);
                        Log.Information($"{Parsed.Id} received.");
                    }
                    else
                    {
                        Log.Information($"Connection from {Client.RemoteEndPoint} closed by client.");
                        break;
                    }

                    //await client.SendAsync(Buffer.AsMemory(0, bytesRead), SocketFlags.None);
                }
            }
            catch (SocketException ex)
            {
                Log.Error($"Socket error with {Client.RemoteEndPoint}: {ex.Message}");
            }
            catch (Exception ex)
            {
                Log.Error($"Unexpected error with {Client.RemoteEndPoint}: {ex.Message}");
            }
            finally
            {
                Client.Dispose();
                Log.Information($"Connection with {Client.RemoteEndPoint} closed.");
            }
        }

        private async Task Tick(double Previous)
        {
            try
            {
                var Starting = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                var Delta = (Starting - Previous) / 1000.0;

                // Update all rooms
                // Rooms.Update(delta);

                var Elapsed = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - Starting;
                var Delay = Math.Max(0, (1000.0 / 100) - Elapsed); // TODO: FPS Needs Environment.

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

        private static Serilog.ILogger Log
        {
            get { return Logger.Get<Hub>(); }
        }
    }
}
