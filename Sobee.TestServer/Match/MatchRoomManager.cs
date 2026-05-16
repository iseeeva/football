using Sobee.TestServer.Match;

namespace Sobee.TestServer
{
    public class MatchRoomManager : RoomManager<MatchRoom, MatchPlayer>
    {
        #region Lifecycle
        protected override void OnUpdate(double delta)
        {
            base.OnUpdate(delta);

            var now = DateTime.UtcNow;
            for (int i = _roomUpdateList.Count - 1; i >= 0; i--)
            {
                var room = _roomUpdateList[i];
                if (room.Players.Count == 0 && (now - room.CreatedAt) > MatchRoom.MAX_IDLE_TIME)
                {
                    TryRemove(room.Id, out _);
                    continue;
                }
            }
        }
        #endregion
    }
}