using System.Net.Sockets;

namespace Sobee.System.Common
{
    public class ClientManager(DispatchHelper dispatchGroup)
    {
        private List<Client> Clients = new();
        private DispatchHelper DispatchGroup = dispatchGroup;

        public async Task Update()
        {
            foreach (Client Client in Clients)
            {
                Client.Update();
            }
        }

        public bool AddClient(Socket Socket)
        {
            try
            {
                Guid uniqueId;
                do { uniqueId = Guid.NewGuid(); }
                while (Clients.Any(c => c.GetId() == uniqueId));

                var Client = new Client(uniqueId, Socket);
                Client.SetDispatchSource(DispatchGroup);
                Clients.Add(Client);

                return Clients.Contains(Client);
            }
            catch (Exception ex)
            {
                throw new Exception($"Client adding failed: " + ex.Message);
            }
        }
    }
}
