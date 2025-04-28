using Serilog;
using Sobee.Common;

namespace Sobee.System.Common
{
    public class RoomManager
    {
        private readonly ILogger log = Logging.Get<RoomManager>();

        private readonly List<Room> Rooms = new();
        private readonly MessageDispatch Dispatch;

        public RoomManager(MessageDispatch dispatch)
        {
            Dispatch = dispatch ?? throw new ArgumentNullException(nameof(dispatch));
        }

        public async Task Update()
        {
            var updateTasks = Rooms.Select(room => room.Update());
            await Task.WhenAll(updateTasks);
        }

        public bool Add(Room room)
        {
            if (room == null)
                throw new ArgumentNullException(nameof(room), "Room cannot be null.");

            lock (Rooms)
            {
                if (Rooms.Any(c => c.Id == room.Id))
                {
                    log.Warning("Room {id} already exists.", room.Id);
                    return false;
                }

                Rooms.Add(room);
                log.Information("Room {id} added.", room.Id);
            }

            return true;
        }

        public Room Create()
        {
            try
            {
                Guid uniqueId;

                do uniqueId = Guid.NewGuid();
                while (Rooms.Any(c => c.Id == uniqueId));

                var room = new Room(uniqueId, Dispatch);

                lock (Rooms)
                {
                    Rooms.Add(room);
                    log.Information("Room {id} added.", room.Id);
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
                    log.Warning("Room {id} not found.", room.Id);
                    return false;
                }

                Rooms.Remove(room);
                log.Information("Room {id} removed.", room.Id);

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
                log.Information("Room {id} removed.", room.Id);

                return true;
            }
        }
    }
}
