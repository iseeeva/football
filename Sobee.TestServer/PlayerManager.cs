using System.Collections.Concurrent;
using Serilog;
using Sobee.Common;
using Sobee.Messaging;
using Sobee.Network;
using Sobee.TestServer.Common;

namespace Sobee.TestServer
{
    public class PlayerManager : Component
    {
        private static readonly ILogger _log = Logging.Get<PlayerManager>();
        private bool _isDisposed;

        private readonly RoomCommunication _comm;
        private readonly ConcurrentDictionary<Guid, Player> Players = new();

        public PlayerManager(RoomCommunication commReference)
        {
            _comm = commReference ?? throw new ArgumentNullException(nameof(commReference));
            _log.Debug("{id} initialized.", Id);
        }

        public override async Task Update(double delta)
        {
            try
            {
                var disconnected = Players.Values.Where(c => !c.IsActive).ToList();
                foreach (var client in disconnected)
                {
                    _log.Information("Client {id} removing due to disconnection.", client.Id);
                    Remove(client);
                    client.Dispose();
                }

                var updateTasks = Players.Values.Select(c => c.Update(delta));
                await Task.WhenAll(updateTasks);
            }
            catch (Exception ex)
            {
                _log.Error("Error during client update: {message}", ex.Message);
                Dispose();
                throw;
            }
        }

        public bool Add(SocketWrapper socket)
        {
            if (socket == null)
                throw new ArgumentNullException(nameof(socket));

            try
            {
                var client = new Player(socket, _comm); // TODO: Define SessionType properly with Database

                if (!Players.TryAdd(client.Id, client))
                {
                    _log.Warning("Client {id} could not be added.", client.Id);
                    return false;
                }

                _log.Information("Client {id} ({endPoint}) added.", client.Id);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public bool Remove(Player client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            if (!Players.TryRemove(client.Id, out _))
            {
                _log.Warning("Client {id} not found.", client.Id);
                return false;
            }

            _log.Information("Client {id} ({endPoint}) removed.", client.Id);
            return true;
        }

        public bool Remove(Guid clientId)
        {
            if (!Players.TryRemove(clientId, out _))
                return false;

            _log.Information("Client {id} removed.", clientId);
            return true;
        }

        public void Broadcast(Message message)
        {
            ArgumentNullException.ThrowIfNull(message);

            foreach (var client in Players.Values)
            {
                client.SendMessage(message);
            }
        }

        public bool Contains(Player client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            return Players.ContainsKey(client.Id);
        }

        public int Count => Players.Count;

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                if (disposing)
                {
                    _log.Debug("Disposing {id} with {count} clients.", Id, Players.Count);

                    foreach (var client in Players.Values)
                    {
                        client.Dispose();
                    }

                    Players.Clear();
                    _log.Debug("{id} disposed.", Id);
                }
            }

            base.Dispose(disposing);
        }
    }
}
