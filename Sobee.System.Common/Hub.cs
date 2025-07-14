using System.Net;
using System.Net.Sockets;
using Sobee.Common;
using Sobee.Messaging;

namespace Sobee.System.Common
{
    public class Hub : Component
    {
        public readonly int Port;
        private readonly Socket Server = new(SocketType.Stream, ProtocolType.Tcp);
        private readonly CancellationTokenSource Cancellation = new();

        public ClientManager? Clients { get; private set; }
        private MessageDispatch? ClientsDispatch;

        public RoomManager? Rooms { get; private set; }
        private MessageDispatch? RoomsDispatch;

        private static readonly Serilog.ILogger _log = Logging.Get<Hub>();
        private bool _isDisposed;

        public Hub(int port)
        {
            try
            {
                if (port <= IPEndPoint.MinPort || port >= IPEndPoint.MaxPort)
                {
                    _log.Error($"{nameof(port)} ({port}) must be in the valid port range.");
                    throw new ArgumentOutOfRangeException(nameof(port), "Port must be between MinPort and MaxPort.");
                }

                Port = port;
                Initialize(Port);
            }
            catch (Exception ex)
            {
                _log.Error($"Failed during construction: " + ex.GetBaseException(), ex);
                this.Dispose();
                throw;
            }
        }

        private void Initialize(int port)
        {
            try
            {
                _log.Information("{id} initializing...", this.Id);

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

                _log.Information("{id} ({port}) initialized.", this.Id, this.Port);
            }
            catch (Exception ex)
            {
                _log.Error($"Failed during initialization: {ex.Message}", ex);
                this.Dispose();
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
                _log.Information("{id} is shutting down.", this.Id);
            }
            catch (Exception ex)
            {
                _log.Error($"Error in start: {ex.Message}", ex);
                this.Dispose();
                throw;
            }
        }

        private void Connection(Socket socket)
        {
            try
            {
                _log.Information($"Connection established from {socket.RemoteEndPoint}");
                Clients?.Add(socket);
            }
            catch (Exception ex)
            {
                _log.Error($"Error handling connection: " + ex.GetBaseException(), ex);
            }
        }

        public override async Task Update()
        {
            try
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
            catch (Exception ex)
            {
                _log.Error($"Error during update: {ex.GetBaseException()}", ex);
                this.Dispose();
                throw;
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
                    _log.Error($"Error in tick: {ex.GetBaseException()}", ex);
                    this.Dispose();
                    throw;

                    // İsterseniz burada kısa bir bekleme ekleyebilirsiniz:
                    //await Task.Delay(10);
                }
            }
        }

        public void Stop()
        {
            if (!Cancellation.IsCancellationRequested)
            {
                Cancellation.Cancel();
                _log.Information("{id} is stopping...", this.Id);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                if (disposing)
                {
                    _log.Debug("{id} disposing.", this.Id);

                    Stop();
                    Server.Dispose();
                    Cancellation.Dispose();

                    Clients?.Dispose();
                    Clients = null;
                    ClientsDispatch = null;

                    Rooms?.Dispose();
                    Rooms = null;
                    RoomsDispatch = null;

                    _log.Debug("{id} disposed.", this.Id);
                }
            }

            base.Dispose(disposing);
        }
    }
}
