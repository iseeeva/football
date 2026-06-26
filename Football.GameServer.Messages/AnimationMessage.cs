using Football.Network.Messaging;

namespace Football.GameServer.Messages
{
    public abstract class AnimationMessage : Message
    {
        public AnimationType AnimationType;

        public AnimationMessage(BinaryReader reader) : base(reader)
        {
            AnimationType = (AnimationType)reader.method_15();
        }

        public AnimationMessage(AnimationType animationType)
        {
            AnimationType = animationType;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_15((ushort)AnimationType);
        }
    }
}
