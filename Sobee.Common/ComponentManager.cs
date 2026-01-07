namespace Sobee.Common
{
    public class ComponentManager<T> : Component where T : Component
    {
        private readonly Serilog.ILogger _log = Logging.Get<ComponentManager<T>>();
        private bool _isDisposed;

        private readonly Dictionary<Guid, T> _components = new();
        private readonly List<T> _updateList = new();

        public T this[Guid id] => _components[id];

        public override void Update(double delta)
        {
            for (int i = 0; i < _updateList.Count; i++)
            {
                _updateList[i].Update(delta);
            }
        }

        public A? GetComponent<A>() where A : T
        {
            foreach (var component in _updateList)
            {
                if (component is A a)
                    return a;
            }
            return null;
        }

        public bool AddComponent(T component)
        {
            if (_components.Values.Any(x => x.GetType() == component.GetType()))
            {
                _log.Warning("{managerId} already has component type {type}.", Id, component.GetType().Name);
                return false;
            }

            _components.Add(component.Id, component);
            _updateList.Add(component);

            _log.Debug("{managerId} added component {componentId}.", Id, component.Id);
            return true;
        }

        public bool RemoveComponent(Guid componentId)
        {
            if (_components.Remove(componentId, out var component))
            {
                _updateList.Remove(component);
                component.Dispose();

                _log.Debug("{managerId} removed component {componentId}.", Id, componentId);
                return true;
            }

            return false;
        }

        public int Count => _components.Count;

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            if (disposing)
            {
                foreach (var component in _updateList)
                    component.Dispose();

                _components.Clear();
                _updateList.Clear();

                _log.Debug("{id} disposed.", Id);
            }

            base.Dispose(disposing);
        }
    }
}
