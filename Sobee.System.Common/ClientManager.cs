using System.Net.Sockets;

namespace Sobee.System.Common
{
    public class ClientManager
    {
        private readonly List<Client> Clients = new();
        private readonly MessageDispatch Dispatch;

        public ClientManager(MessageDispatch dispatch)
        {
            Dispatch = dispatch ?? throw new ArgumentNullException(nameof(dispatch));
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
                } while (Clients.Any(c => c.Id == uniqueId));

                var client = new Client(uniqueId, socket);
                client.SetDispatchSource(Dispatch);

                lock (Clients)
                {
                    Clients.Add(client);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
