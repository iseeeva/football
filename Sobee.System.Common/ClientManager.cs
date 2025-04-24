using System.Net.Sockets;

namespace Sobee.System.Common
{
    public class ClientManager
    {
        private readonly List<Client> Clients;
        private readonly MessageDispatcher DispatchGroup;

        public ClientManager(MessageDispatcher dispatchGroup)
        {
            Clients = new List<Client>();
            DispatchGroup = dispatchGroup ?? throw new ArgumentNullException(nameof(dispatchGroup));
        }

        public async Task Update()
        {
            var updateTasks = Clients.Select(client => client.Update());
            await Task.WhenAll(updateTasks);
        }

        public bool AddClient(Socket socket)
        {
            if (socket == null)
                throw new ArgumentNullException(nameof(socket), "Socket cannot be null.");

            try
            {
                Guid uniqueId;
                do
                {
                    uniqueId = Guid.NewGuid();
                } while (Clients.Any(c => c.GetId() == uniqueId));

                var client = new Client(uniqueId, socket);
                client.SetDispatchSource(DispatchGroup);

                lock (Clients)
                {
                    Clients.Add(client);
                }

                return true;
            }
            catch (SocketException ex)
            {
                // Daha spesifik bir hata türü yakalanıyor
                throw new InvalidOperationException("Failed to add client due to a socket error.", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An unexpected error occurred while adding a client.", ex);
            }
        }
    }
}
