using Sobee.Common;
using Sobee.Network;
using Sobee.TestServer.Auth;
using System.Net;
using System.Net.Sockets;

namespace Sobee.TestServer
{
    public sealed class Hub : ComponentManager<Component>
    {
        public readonly int Port;
        public readonly Socket Socket;

        public readonly AuthRoom AuthRoom;
        public readonly MatchRoomManager RoomManager;

        #region Constructor
        public Hub(int port)
        {
            if (port <= IPEndPoint.MinPort || port >= IPEndPoint.MaxPort)
                throw new ArgumentOutOfRangeException(nameof(port));

            Port = port;

            try
            {
                Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Socket.Bind(new IPEndPoint(IPAddress.Loopback, Port));
                Socket.Listen(100);
                Socket.Blocking = false;

                AuthRoom = new AuthRoom();
                AddComponent(AuthRoom);

                RoomManager = new MatchRoomManager();
                AddComponent(RoomManager);

                _log.Information("initialized on port {port}.", Port);
            }
            catch (Exception ex)
            {
                _log.Error(ex, "failed to initialize on port {port}.", Port);
                throw;
            }
        }
        #endregion

        #region Lifecycle
        //protected override void OnStart()
        //{
        //    base.OnStart();
        //}

        //protected override void OnStop()
        //{
        //    base.OnStop();
        //}

        protected override void OnUpdate(double delta)
        {
            base.OnUpdate(delta);
            AcceptClients();
        }

        private void AcceptClients()
        {
            if (!IsRunning) return;

            try
            {
                while (Socket.Poll(0, SelectMode.SelectRead))
                {
                    Socket clientSocket = Socket.Accept();
                    clientSocket.Blocking = false;
                    _log.Information("client connected from {ep}", clientSocket.RemoteEndPoint);
                    AuthRoom.Users.TryCreate(new SocketWrapper(clientSocket));
                }
            }
            catch (ObjectDisposedException) { }
            catch (SocketException ex)
            {
                _log.Error(ex, "error while accepting client.");
            }
        }
        #endregion

        #region Dispose
        protected override void OnDispose()
        {
            try
            {
                if (Socket.Connected)
                    Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();
                Socket.Dispose();
            }
            catch (Exception ex)
            {
                _log.Information(ex, "socket dispose error.");
            }

            base.OnDispose();
        }
        #endregion
    }
}