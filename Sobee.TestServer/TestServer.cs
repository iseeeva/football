using Sobee.Tasks;

namespace Sobee.TestServer
{
    public sealed class TestServer : TickScheduler
    {
        public readonly Hub Hub;

        public TestServer(int port, double ticksPerSecond = 100)
        {
            Hub = new Hub(port);
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