namespace Sobee.Common
{
    public class Component : IDisposable
    {
        public Component()
        {
            // Constructor logic here
        }

        public virtual Task Update()
        {
            // Start receiving logic here
            throw new Exception("Update not defined");
        }

        public virtual void Dispose()
        {
            // Dispose logic here
            throw new Exception("Dispose not defined");
        }
    }
}
