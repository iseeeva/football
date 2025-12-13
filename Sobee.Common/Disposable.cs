namespace Sobee.Common
{
    public class Disposable : IDisposable
    {
        // To detect redundant calls
        private bool _isDisposed;

        ~Disposable() // the finalizer
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            if (disposing)
            {
                // Dispose managed state.

            }
        }
    }
}
