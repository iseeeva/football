using Football.Common;
using Football.Network;

namespace Football.GameServer.Game
{
    public class GameRoom<TRoom, TSession> : ComponentManager<IComponent>
        where TRoom : GameRoom<TRoom, TSession>
        where TSession : Session
    {
        protected readonly SessionManager<TRoom, TSession> _sessions;
        protected readonly GameRoomCommunication _communication;

        public GameRoom()
        {
            _sessions = CreateSessionManager();
            AddComponent(_sessions);
            _communication = new GameRoomCommunication();
            AddComponent(_communication);
        }

        protected virtual SessionManager<TRoom, TSession> CreateSessionManager()
            => new();
    }
}