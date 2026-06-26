namespace Football.Common
{
    public interface IComponentOwner : IComponent
    {

    }

    public class ComponentManager<TComponent> : Component<IComponentOwner>, IComponentOwner
        where TComponent : IComponent
    {
        private readonly Dictionary<Guid, TComponent> _components = new();

        public TComponent? this[Guid id]
            => _components.GetValueOrDefault(id);

        public int Count => _components.Count;

        public event Action<TComponent>? ComponentAdded;
        public event Action<TComponent>? ComponentRemoved;

        #region Lifecycle
        protected override void OnStart()
        {
            foreach (var c in _components.Values.ToArray())
                c.Start();
        }

        protected override void OnStop()
        {
            foreach (var c in _components.Values.ToArray())
                c.Stop();
        }

        protected override void OnUpdate(double delta)
        {
            foreach (var c in _components.Values.ToArray())
                c.Update(delta);
        }
        #endregion

        #region Components
        public bool AddComponent(TComponent component)
        {
            ArgumentNullException.ThrowIfNull(component);

            if (IsDisposed)
            {
                _log.Warning("class disposed — cannot add {Type} component.", component.GetType().Name);
                return false;
            }

            if (component.IsDisposed)
            {
                _log.Warning("cannot add disposed {Type} component.", component.GetType().Name);
                return false;
            }

            if (HasComponent(component.GetType()))
            {
                _log.Warning("class already owns a {Type} component.", component.GetType().Name);
                return false;
            }

            if (component is Component c) // TODO: Tehlikeli kod. Eger IComponent, Component uretmediyse ne olacak??
                c.Owner = this;

            _components.Add(component.Id, component);

            if (IsRunning)
                component.Start();

            _log.Information("added {Type} ({ComponentId}) component.", component.GetType().Name, component.Id);
            ComponentAdded?.Invoke(component);
            return true;
        }

        public bool RemoveComponent(Guid componentId, bool withoutDispose = false)
        {
            if (IsDisposed) return false;
            if (!_components.Remove(componentId, out var component)) return false;

            if (component is Component c) // TODO: Tehlikeli kod. Eger IComponent, Component uretmediyse ne olacak??
                c.Owner = null;

            if (!withoutDispose)
                component.Dispose();

            _log.Information("removed {Type} ({ComponentId}) component.", component.GetType().Name, component.Id);
            ComponentRemoved?.Invoke(component);
            return true;
        }

        public bool RemoveComponent<TDerived>(bool withoutDispose = false) where TDerived : TComponent
        {
            var component = GetComponent<TDerived>();
            return component is not null && RemoveComponent(component.Id, withoutDispose);
        }

        public TDerived? GetComponent<TDerived>() where TDerived : TComponent
        {
            foreach (var c in _components.Values)
                if (c is TDerived match) return match;
            return default;
        }

        public IEnumerable<TComponent> GetAllComponents()
            => _components.Values.ToArray();

        public IEnumerable<TDerived> GetComponentOfType<TDerived>() where TDerived : TComponent
            => _components.Values.OfType<TDerived>();

        public bool HasComponent<TDerived>() where TDerived : TComponent
            => HasComponent(typeof(TDerived));

        public bool HasComponent(Type type, bool allowDerived = false)
            => _components.Values.Any(c => allowDerived
                ? type.IsAssignableFrom(c.GetType())
                : c.GetType() == type);
        #endregion

        #region Dispose
        protected override void OnDispose()
        {
            foreach (var c in _components.Values.ToArray())
                c.Dispose();

            _components.Clear();
        }
        #endregion
    }
}