using System.Numerics;
using Sobee.Network.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Player
{
    [MessageAttribute(18290)]
    public class PlayerMoveTxMessage : Message
    {
        public readonly sbyte SquadNumber;
        public readonly Vector2 PlayerPosition;
        public readonly Vector2 PlayerVelocity;
        public readonly bool IsSprint;
        public readonly bool IsAlerted;
        public readonly byte Stamina;

        public PlayerMoveTxMessage(BinaryReader reader) : base(reader)
        {
            SquadNumber = reader.method_11();
            PlayerPosition = new Vector2(reader.method_8() / 6f, reader.method_8() / 6f);
            float num = reader.method_8() / 10f;
            float float_ = reader.method_8() / 10000f;
            PlayerVelocity = new Vector2(num * GClass97.smethod_11(float_), num * GClass97.smethod_10(float_));
            IsSprint = reader.method_1();
            IsAlerted = reader.method_1();
            Stamina = reader.method_2();
        }

        public PlayerMoveTxMessage(sbyte squadNumber, Vector2 playerPosition, Vector2 playerVelocity, bool isSprint, bool isAlerted, byte stamina)
        {
            SquadNumber = squadNumber;
            PlayerPosition = playerPosition;
            PlayerVelocity = playerVelocity;
            IsSprint = isSprint;
            IsAlerted = isAlerted;
            Stamina = stamina;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_11(SquadNumber);
            writer.method_8((short)(PlayerPosition.X * 6f));
            writer.method_8((short)(PlayerPosition.Y * 6f));
            writer.method_8((short)(PlayerVelocity.Length() * 10f));
            Vector2 vector = Vector2.Normalize(PlayerVelocity);
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
            PlayerPosition.ToString(),
            " Vel:",
            PlayerVelocity.ToString(),
            " Spd:",
            PlayerVelocity.Length(),
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
