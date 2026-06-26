using Football.Network.Messaging;
using Football.Serialization;
using System.Numerics;

namespace Football.GameServer.Messages.Player
{
    [Message(48125)]
    public class PlayerMoveKeyDownRxMessage : Message
    {
        public readonly Vector2 Direction;
        public readonly bool IsSprint;

        public PlayerMoveKeyDownRxMessage(BinaryReader reader) : base(reader)
        {
            Direction = GClass97.smethod_10_11_c(GClass97.smethod_21(reader.method_12()));
            IsSprint = reader.method_1();
        }

        public PlayerMoveKeyDownRxMessage(Vector2 direction, bool isSprint)
        {
            Direction = direction;
            IsSprint = isSprint;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_12(GClass97.smethod_20(GClass97.smethod_15(Direction.Y, Direction.X)));
            writer.method_1(IsSprint);
        }
    }
}
