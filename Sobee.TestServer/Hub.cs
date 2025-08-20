using System.Net;
using System.Net.Sockets;
using Sobee.Common;
using Sobee.Messaging;
using Sobee.Network;
using Sobee.TestServer.Common;

namespace Sobee.TestServer
{
    public class Hub : Session
    {
        public readonly int Port;
        private readonly CancellationTokenSource Cancellation = new();

        public ClientManager? Clients { get; private set; }
        private MessageDispatch? ClientsDispatch;

        public RoomManager? Rooms { get; private set; }
        private MessageDispatch? RoomsDispatch;

        private static readonly Serilog.ILogger _log = Logging.Get<Hub>();
        private bool _isDisposed;

        public Hub(int port) : base(new(SocketType.Stream, ProtocolType.Tcp), SessionType.Game)
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
                Dispose();
                throw;
            }
        }

        private void Initialize(int port)
        {
            try
            {
                _log.Information("{id} initializing...", Id);

                Socket.Bind(new IPEndPoint(IPAddress.Loopback, port));
                Socket.Listen();

                ClientsDispatch = new MessageDispatch(this);
                ClientsDispatch.RegisterMessagesFromAssemblyName("Sobee.TestServer.Messages");
                ClientsDispatch.RegisterMessageEvent(typeof(Messages.Player.PlayerInformation), Events.Player.Information);
                Clients = new ClientManager(ClientsDispatch);

                RoomsDispatch = new MessageDispatch(this);
                RoomsDispatch.RegisterMessagesFromAssemblyName("Sobee.TestServer.Messages");
                RoomsDispatch.RegisterMessageEvent(typeof(Messages.Chat.ChatMessage), Events.Chat.Messaging);
                RoomsDispatch.RegisterMessageEvent(typeof(Messages.Player.PlayerMovePressed), Events.Player.MovePressed);
                Rooms = new RoomManager(RoomsDispatch);

                _ = Task.Run(() => Start(Cancellation.Token));
                _ = Task.Run(() => Tick(Cancellation.Token));

                _log.Information("{id} ({port}) initialized.", Id, Port);
            }
            catch (Exception ex)
            {
                _log.Error($"Failed during initialization: {ex.Message}", ex);
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
                    var clientSocket = await Socket.AcceptAsync(cancellationToken);
                    _ = Task.Run(() => Connection(clientSocket));
                }
            }
            catch (OperationCanceledException)
            {
                _log.Information("{id} is shutting down.", Id);
            }
            catch (Exception ex)
            {
                _log.Error($"Error in start: {ex.Message}", ex);
                Dispose();
                throw;
            }
        }

        private void Connection(Socket socket)
        {
            try
            {
                _log.Debug($"Connection established from {socket.RemoteEndPoint}");
                Clients?.Add(socket);
            }
            catch (Exception ex)
            {
                _log.Error($"Error handling connection: " + ex.GetBaseException(), ex);
            }
        }

        public override async Task Update(double delta)
        {
            try
            {
                if (Clients != null)
                {
                    await Clients.Update(delta);
                }

                if (Rooms != null)
                {
                    await Rooms.Update(delta);
                }
            }
            catch (Exception ex)
            {
                _log.Error($"Error during update: {ex.GetBaseException()}", ex);
                Dispose();
                throw;
            }
        }

        private async Task Tick(CancellationToken cancellationToken)
        {
            double previous = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var starting = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    var delta = (starting - previous) / 1000.0;

                    await Update(delta);

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
                    Dispose();
                    throw;
                }
            }
        }

        public void Stop()
        {
            if (!Cancellation.IsCancellationRequested)
            {
                Cancellation.Cancel();
                _log.Information("{id} is stopping...", Id);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                if (disposing)
                {
                    _log.Debug("{id} disposing.", Id);

                    Stop();
                    Socket.Dispose();
                    Cancellation.Dispose();

                    Clients?.Dispose();
                    Clients = null;
                    ClientsDispatch = null;

                    Rooms?.Dispose();
                    Rooms = null;
                    RoomsDispatch = null;

                    _log.Debug("{id} disposed.", Id);
                }
            }

            base.Dispose(disposing);
        }
    }
}
