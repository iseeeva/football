using System.Numerics;
using Sobee.Network.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Player
{
    // Token: 0x02000061 RID: 97
    [MessageAttribute(48125)]
    public class PlayerMoveKeyDown : Message
    {
        public readonly Vector2 Velocity;

        public readonly bool IsSprint;

        // Token: 0x060001DF RID: 479 RVA: 0x00003BD7 File Offset: 0x00001DD7
        public PlayerMoveKeyDown(BinaryReader reader) : base(reader)
        {
            Velocity = GClass97.smethod_16(GClass97.smethod_21(reader.method_12()));
            IsSprint = reader.method_1();
        }

        // Token: 0x060001E0 RID: 480 RVA: 0x00003BF8 File Offset: 0x00001DF8
        public PlayerMoveKeyDown(Vector2 float_1, bool bool_1)
        {
            Velocity = float_1;
            IsSprint = bool_1;
        }

        // Token: 0x060001E1 RID: 481 RVA: 0x00003C0E File Offset: 0x00001E0E
        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_12(GClass97.smethod_20(GClass97.smethod_15(Velocity.Y, Velocity.X)));
            writer.method_1(IsSprint);
        }
    }
}
