using Sobee.Messaging;

namespace Sobee.TestServer.Messages
{
    public abstract class AnimationMessageAbstract : Message
    {
        public AnimationMessageAbstract(BinaryReader gclass315_0) : base(gclass315_0)
        {
            this.AnimationType = (AnimationType)gclass315_0.method_15();
        }

        public AnimationMessageAbstract(AnimationType animationType_1)
        {
            this.AnimationType = animationType_1;
        }

        public override void Serialize(BinaryWriter gclass316_0)
        {
            base.Serialize(gclass316_0);
            gclass316_0.method_15((ushort)this.AnimationType);
        }

        public AnimationType AnimationType;
    }
}
