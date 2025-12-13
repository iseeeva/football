namespace Sobee.Common
{
    public class Component : Disposable
    {
        public readonly Guid Id = Guid.NewGuid();
        public readonly DateTime CreatedAt = DateTime.Now;

        public Component()
        {
            // Constructor logic here
        }

        public virtual Task Update(double delta)
        {
            // Start receiving logic here
            throw new Exception("Update not defined");
        }
    }
}
