using System.Numerics;
using Sobee.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Player
{
    [MessageAttribute(18290)]
    public class PlayerMove : Message
    {
        public readonly sbyte SquadNumber;

        public readonly Vector2 Position;

        public readonly Vector2 Velocity;

        public readonly bool Sprint;

        public readonly bool Alerted;

        public readonly byte Stamina;

        public PlayerMove(BinaryReader reader) : base(reader)
        {
            SquadNumber = reader.method_11();
            Position = new Vector2(reader.method_8() / 6f, reader.method_8() / 6f);
            float num = reader.method_8() / 10f;
            float float_ = reader.method_8() / 10000f;
            Velocity = new Vector2(num * GClass97.smethod_11(float_), num * GClass97.smethod_10(float_));
            Sprint = reader.method_1();
            Alerted = reader.method_1();
            Stamina = reader.method_2();
        }

        public PlayerMove(sbyte sbyte_1, Vector2 vector2_2, Vector2 vector2_3, bool bool_2, bool bool_3, byte byte_1)
        {
            SquadNumber = sbyte_1;
            Position = vector2_2;
            Velocity = vector2_3;
            Sprint = bool_2;
            Alerted = bool_3;
            Stamina = byte_1;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_11(SquadNumber);
            writer.method_8((short)(Position.X * 6f));
            writer.method_8((short)(Position.Y * 6f));
            writer.method_8((short)(Velocity.Length() * 10f));
            Vector2 vector = Vector2.Normalize(Velocity);
            writer.method_8((short)(GClass97.smethod_15(vector.Y, vector.X) * 10000f));
            writer.method_1(Sprint);
            writer.method_1(Alerted);
            writer.method_2(Stamina);
        }

        public override string ToString()
        {
            return string.Concat(new object[]
            {
            "PlayerMove - #",
            SquadNumber,
            " Pos:",
            Position.ToString(),
            " Vel:",
            Velocity.ToString(),
            " Spd:",
            Velocity.Length(),
            " Sprnt:",
            Sprint,
            " Alerted:",
            Alerted,
            " Stamina: ",
            Stamina
            });
        }
    }
}
