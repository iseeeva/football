using System.Numerics;
using Sobee.Messaging;

namespace Sobee.TestServer.Messages.Player
{
    // Token: 0x02000061 RID: 97
    [GAttribute0(48125)]
    public class PlayerMovePressed : Message
    {
        public Vector2 Velocity { get; }

        public bool Sprint { get; }

        // Token: 0x060001DF RID: 479 RVA: 0x00003BD7 File Offset: 0x00001DD7
        public PlayerMovePressed(BinaryReader reader) : base(reader)
        {
            this.Velocity = GClass97.smethod_16(reader.method_12());
            this.Sprint = reader.method_1();
        }

        // Token: 0x060001E0 RID: 480 RVA: 0x00003BF8 File Offset: 0x00001DF8
        public PlayerMovePressed(Vector2 float_1, bool bool_1)
        {
            this.Velocity = float_1;
            this.Sprint = bool_1;
        }

        // Token: 0x060001E1 RID: 481 RVA: 0x00003C0E File Offset: 0x00001E0E
        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_12(GClass97.smethod_15(this.Velocity.Y, this.Velocity.X));
            writer.method_1(this.Sprint);
        }
    }
}
