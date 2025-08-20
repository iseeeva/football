namespace Sobee.Common
{
    public class Component : IDisposable
    {
        public readonly Guid Id = Guid.NewGuid();
        public readonly DateTime CreatedAt = DateTime.Now;

        // To detect redundant calls
        private bool _isDisposed;

        public Component()
        {
            // Constructor logic here
        }

        ~Component() // the finalizer
        {
            Dispose(false);
        }

        public virtual Task Update(double delta)
        {
            // Start receiving logic here
            throw new Exception("Update not defined");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                if (disposing)
                {
                    // Dispose managed state.

                }
            }
        }
    }
}
