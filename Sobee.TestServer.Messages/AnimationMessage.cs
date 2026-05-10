using Sobee.Network.Messaging;

namespace Sobee.TestServer.Messages
{
    public abstract class AnimationMessage : Message
    {
        public AnimationType AnimationType;

        public AnimationMessage(BinaryReader reader) : base(reader)
        {
            this.AnimationType = (AnimationType)reader.method_15();
        }

        public AnimationMessage(AnimationType animationType)
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
