using Football.Tasks;

namespace Football.GameServer.Game
{
    public sealed class GameMainServer : TickScheduler
    {
        public readonly GameHub Hub;

        public GameMainServer(int port, double ticksPerSecond = 100)
        {
            Hub = new GameHub(port);
            AddComponent(Hub);

            SetTickRate(ticksPerSecond);
        }

        #region Lifecycle
        //protected override void OnStart()
        //{
        //    base.OnStart();
        //}

        //protected override void OnStop()
        //{
        //    base.OnStop();
        //}

        //protected override void OnUpdate(double delta)
        //{
        //    base.OnUpdate(delta);
        //}
        #endregion

        #region Dispose
        //protected override void OnDispose()
        //{
        //    base.OnDispose();
        //}
        #endregion
    }
}