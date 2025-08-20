using System.Collections.Concurrent;
using Serilog;
using Sobee.Common;
using Sobee.Messaging;

namespace Sobee.TestServer.Common
{
    public class RoomManager : Component
    {
        private static readonly ILogger _log = Logging.Get<RoomManager>();
        private bool _isDisposed;

        private readonly ConcurrentDictionary<Guid, Room> Rooms = new();
        private readonly MessageDispatch Dispatch;

        public RoomManager(MessageDispatch dispatch)
        {
            Dispatch = dispatch ?? throw new ArgumentNullException(nameof(dispatch));
            _log.Debug("{id} initialized.", Id);
        }

        public override async Task Update(double delta)
        {
            if (_isDisposed) return;

            try
            {
                var inactiveRooms = Rooms.Values.Where(room =>
                    room.Clients.Count == 0 &&
                    ((DateTime.Now - room.CreatedAt).TotalMilliseconds > Room.MAX_IDLE_TIME)
                ).ToList();

                foreach (var room in inactiveRooms)
                {
                    _log.Information("Room {id} removing due to inactivity.", room.Id);
                    Remove(room);
                    room.Dispose();
                }

                var updateTasks = Rooms.Values.Select(room => room.Update(delta));
                await Task.WhenAll(updateTasks);
            }
            catch (Exception ex)
            {
                _log.Error("Error during room update: {message}", ex.Message);
                Dispose();
                throw;
            }
        }

        public bool Add(Room room)
        {
            if (room == null)
                throw new ArgumentNullException(nameof(room), "Room cannot be null.");

            if (!Rooms.TryAdd(room.Id, room))
            {
                _log.Warning("Room {id} already exists.", room.Id);
                return false;
            }

            _log.Information("Room {id} added.", room.Id);
            return true;
        }

        public Room Create()
        {
            try
            {
                Room room = new Room(Dispatch);

                if (!Rooms.TryAdd(room.Id, room))
                {
                    throw new Exception($"Room {room.Id} could not be added.");
                }

                _log.Information("Room {id} created.", room.Id);
                return room;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public bool Remove(Room room)
        {
            if (room == null)
                throw new ArgumentNullException(nameof(room), "Room cannot be null.");

            if (!Rooms.TryRemove(room.Id, out _))
            {
                _log.Warning("Room {id} not found.", room.Id);
                return false;
            }

            _log.Information("Room {id} removed.", room.Id);
            return true;
        }

        public bool Remove(Guid roomId)
        {
            if (!Rooms.TryRemove(roomId, out _))
            {
                return false;
            }

            _log.Information("Room {id} removed.", roomId);
            return true;
        }

        public Room? FindRoomByClient(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client), "Client cannot be null.");

            return Rooms.Values.FirstOrDefault(room => room.Clients.Contains(client));
        }

        public int Count => Rooms.Count;

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                if (disposing)
                {
                    _log.Debug("Disposing {id} with {count} rooms.", Id, Rooms.Count);

                    foreach (var room in Rooms.Values)
                    {
                        room.Dispose();
                    }

                    Rooms.Clear();
                    _log.Debug("{id} disposed.", Id);
                }
            }

            base.Dispose(disposing);
        }
    }
}
