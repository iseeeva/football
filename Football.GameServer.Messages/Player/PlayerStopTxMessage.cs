using Football.Network.Messaging;
using Football.Serialization;
using System.Numerics;

namespace Football.GameServer.Messages.Player
{
    [Message(13107)]
    public class PlayerStopTxMessage : Message
    {
        public readonly sbyte SquadNumber;
        public readonly Vector2 PlayerPosition;
        public readonly float PlayerDirection;
        public readonly bool IsAlerted;
        public readonly byte Stamina;

        public PlayerStopTxMessage(BinaryReader reader) : base(reader)
        {
            SquadNumber = reader.method_11();
            PlayerPosition = new Vector2(reader.method_8() / 6f, reader.method_8() / 6f);
            PlayerDirection = reader.method_8() / 10000f;
            IsAlerted = reader.method_1();
            Stamina = reader.method_2();
        }

        public PlayerStopTxMessage(sbyte squadNumber, Vector2 playerPosition, Vector2 playerDirection, bool isAlerted, byte stamina)
        {
            SquadNumber = squadNumber;
            PlayerPosition = playerPosition;
            PlayerDirection = GClass97.smethod_15(playerDirection.Y, playerDirection.X);
            IsAlerted = isAlerted;
            Stamina = stamina;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_11(SquadNumber);
            writer.method_8((short)(PlayerPosition.X * 6f));
            writer.method_8((short)(PlayerPosition.Y * 6f));
            writer.method_8((short)(PlayerDirection * 10000f));
            writer.method_1(IsAlerted);
            writer.method_2(Stamina);
        }

        public override string ToString()
        {
            return string.Concat([
                "PlayerStop - ",
                SquadNumber,
                " Pos: ",
                PlayerPosition,
                " Dir: ",
                PlayerDirection,
                " Alerted: ",
                IsAlerted,
                " Stamina: ",
                Stamina
            ]);
        }
    }
}
