using System.Numerics;
using Sobee.Messaging;

namespace Sobee.TestServer.Messages.Player
{
    [GAttribute0(18290)]
    public class PlayerMove : Message
    {
        public sbyte SquadNumber { get; }

        public Vector2 Position { get; }

        public Vector2 Velocity { get; }

        public bool Sprint { get; }

        public bool Alerted { get; }

        public byte Stamina { get; }

        public PlayerMove(BinaryReader reader) : base(reader)
        {
            this.SquadNumber = reader.method_11();
            this.Position = new Vector2((float)reader.method_8() / 6f, (float)reader.method_8() / 6f);
            float num = (float)reader.method_8() / 10f;
            float float_ = (float)reader.method_8() / 10000f;
            this.Velocity = new Vector2(num * GClass97.smethod_11(float_), num * GClass97.smethod_10(float_));
            this.Sprint = reader.method_1();
            this.Alerted = reader.method_1();
            this.Stamina = reader.method_2();
        }

        public PlayerMove(sbyte sbyte_1, Vector2 vector2_2, Vector2 vector2_3, bool bool_2, bool bool_3, byte byte_1)
        {
            this.SquadNumber = sbyte_1;
            this.Position = vector2_2;
            this.Velocity = vector2_3;
            this.Sprint = bool_2;
            this.Alerted = bool_3;
            this.Stamina = byte_1;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_11(this.SquadNumber);
            writer.method_8((short)(this.Position.X * 6f));
            writer.method_8((short)(this.Position.Y * 6f));
            writer.method_8((short)(this.Velocity.Length() * 10f));
            Vector2 vector = Vector2.Normalize(this.Velocity);
            writer.method_8((short)(GClass97.smethod_15(vector.Y, vector.X) * 10000f));
            writer.method_1(this.Sprint);
            writer.method_1(this.Alerted);
            writer.method_2(this.Stamina);
        }

        public override string ToString()
        {
            return string.Concat(new object[]
            {
            "PlayerMove - #",
            this.SquadNumber,
            " Pos:",
            this.Position.ToString(),
            " Vel:",
            this.Velocity.ToString(),
            " Spd:",
            this.Velocity.Length(),
            " Sprnt:",
            this.Sprint,
            " Alerted:",
            this.Alerted,
            " Stamina: ",
            this.Stamina
            });
        }
    }
}
