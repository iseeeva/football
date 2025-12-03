using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Serilog;
using Sobee.Common;

namespace Sobee.TestServer.Match
{
    public class MatchRoomManager : Component
    {
        private static readonly ILogger _log = Logging.Get<MatchRoomManager>();
        private bool _isDisposed;

        private readonly ConcurrentDictionary<Guid, MatchRoom> _rooms = new();

        public MatchRoomManager()
        {
            _log.Debug("{id} initialized.", Id);
        }

        public virtual bool TryAdd(MatchRoom room)
        {
            if (_rooms.TryAdd(room.Id, room))
            {
                _log.Information("Room {id} added.", room.Id);
                return true;
            }

            _log.Warning("Room {id} already exists, skipping.", room.Id);
            return false;
        }

        public virtual bool TryCreate([NotNullWhen(true)] out MatchRoom? matchRoom)
        {
            var createdRoom = new MatchRoom();
            if (TryAdd(createdRoom))
            {
                matchRoom = createdRoom;
                return true;
            }

            createdRoom.Dispose();
            matchRoom = null;
            return false;
        }


        public virtual bool TryRemove(Guid roomId, [NotNullWhen(true)] out MatchRoom? room)
        {
            if (_rooms.TryRemove(roomId, out room))
            {
                room.Dispose();
                _log.Information("Room {id} removed.", roomId);
                return true;
            }
            else if (_rooms.ContainsKey(roomId))
            {
                _log.Warning("Failed to remove room {id}.", roomId);
                return false;
            }

            _log.Warning("Room {id} not found for removal.", roomId);
            return false;
        }

        public bool TryGetRoom(Guid roomId, [NotNullWhen(true)] out MatchRoom? room) =>
            _rooms.TryGetValue(roomId, out room);

        public override async Task Update(double delta)
        {
            if (_isDisposed)
                return;

            try
            {
                var now = DateTime.Now;

                var expiredRooms = _rooms
                    .Where(x =>
                        x.Value.Players.Count == 0 &&
                        (now - x.Value.CreatedAt).TotalMilliseconds > MatchRoom.MAX_IDLE_TIME.TotalMilliseconds)
                    .Select(x => x.Key)
                    .ToList();

                foreach (var id in expiredRooms)
                {
                    if (TryRemove(id, out var room))
                    {
                        _log.Information(
                            "Room {id} removed due to inactivity. (idle limit {@maxIdleSecond}s)",
                            id, MatchRoom.MAX_IDLE_TIME.TotalSeconds
                        );
                    }
                    else
                    {
                        _log.Warning("Failed to remove inactive room {id}.", id);
                    }
                }

                await Task.WhenAll(_rooms.Values.Select(room => room.Update(delta)));
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Error during {id} update.", Id);
            }
        }

        public int Count => _rooms.Count;

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed) return;
            _isDisposed = true;

            if (disposing)
            {
                _log.Debug("Disposing {id} with {count} rooms.", Id, _rooms.Count);

                foreach (var room in _rooms.Values)
                    room.Dispose();

                _rooms.Clear();
                _log.Debug("{id} disposed.", Id);
            }

            base.Dispose(disposing);
        }
    }
}
