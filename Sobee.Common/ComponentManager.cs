
using System.Collections.Concurrent;

namespace Sobee.Common
{
    public class ComponentManager<T> : Component where T : Component
    {
        private readonly Serilog.ILogger _log = Logging.Get<ComponentManager<T>>();
        private bool _isDisposed;

        private readonly ConcurrentDictionary<Guid, T> _components = new();

        public ComponentManager() : base()
        {

        }

        public T this[Guid id] => _components[id];

        public override Task Update(double delta)
        {
            foreach (var component in _components.Values)
            {
                component.Update(delta);
            }

            return Task.CompletedTask;
        }

        public A? GetComponent<A>() where A : T
        {
            foreach (var component in _components.Values)
            {
                if (component is A aComponent)
                {
                    return aComponent;
                }
            }
            return default;
        }

        public bool AddComponent(T component)
        {
            if (_components.Values.OfType<T>().Any())
            {
                _log.Warning("{managerId} already has a component of type {componentType}.", Id, typeof(T).Name);
                return false;
            }

            if (_components.TryAdd(component.Id, component))
            {
                _log.Debug("{managerId} added component {componentId}.", Id, component.Id);
                return true;
            }

            return false;
        }

        public bool RemoveComponent(T component)
        {
            return RemoveComponent(component.Id);
        }

        public bool RemoveComponent(Guid componentId)
        {
            if (_components.TryRemove(componentId, out var component))
            {
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
                try
                {
                    foreach (var component in _components.Values)
                    {
                        component.Dispose();
                    }

                    _components.Clear();
                    _log.Debug("{id} disposed.", Id);
                }
                catch (Exception ex)
                {
                    _log.Error(ex, "{id} dispose error.", Id);
                }
            }

            base.Dispose(disposing);
        }
    }
}