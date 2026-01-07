namespace Sobee.Common
{
    public class Component : Disposable
    {
        public readonly Guid Id = Guid.NewGuid();
        public readonly DateTime CreatedAt = DateTime.UtcNow;

        public Component()
        {
            // Constructor logic here
        }

        public virtual void Update(double delta)
        {

        }
    }
}
