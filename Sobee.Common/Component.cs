namespace Sobee.Common
{
    public class Component
    {
        public Component()
        {
            // Constructor logic here
        }

        public virtual async Task Update()
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
