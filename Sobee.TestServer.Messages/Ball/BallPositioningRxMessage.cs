using System.Numerics;
using Sobee.Network.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Ball
{
    [MessageAttribute(22132)]
    public class BallPositioningRxMessage : Message
    {
        public readonly float HitStrength;
        public readonly Vector2 HitDirection;
        public readonly HitSubType HitSubType;

        public BallPositioningRxMessage(BinaryReader reader) : base(reader)
        {
            HitStrength = reader.method_12();
            HitDirection = reader.method_19();
            HitSubType = (HitSubType)reader.method_2();
        }

        public BallPositioningRxMessage(float hitStrength, Vector2 hitDirection, HitSubType hitSubType)
        {
            HitStrength = hitStrength;
            HitDirection = hitDirection;
            HitSubType = hitSubType;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_12(HitStrength);
            writer.method_19(HitDirection);
            writer.method_2((byte)HitSubType);
        }
    }
}
