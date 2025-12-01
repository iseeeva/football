using System.Numerics;
using Sobee.Network.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Player
{
    [MessageAttribute(18290)]
    public class PlayerMoveMessage : Message
    {
        public readonly sbyte SquadNumber;

        public readonly Vector2 Position;

        public readonly Vector2 Velocity;

        public readonly bool IsSprint;

        public readonly bool IsAlerted;

        public readonly byte Stamina;

        public PlayerMoveMessage(BinaryReader reader) : base(reader)
        {
            SquadNumber = reader.method_11();
            Position = new Vector2(reader.method_8() / 6f, reader.method_8() / 6f);
            float num = reader.method_8() / 10f;
            float float_ = reader.method_8() / 10000f;
            Velocity = new Vector2(num * GClass97.smethod_11(float_), num * GClass97.smethod_10(float_));
            IsSprint = reader.method_1();
            IsAlerted = reader.method_1();
            Stamina = reader.method_2();
        }

        public PlayerMoveMessage(sbyte squadNumber, Vector2 position, Vector2 velocity, bool isSprint, bool isAlerted, byte stamina)
        {
            SquadNumber = squadNumber;
            Position = position;
            Velocity = velocity;
            IsSprint = isSprint;
            IsAlerted = isAlerted;
            Stamina = stamina;
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
            writer.method_1(IsSprint);
            writer.method_1(IsAlerted);
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
            IsSprint,
            " Alerted:",
            IsAlerted,
            " Stamina: ",
            Stamina
            });
        }
    }
}
