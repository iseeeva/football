using Football.Common;
using Football.Network;
using System.Diagnostics.CodeAnalysis;

namespace Football.GameServer.Game
{
    public class GameRoomManager<R, A> : Component
        where R : GameRoom<R, A>, new()
        where A : Session
    {
        protected readonly Dictionary<Guid, R> _rooms = new();
        protected readonly List<R> _roomUpdateList = new();

        public R? this[Guid roomId] => _rooms.TryGetValue(roomId, out var r) ? r : null;
        public int Count => _rooms.Count;

        #region Rooms
        public bool TryAdd(R room)
        {
            if (_rooms.ContainsKey(room.Id))
            {
                _log.Warning("room ({id}) already exists, skipping.", room.Id);
                return false;
            }

            _rooms.Add(room.Id, room);
            _roomUpdateList.Add(room);
            room.Start();

            _log.Information("room ({id}) added.", room.Id);
            return true;
        }

        public bool TryCreate([NotNullWhen(true)] out R? room)
        {
            room = new R();
            if (!TryAdd(room))
            {
                room.Dispose();
                room = null;
                return false;
            }
            return true;
        }

        public bool TryRemove(Guid roomId, [NotNullWhen(true)] out R? room)
        {
            if (!_rooms.TryGetValue(roomId, out room))
            {
                _log.Warning("room ({id}) not found for removal.", roomId);
                return false;
            }

            _log.Information("room ({id}) removing.", roomId);

            _rooms.Remove(roomId);
            _roomUpdateList.Remove(room);
            room.Dispose();

            _log.Information("room ({id}) removed.", roomId);
            return true;
        }

        public bool TryGet(Guid roomId, [NotNullWhen(true)] out R? room)
            => _rooms.TryGetValue(roomId, out room);

        public List<R> GetAll()
           => _rooms.Values.ToList();
        #endregion

        #region Lifecycle
        protected override void OnUpdate(double delta)
        {
            try
            {
                for (int i = _roomUpdateList.Count - 1; i >= 0; i--)
                {
                    var room = _roomUpdateList[i];
                    try { room.Update(delta); }
                    catch (Exception ex)
                    {
                        _log.Error(ex, "error updating room ({id}).", room.Id);
                        TryRemove(room.Id, out _);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex, "error during ({id}) update.", Id);
            }
        }
        #endregion

        #region Dispose
        protected override void OnDispose()
        {
            _log.Debug("disposing {id} with {count} rooms.", Id, _rooms.Count);

            for (int i = _roomUpdateList.Count - 1; i >= 0; i--)
                TryRemove(_roomUpdateList[i].Id, out _);

            _rooms.Clear();
            _roomUpdateList.Clear();
        }
        #endregion
    }
}