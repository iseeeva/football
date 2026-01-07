using System.Diagnostics.CodeAnalysis;
using Serilog;
using Sobee.Common;

namespace Sobee.TestServer.Match
{
    public class MatchRoomManager : Component
    {
        private static readonly ILogger _log = Logging.Get<MatchRoomManager>();
        private bool _isDisposed;

        private readonly Dictionary<Guid, MatchRoom> _rooms = new();

        public MatchRoomManager()
        {
            _log.Debug("{id} initialized.", Id);
        }

        public bool TryAdd(MatchRoom room)
        {
            if (_rooms.ContainsKey(room.Id))
            {
                _log.Warning("Room {id} already exists, skipping.", room.Id);
                return false;
            }

            _rooms.Add(room.Id, room);
            _log.Information("Room {id} added.", room.Id);
            return true;
        }

        public bool TryCreate([NotNullWhen(true)] out MatchRoom? matchRoom)
        {
            matchRoom = new MatchRoom();

            if (!TryAdd(matchRoom))
            {
                matchRoom.Dispose();
                matchRoom = null;
                return false;
            }

            return true;
        }

        public bool TryRemove(Guid roomId, [NotNullWhen(true)] out MatchRoom? room)
        {
            if (!_rooms.TryGetValue(roomId, out room))
            {
                _log.Warning("Room {id} not found for removal.", roomId);
                return false;
            }

            _log.Information("Room {id} removing. (players: {playerCount})", roomId, room.Players.Count);

            _rooms.Remove(roomId);
            room.Dispose();

            _log.Information("Room {id} removed.", roomId);
            return true;
        }

        public bool TryGetRoom(Guid roomId, [NotNullWhen(true)] out MatchRoom? room) =>
            _rooms.TryGetValue(roomId, out room);

        public override void Update(double delta)
        {
            if (_isDisposed)
                return;

            try
            {
                var now = DateTime.UtcNow;
                var expiredRooms = _rooms
                    .Where(x =>
                        x.Value.Players.Count == 0 &&
                        (now - x.Value.CreatedAt) > MatchRoom.MAX_IDLE_TIME)
                    .Select(x => x.Key)
                    .ToList();

                foreach (var id in expiredRooms)
                {
                    TryRemove(id, out _);
                }

                foreach (var room in _rooms.Values)
                {
                    room.Update(delta);
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Error during {id} update.", Id);
            }
        }

        public int Count => _rooms.Count;

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            if (disposing)
            {
                _log.Debug("Disposing {id} with {count} rooms.", Id, _rooms.Count);

                foreach (var room in _rooms.Values)
                    room.Dispose();

                _rooms.Clear();
            }

            base.Dispose(disposing);
        }
    }
}
