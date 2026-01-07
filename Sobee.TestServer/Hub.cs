using System.Net;
using System.Net.Sockets;
using Sobee.Common;
using Sobee.Network;
using Sobee.Tasks;
using Sobee.TestServer.Auth;
using Sobee.TestServer.Match;

namespace Sobee.TestServer
{
    public sealed class Hub : TickController
    {
        private static readonly Serilog.ILogger _log = Logging.Get<Hub>();
        private bool _isDisposed;

        public readonly int Port;
        private readonly Socket _socket;

        public readonly AuthRoom AuthRoom;
        public readonly MatchRoomManager RoomManager;

        public Hub(int port) : base()
        {
            if (port <= IPEndPoint.MinPort || port >= IPEndPoint.MaxPort)
                throw new ArgumentOutOfRangeException(nameof(port));

            Port = port;

            try
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _socket.Bind(new IPEndPoint(IPAddress.Loopback, Port));
                _socket.Listen(100);
                _socket.Blocking = false;

                AuthRoom = new AuthRoom(this);
                RoomManager = new MatchRoomManager();
                TickStart();

                _log.Information("{id} initialized on port {port}.", Id, Port);
            }
            catch (Exception ex)
            {
                _log.Fatal(ex, "{id} failed to initialize on port {port}.", Id, Port);
                throw;
            }
        }

        public override void Update(double delta)
        {
            AcceptClients();
            AuthRoom.Update(delta);
            RoomManager.Update(delta);
            base.Update(delta);
        }

        private void AcceptClients()
        {
            if (_socket == null || !IsRunning)
                return;

            try
            {
                while (_socket.Poll(0, SelectMode.SelectRead))
                {
                    Socket clientSocket = _socket.Accept();
                    clientSocket.Blocking = false;
                    _log.Information("{id} client connected from {ep}", Id, clientSocket.RemoteEndPoint);

                    AuthRoom.Users.TryCreate(new SocketWrapper(clientSocket));
                }
            }
            catch (ObjectDisposedException)
            {

            }
            catch (SocketException ex)
            {
                _log.Error(ex, "{id} error while accepting client.", Id);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            if (disposing)
            {
                TickStop();

                try
                {
                    if (_socket.Connected)
                        _socket.Shutdown(SocketShutdown.Both);

                    _socket.Close();
                    _socket.Dispose();
                }
                catch (Exception ex)
                {
                    _log.Debug(ex, "{id} socket dispose error.", Id);
                }

                AuthRoom?.Dispose();

                _log.Debug("{id} disposed.", Id);
            }

            base.Dispose(disposing);
        }
    }
}