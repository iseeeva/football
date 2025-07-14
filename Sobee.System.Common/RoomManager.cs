using Serilog;
using Sobee.Common;
using Sobee.Messaging;

namespace Sobee.System.Common
{
    public class RoomManager : Component
    {
        private readonly ILogger _log = Logging.Get<RoomManager>();
        private bool _isDisposed;

        private readonly List<Room> Rooms = new();
        private readonly MessageDispatch Dispatch;

        public RoomManager(MessageDispatch dispatch)
        {
            Dispatch = dispatch ?? throw new ArgumentNullException(nameof(dispatch));
        }

        public override async Task Update()
        {
            if (_isDisposed) return;

            try
            {
                var updateTasks = Rooms.Select(room => room.Update());
                await Task.WhenAll(updateTasks);
            }
            catch (Exception ex)
            {
                _log.Error("Error during room update: {message}", ex.Message);
                this.Dispose();
                throw;
            }
        }

        public bool Add(Room room)
        {
            if (room == null)
                throw new ArgumentNullException(nameof(room), "Room cannot be null.");

            lock (Rooms)
            {
                if (Rooms.Any(c => c.Id == room.Id))
                {
                    _log.Warning("Room {id} already exists.", room.Id);
                    return false;
                }

                Rooms.Add(room);
                _log.Information("Room {id} added.", room.Id);
            }

            return true;
        }

        public Room Create()
        {
            try
            {
                Room room = new Room(Dispatch);

                lock (Rooms)
                {
                    Rooms.Add(room);
                    _log.Information("Room {id} created.", room.Id);
                }

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

            lock (Rooms)
            {
                if (!Rooms.Contains(room))
                {
                    _log.Warning("Room {id} not found.", room.Id);
                    return false;
                }

                Rooms.Remove(room);
                _log.Information("Room {id} removed.", room.Id);

                return true;
            }
        }

        public bool Remove(Guid roomId)
        {
            lock (Rooms)
            {
                var room = Rooms.FirstOrDefault(c => c.Id == roomId);
                if (room == null)
                {
                    return false;
                }

                Rooms.Remove(room);
                _log.Information("Room {id} removed.", room.Id);

                return true;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                if (disposing)
                {
                    _log.Debug("Disposing {id} with {count} rooms.", this.Id, Rooms.Count);

                    Rooms.ForEach(room => room.Dispose());
                    Rooms.Clear();

                    _log.Debug("{id} disposed.", this.Id);
                }
            }

            base.Dispose(disposing);
        }
    }
}
