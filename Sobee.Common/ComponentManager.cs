
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
            if (_components.TryAdd(component.Id, component))
            {
                _log.Debug("{managerId} added component {componentId}.", Id, component.Id);
                return true;
            }

            return false;
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

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                if (disposing)
                {
                    foreach (var component in _components.Values)
                    {
                        component.Dispose();
                    }

                    _components.Clear();
                    _log.Debug("{id} disposed.", Id);
                }
            }

            base.Dispose(disposing);
        }
    }
}
