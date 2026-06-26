using Football.Common;
using System.Diagnostics;

namespace Football.Tasks
{
    // INFO: Bu class, türetilmişleri için tick looplar sağlar.
    // Eğer bir class sürekli güncellenmesi gereken bir iş yapıyorsa,
    // TickScheduler'dan türetilebilir ve Start() ile başlatılabilir.
    // WARNING: TickScheduler'dan türetilenlerin karmaşıklığı önlemek için sealed olması önerilir.
    public class TickScheduler : ComponentManager<Component>
    {
        private readonly object _tickLock = new();
        private CancellationTokenSource? _loopCancelToken;
        private Task? _loopTask;
        private readonly Stopwatch _loopTimer = new();
        private double _targetTickSeconds = 1.0 / 100.0;

        #region Lifecycle
        protected override void OnStart()
        {
            base.OnStart();

            lock (_tickLock)
            {
                if (_loopCancelToken != null) return;

                _loopCancelToken = new CancellationTokenSource();
                _loopTask = Task.Factory.StartNew(
                    () => TickLoop(_loopCancelToken.Token),
                    _loopCancelToken.Token,
                    TaskCreationOptions.LongRunning,
                    TaskScheduler.Default
                );
            }
        }

        protected override void OnStop()
        {
            CancellationTokenSource? cts;
            Task? task;

            lock (_tickLock)
            {
                if (_loopCancelToken == null) return;
                cts = _loopCancelToken;
                task = _loopTask;
                _loopCancelToken = null;
                _loopTask = null;
            }

            cts.Cancel();
            try { task?.Wait(1000); } catch { }
            cts.Dispose();

            base.OnStop();
        }

        protected override void OnUpdate(double delta)
        {
            // TickScheduler controls this update func.
            // Check TickLoop for cycle.

            base.OnUpdate(delta);
        }
        #endregion

        #region Tick Loop
        private void TickLoop(CancellationToken token)
        {
            _loopTimer.Restart();
            double previous = _loopTimer.Elapsed.TotalSeconds;
            var spinWait = new SpinWait();

            try
            {
                while (!token.IsCancellationRequested)
                {
                    double now = _loopTimer.Elapsed.TotalSeconds;
                    double delta = now - previous;
                    previous = now;

                    try
                    {
                        Update(delta);
                    }
                    catch (Exception ex)
                    {
                        _log.Error(ex, "error during update cycle.");
                    }

                    double elapsed = _loopTimer.Elapsed.TotalSeconds - now;
                    double sleepSeconds = _targetTickSeconds - elapsed;

                    if (sleepSeconds > 0)
                    {
                        int sleepMs = (int)(sleepSeconds * 1000);
                        if (sleepMs > 2) Thread.Sleep(sleepMs - 2);

                        while (_loopTimer.Elapsed.TotalSeconds < now + _targetTickSeconds)
                            spinWait.SpinOnce();
                    }
                }
            }
            catch (OperationCanceledException) { }
            finally
            {
                _loopTimer.Stop();
                _log.Information("loop thread terminated.");
            }
        }

        public void SetTickRate(double ticksPerSecond)
        {
            if (ticksPerSecond <= 0)
                throw new ArgumentOutOfRangeException(nameof(ticksPerSecond));

            _targetTickSeconds = 1.0 / ticksPerSecond;
            _log.Information("rate updated to {rate} TPS.", ticksPerSecond);
        }
        #endregion

        #region Dispose
        //protected override void OnDispose()
        //{
        //    base.OnDispose();
        //}
        #endregion
    }
}