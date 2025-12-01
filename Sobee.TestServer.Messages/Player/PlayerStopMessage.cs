using System.Numerics;
using Sobee.Network.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Player
{
    [MessageAttribute(13107)]
    public class PlayerStopMessage : Message
    {
        public PlayerStopMessage(BinaryReader gclass315_0) : base(gclass315_0)
        {
            this.SquadNumber = gclass315_0.method_11();
            this.Position = new Vector2((float)gclass315_0.method_8() / 6f, (float)gclass315_0.method_8() / 6f);
            this.Direction = (float)gclass315_0.method_8() / 10000f;
            this.IsAlerted = gclass315_0.method_1();
            this.Stamina = gclass315_0.method_2();
        }

        public PlayerStopMessage(sbyte squadNumber, Vector2 position, Vector2 direction, bool isAlerted, byte stamina)
        {
            this.SquadNumber = squadNumber;
            this.Position = position;
            this.Direction = GClass97.smethod_15(direction.Y, direction.X);
            this.IsAlerted = isAlerted;
            this.Stamina = stamina;
        }

        public override void Serialize(BinaryWriter gclass316_0)
        {
            base.Serialize(gclass316_0);
            gclass316_0.method_11(this.SquadNumber);
            gclass316_0.method_8((short)(this.Position.X * 6f));
            gclass316_0.method_8((short)(this.Position.Y * 6f));
            gclass316_0.method_8((short)(this.Direction * 10000f));
            gclass316_0.method_1(this.IsAlerted);
            gclass316_0.method_2(this.Stamina);
        }

        public override string ToString()
        {
            return string.Concat(new object[]
            {
            "PlayerStop - ",
            this.SquadNumber,
            " Pos: ",
            this.Position,
            " Dir: ",
            this.Direction,
            " Alerted: ",
            this.IsAlerted,
            " Stamina: ",
            this.Stamina
            });
        }

        public readonly sbyte SquadNumber;

        public readonly Vector2 Position;

        public readonly float Direction;

        public readonly bool IsAlerted;

        public readonly byte Stamina;
    }

}
