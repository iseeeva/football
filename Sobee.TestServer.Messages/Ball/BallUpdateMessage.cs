using System.Numerics;
using Sobee.Network.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Ball
{
    [MessageAttribute(13196)]
    public class BallUpdateMessage : Message
    {
        public BallUpdateMessage(BinaryReader gclass315_0) : base(gclass315_0)
        {
            this.Position = gclass315_0.method_20();
            this.Velocity = gclass315_0.method_20();
        }

        public BallUpdateMessage(Vector3 position, Vector3 velocity)
        {
            this.Position = position;
            this.Velocity = velocity;
        }

        public override void Serialize(BinaryWriter gclass316_0)
        {
            base.Serialize(gclass316_0);
            gclass316_0.method_20(this.Position);
            gclass316_0.method_20(this.Velocity);
        }

        private Vector3 Position;

        private Vector3 Velocity;
    }

}
