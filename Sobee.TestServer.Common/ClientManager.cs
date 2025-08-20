using System.Collections.Concurrent;
using System.Net.Sockets;
using Serilog;
using Sobee.Common;
using Sobee.Messaging;

namespace Sobee.TestServer.Common
{
    public class ClientManager : Component
    {
        private static readonly ILogger _log = Logging.Get<ClientManager>();
        private bool _isDisposed;

        private readonly ConcurrentDictionary<Guid, Client> Clients = new();
        private readonly MessageDispatch Dispatch;

        public ClientManager(MessageDispatch dispatch)
        {
            Dispatch = dispatch ?? throw new ArgumentNullException(nameof(dispatch));
            _log.Debug("{id} initialized.", Id);
        }

        public override async Task Update(double delta)
        {
            try
            {
                // Remove disconnected clients
                var disconnected = Clients.Values.Where(c => !c.IsConnected).ToList();
                foreach (var client in disconnected)
                {
                    _log.Information("Client {id} removing due to disconnection.", client.Id);
                    Remove(client);  // Thread-safe removal
                    client.Dispose();
                }

                // Update remaining clients
                var updateTasks = Clients.Values.Select(c => c.Update(delta));
                await Task.WhenAll(updateTasks);
            }
            catch (Exception ex)
            {
                _log.Error("Error during client update: {message}", ex.Message);
                Dispose();
                throw;
            }
        }

        public bool Add(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            client.messageHandle.SetDispatchSource(Dispatch);

            if (!Clients.TryAdd(client.Id, client))
            {
                _log.Warning("Client {id} already exists.", client.Id);
                return false;
            }

            _log.Information("Client {id} ({endPoint}) added.", client.Id, client.Socket.RemoteEndPoint);
            return true;
        }

        public bool Add(Socket socket)
        {
            if (socket == null)
                throw new ArgumentNullException(nameof(socket));

            try
            {
                var client = new Client(socket); // TODO: Define SessionType properly with Database
                client.messageHandle.SetDispatchSource(Dispatch);

                if (!Clients.TryAdd(client.Id, client))
                {
                    _log.Warning("Client {id} could not be added.", client.Id);
                    return false;
                }

                _log.Information("Client {id} ({endPoint}) added.", client.Id, client.Socket.RemoteEndPoint);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public bool Remove(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            if (!Clients.TryRemove(client.Id, out _))
            {
                _log.Warning("Client {id} not found.", client.Id);
                return false;
            }

            _log.Information("Client {id} ({endPoint}) removed.", client.Id, client.Socket.RemoteEndPoint);
            return true;
        }

        public bool Remove(Guid clientId)
        {
            if (!Clients.TryRemove(clientId, out _))
                return false;

            _log.Information("Client {id} removed.", clientId);
            return true;
        }

        public void Broadcast(Message message)
        {
            ArgumentNullException.ThrowIfNull(message);

            foreach (var client in Clients.Values)
            {
                client.messageHandle.SendMessage(message);
            }
        }

        public bool Contains(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            return Clients.ContainsKey(client.Id);
        }

        public int Count => Clients.Count;

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                if (disposing)
                {
                    _log.Debug("Disposing {id} with {count} clients.", Id, Clients.Count);

                    foreach (var client in Clients.Values)
                    {
                        client.Dispose();
                    }

                    Clients.Clear();
                    _log.Debug("{id} disposed.", Id);
                }
            }

            base.Dispose(disposing);
        }
    }
}
