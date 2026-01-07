using System.Diagnostics;
using Sobee.Common;

namespace Sobee.Tasks
{
    public enum TickState
    {
        Stopped,
        Running,
        Paused
    }

    // INFO: Bu class, türetilmişleri için tick looplar sağlar.
    // Eğer bir class sürekli güncellenmesi gereken bir iş yapıyorsa,
    // TickController'dan türetilebilir ve TickStart() ile başlatılabilir.
    // WARNING: TickController'dan türetilenlerin karmaşıklığı önlemek için sealed olması önerilir.
    public class TickController : Component
    {
        private static readonly Serilog.ILogger _log = Logging.Get<TickController>();
        private bool _isDisposed;

        private CancellationTokenSource? _loopCancelToken;
        private Task? _loopTask;
        private readonly Stopwatch _loopTimer = new();
        private double _targetTickSeconds = 1.0 / 100.0;

        private volatile TickState _tickState = TickState.Stopped;
        public bool IsRunning => _tickState == TickState.Running;

        public TickController()
        {
            _log.Information("{id} initialized.", Id);
        }

        public void TickStart()
        {
            switch (_tickState)
            {
                case TickState.Running:
                    return;

                case TickState.Paused:
                    _tickState = TickState.Running;
                    _log.Information("{id} resumed.", Id);
                    return;

                case TickState.Stopped:
                    _loopCancelToken = new CancellationTokenSource();
                    _tickState = TickState.Running;
                    _loopTask = Task.Factory.StartNew(
                        () => TickLoop(_loopCancelToken.Token),
                        _loopCancelToken.Token,
                        TaskCreationOptions.LongRunning,
                        TaskScheduler.Default
                    );
                    _log.Information("{id} started.", Id);
                    break;
            }
        }

        public void TickPause()
        {
            if (_tickState == TickState.Running)
            {
                _tickState = TickState.Paused;
                _log.Information("{id} paused.", Id);
            }
        }

        private void TickLoop(CancellationToken token)
        {
            _loopTimer.Restart();
            double previous = _loopTimer.Elapsed.TotalSeconds;
            var spinWait = new SpinWait();

            try
            {
                while (!token.IsCancellationRequested)
                {
                    if (_tickState != TickState.Running)
                    {
                        previous = _loopTimer.Elapsed.TotalSeconds;
                        Thread.Sleep(10);
                        continue;
                    }

                    double now = _loopTimer.Elapsed.TotalSeconds;
                    double delta = now - previous;
                    previous = now;

                    try
                    {
                        Update(delta);
                    }
                    catch (Exception ex)
                    {
                        _log.Error(ex, "{id} error during update cycle.", Id);
                    }

                    double elapsed = _loopTimer.Elapsed.TotalSeconds - now;
                    double sleepSeconds = _targetTickSeconds - elapsed;

                    if (sleepSeconds > 0)
                    {
                        int sleepMs = (int)(sleepSeconds * 1000);
                        if (sleepMs > 2) Thread.Sleep(sleepMs - 2);

                        while (_loopTimer.Elapsed.TotalSeconds < (now + _targetTickSeconds))
                        {
                            spinWait.SpinOnce();
                        }
                    }
                }
            }
            catch (OperationCanceledException) { }
            finally
            {
                _loopTimer.Stop();
                _tickState = TickState.Stopped;
                _log.Information("{id} loop thread terminated.", Id);
            }
        }

        public override void Update(double delta)
        {
            // TickController controls this update func.
            // Check TickLoop for cycle.
        }

        public void TickStop()
        {
            if (_loopCancelToken == null) return;

            _loopCancelToken.Cancel();
            try { _loopTask?.Wait(1000); } catch { }

            _loopCancelToken.Dispose();
            _loopCancelToken = null;
            _tickState = TickState.Stopped;
            _log.Information("{id} stop requested.", Id);
        }

        public void SetTickRate(double ticksPerSecond)
        {
            if (ticksPerSecond <= 0) throw new ArgumentOutOfRangeException(nameof(ticksPerSecond));
            _targetTickSeconds = 1.0 / ticksPerSecond;
            _log.Information("{id} rate updated to {rate} TPS.", Id, ticksPerSecond);
        }

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            if (disposing)
                TickStop();

            base.Dispose(disposing);
        }
    }
}