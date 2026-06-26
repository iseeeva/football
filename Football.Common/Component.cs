using Serilog;

namespace Football.Common
{
    public interface IComponent : IDisposable
    {
        Guid Id { get; }
        DateTime CreatedAt { get; }
        bool IsDisposed { get; }
        bool IsRunning { get; }
        IComponentOwner? Owner { get; }
        void Start();
        void Stop();
        void Update(double delta);
    }

    public interface IComponent<out TOwner> : IComponent
        where TOwner : IComponentOwner
    {
        new TOwner? Owner { get; }
    }

    public abstract class Component : IComponent
    {
        protected readonly ILogger _log;

        public Guid Id { get; } = Guid.NewGuid();
        public DateTime CreatedAt { get; } = DateTime.UtcNow;
        public bool IsDisposed { get; private set; }
        public bool IsRunning { get; private set; }
        public IComponentOwner? Owner { get; internal set; }

        protected Component()
        {
            _log = LogFactory.GetContextForObject(this);
            _log.Debug("initialized.", Id);
        }

        #region Start
        public void Start()
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            if (IsRunning) return;

            _log.Debug("starting.", Id);
            OnStart();

            IsRunning = true;
            _log.Information("started.", Id);
        }

        protected virtual void OnStart() { }
        #endregion

        #region Stop
        public void Stop()
        {
            if (!IsRunning) return;

            _log.Debug("stopping.", Id);
            OnStop();
            IsRunning = false;
            _log.Information("stopped.", Id);
        }

        protected virtual void OnStop() { }
        #endregion

        #region Update
        public void Update(double delta)
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            if (!IsRunning) return;

            OnUpdate(delta);
        }

        protected virtual void OnUpdate(double delta) { }
        #endregion

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed) return;

            if (IsRunning) Stop();

            if (disposing)
            {
                _log.Debug("disposing.", Id);
                OnDispose();
                Owner = null;
            }

            IsDisposed = true;
        }

        protected virtual void OnDispose() { }
        #endregion
    }

    public abstract class Component<TOwner> : Component, IComponent<TOwner>
        where TOwner : IComponentOwner
    {
        public new TOwner? Owner
        {
            get => (TOwner?)base.Owner;
            internal set => base.Owner = value;
        }
    }
}