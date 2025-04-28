using System.Net.Sockets;
using Serilog;
using Sobee.Common;

namespace Sobee.System.Common
{
    public class ClientManager
    {
        private readonly ILogger log = Logging.Get<ClientManager>();

        private readonly List<Client> Clients = new();
        private readonly MessageDispatch Dispatch;

        public ClientManager(MessageDispatch dispatch)
        {
            Dispatch = dispatch ?? throw new ArgumentNullException(nameof(dispatch));
        }

        public async Task Update()
        {
            List<Client> disconnectedClients = new();

            lock (Clients)
            {
                foreach (var client in Clients)
                {
                    if (!client.IsConnected())
                    {
                        disconnectedClients.Add(client);
                    }
                }

                foreach (var client in disconnectedClients)
                {
                    log.Information("Client {id} removing due disconnection.", client.Id);

                    client.Dispose();
                    Clients.Remove(client);
                }
            }

            var updateTasks = Clients.Select(client => client.Update());
            await Task.WhenAll(updateTasks);
        }

        public bool Add(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client), "Client cannot be null.");

            lock (Clients)
            {
                if (Clients.Any(c => c.Id == client.Id))
                {
                    log.Warning("Client {id} already exists.", client.Id);
                    return false;
                }

                client.SetDispatchSource(Dispatch);
                Clients.Add(client);

                log.Information("Client {id} ({endPoint}) added.", client.Id, client.Socket.RemoteEndPoint);
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

                var client = new Client(uniqueId, socket);
                client.SetDispatchSource(Dispatch);

                lock (Clients)
                {
                    Clients.Add(client);
                    log.Information("Client {id} ({endPoint}) added.", client.Id, client.Socket.RemoteEndPoint);
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
                    log.Warning("Client {id} not found.", client.Id);
                    return false;
                }

                Clients.Remove(client);
                log.Information("Client {id} ({endPoint}) removed.", client.Id, client.Socket.RemoteEndPoint);

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
                log.Information("Client {id} ({endPoint}) removed.", client.Id, client.Socket.RemoteEndPoint);

                return true;
            }
        }

        public void Broadcast(Message message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            lock (Clients)
            {
                foreach (var client in Clients)
                {
                    client.SendMessage(message);
                }
            }
        }
    }
}
