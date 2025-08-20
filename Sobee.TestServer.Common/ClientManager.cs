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

        private readonly List<Client> Clients = [];
        private readonly MessageDispatch Dispatch;

        public ClientManager(MessageDispatch dispatch)
        {
            Dispatch = dispatch ?? throw new ArgumentNullException(nameof(dispatch));
            _log.Debug("{id} initialized.", Id);
        }

        public override Task Update(double delta)
        {
            try
            {
                lock (Clients)
                {
                    List<Client> disconnectedClients = new();

                    foreach (var client in Clients.ToList())
                    {
                        if (!client.IsConnected)
                        {
                            disconnectedClients.Add(client);
                        }
                    }

                    foreach (var client in disconnectedClients)
                    {
                        _log.Information("Client {id} removing due disconnection.", client.Id);

                        client.Dispose();
                        Clients.Remove(client);
                    }
                }

                lock (Clients)
                {
                    foreach (var client in Clients.ToList())
                    {
                        if (client.IsConnected)
                        {
                            client.Update(delta);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error during client update: {message}", ex.Message);
                Dispose();
                throw;
            }

            return Task.CompletedTask;
        }

        public bool Add(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client), "Client cannot be null.");

            lock (Clients)
            {
                if (Clients.Any(c => c.Id == client.Id))
                {
                    _log.Warning("Client {id} already exists.", client.Id);
                    return false;
                }

                client.messageHandle.SetDispatchSource(Dispatch);
                Clients.Add(client);

                _log.Information("Client {id} ({endPoint}) added.", client.Id, client.Socket.RemoteEndPoint);
            }

            return true;
        }

        public bool Add(Socket socket)
        {
            if (socket == null)
                throw new ArgumentNullException(nameof(socket), "Socket cannot be null.");

            try
            {
                Guid uniqueId;

                do uniqueId = Guid.NewGuid();
                while (Clients.Any(c => c.Id == uniqueId));

                var client = new Client(socket); // TODO: SessionType need to be defined properly with Database
                client.messageHandle.SetDispatchSource(Dispatch);

                lock (Clients)
                {
                    Clients.Add(client);
                    _log.Information("Client {id} ({endPoint}) added.", client.Id, client.Socket.RemoteEndPoint);
                }

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
                throw new ArgumentNullException(nameof(client), "Client cannot be null.");

            lock (Clients)
            {
                if (!Clients.Contains(client))
                {
                    _log.Warning("Client {id} not found.", client.Id);
                    return false;
                }

                Clients.Remove(client);
                _log.Information("Client {id} ({endPoint}) removed.", client.Id, client.Socket.RemoteEndPoint);

                return true;
            }
        }

        public bool Remove(Guid clientId)
        {
            lock (Clients)
            {
                var client = Clients.FirstOrDefault(c => c.Id == clientId);
                if (client == null)
                {
                    return false;
                }

                Clients.Remove(client);
                _log.Information("Client {id} ({endPoint}) removed.", client.Id, client.Socket.RemoteEndPoint);

                return true;
            }
        }

        public void Broadcast(Message message)
        {
            ArgumentNullException.ThrowIfNull(message);

            lock (Clients)
            {
                foreach (var client in Clients)
                {
                    client.messageHandle.SendMessage(message);
                }
            }
        }

        public bool Contains(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client), "Client cannot be null.");

            lock (Clients)
            {
                return Clients.Contains(client);
            }
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

                    Clients.ForEach(client => client.Dispose());
                    Clients.Clear();

                    _log.Debug("{id} disposed.", Id);
                }
            }

            base.Dispose(disposing);
        }
    }
}
