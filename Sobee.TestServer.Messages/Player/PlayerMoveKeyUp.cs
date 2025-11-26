using Sobee.Network.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Player
{
    [MessageAttribute(13399)]
    public class PlayerMoveKeyUp : Message
    {
        // Token: 0x06000335 RID: 821 RVA: 0x00002496 File Offset: 0x00000696
        public PlayerMoveKeyUp()
        {
        }

        // Token: 0x06000336 RID: 822 RVA: 0x0000249E File Offset: 0x0000069E
        public PlayerMoveKeyUp(BinaryReader gclass315_0) : base(gclass315_0)
        {
        }

        // Token: 0x06000337 RID: 823 RVA: 0x000024A7 File Offset: 0x000006A7
        public override void Serialize(BinaryWriter gclass316_0)
        {
            base.Serialize(gclass316_0);
        }
    }

}
