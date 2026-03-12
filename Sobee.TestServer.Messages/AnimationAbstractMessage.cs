using Sobee.Network.Messaging;

namespace Sobee.TestServer.Messages
{
    public abstract class AnimationAbstractMessage : Message
    {
        public AnimationType AnimationType;

        public AnimationAbstractMessage(BinaryReader reader) : base(reader)
        {
            this.AnimationType = (AnimationType)reader.method_15();
        }

        public AnimationAbstractMessage(AnimationType animationType)
        {
            this.AnimationType = animationType;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_15((ushort)this.AnimationType);
        }
    }
}
