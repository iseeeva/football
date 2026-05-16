
using Serilog;

namespace Sobee.Common
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

    public interface IComponent<TOwner> : IComponent
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
            _log.Information("initializing.");
        }

        ~Component() => Dispose(false);

        #region Start
        public void Start()
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            if (IsRunning) return;
            IsRunning = true;
            OnStart();
            _log.Information("starting.");
        }

        protected virtual void OnStart() { }
        #endregion

        #region Stop
        public void Stop()
        {
            if (!IsRunning) return;
            IsRunning = false;
            OnStop();
            _log.Information("stopping.");
        }

        protected virtual void OnStop() { }
        #endregion

        #region Update
        public void Update(double delta)
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            if (!IsRunning) return;
            OnUpdate(delta);
            //_log.Information("Updated.");
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
            IsDisposed = true;
            if (!disposing) return;
            OnDispose();
            Owner = null;
            _log.Information("dispossing.");
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