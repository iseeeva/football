using Sobee.Common;
using Sobee.Network;

namespace Sobee.TestServer
{
    public class Room<TRoom, TSession> : ComponentManager<IComponent>
        where TRoom : Room<TRoom, TSession>
        where TSession : Session
    {
        protected readonly SessionManager<TRoom, TSession> _sessions;
        protected readonly RoomCommunication _communication;

        public Room()
        {
            _sessions = CreateSessionManager();
            AddComponent(_sessions);
            _communication = new RoomCommunication();
            AddComponent(_communication);
        }

        protected virtual SessionManager<TRoom, TSession> CreateSessionManager()
            => new();
    }
}