using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using Sobee.Common;
using Sobee.Network;
using Sobee.TestServer.Auth;
using Sobee.TestServer.Match;

namespace Sobee.TestServer
{
    public class Hub : Component
    {
        private static readonly Serilog.ILogger _log = Logging.Get<Hub>();
        private bool _isDisposed;

        private readonly CancellationTokenSource _cancellation = new();
        private readonly Stopwatch _stopwatch = new();

        public readonly int Port;
        private readonly Socket _socket;

        private readonly AuthRoom _authRoom; // Authentication room
        public readonly MatchRoomManager MatchRoomManager = new(); // Match rooms

        private const double TargetFrameTimeMilliseconds = 1000.0 / 60.0;

        public Hub(int port)
        {
            if (port > IPEndPoint.MinPort || port < IPEndPoint.MaxPort)
                Port = port;
            else
                throw new ArgumentOutOfRangeException(nameof(port), $"{nameof(port)} ({port}) must be in the valid port range.");

            try
            {
                _log.Information("{id} initializing...", Id);
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _socket.Bind(new IPEndPoint(IPAddress.Loopback, Port));
                _socket.Listen(100);

                _authRoom = new AuthRoom(this);
                _ = Task.Run(() => AcceptClientsAsync(_cancellation.Token), _cancellation.Token);
                _ = Task.Run(() => TickAsync(_cancellation.Token), _cancellation.Token);

                _log.Information("{id} ({port}) initialized and listening.", Id, Port);
            }
            catch (Exception ex)
            {
                _log.Error($"Failed during start: {ex.GetBaseException()}", ex);
                Dispose();
                throw;
            }
        }

        private async Task AcceptClientsAsync(CancellationToken cancellationToken)
        {
            _log.Information("Client accept loop started.");
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    Socket clientSocket = await _socket.AcceptAsync(cancellationToken);

                    _log.Information("New client connected: {remoteEp}", clientSocket.RemoteEndPoint);
                    _authRoom.Users.TryAdd(new SocketWrapper(clientSocket));
                }
                catch (OperationCanceledException)
                {
                    _log.Information("Client accept loop stopping.");
                    break;
                }
                catch (Exception ex)
                {
                    _log.Error($"Error in accept loop: {ex.GetBaseException()}", ex);
                }
            }
            _log.Information("Client accept loop stopped.");
        }

        private async Task TickAsync(CancellationToken cancellationToken)
        {
            _log.Information("Server tick loop started.");
            _stopwatch.Start();
            double previous = _stopwatch.Elapsed.TotalMilliseconds;

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var starting = _stopwatch.Elapsed.TotalMilliseconds;
                    var delta = (starting - previous) / 1000.0;
                    previous = starting;

                    await Update(delta);

                    var elapsed = _stopwatch.Elapsed.TotalMilliseconds - starting;
                    var delay = Math.Max(0, TargetFrameTimeMilliseconds - elapsed);

                    if (delay > 0)
                    {
                        await Task.Delay((int)delay, cancellationToken);
                    }
                }
                catch (OperationCanceledException)
                {
                    _log.Information("Server tick loop stopping.");
                    break;
                }
                catch (Exception ex)
                {
                    _log.Error($"Critical error in tick loop: {ex.GetBaseException()}", ex);
                    await Task.Delay(1000, cancellationToken);
                }
            }
            _log.Information("Server tick loop stopped.");
        }


        public override async Task Update(double delta)
        {
            await _authRoom.Update(delta);
            await MatchRoomManager.Update(delta);
        }

        public void Stop()
        {
            if (!_cancellation.IsCancellationRequested)
            {
                _log.Information("{id} is stopping...", Id);
                _cancellation.Cancel();
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

                    try
                    {
                        Stop();

                        _socket.Dispose();
                        _authRoom.Dispose();
                        _cancellation.Dispose();
                    }
                    catch (Exception ex)
                    {
                        _log.Warning("Error disposing listener socket: {msg}", ex.Message);
                    }

                    _log.Debug("{id} disposed.", Id);
                }
            }
            base.Dispose(disposing);
        }
    }
}