using System.Numerics;
using Sobee.Network.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Player
{
    [MessageAttribute(13107)]
    public class PlayerStopTxMessage : Message
    {
        public readonly sbyte SquadNumber;
        public readonly Vector2 PlayerPosition;
        public readonly float PlayerDirection;
        public readonly bool IsAlerted;
        public readonly byte Stamina;

        public PlayerStopTxMessage(BinaryReader reader) : base(reader)
        {
            this.SquadNumber = reader.method_11();
            this.PlayerPosition = new Vector2((float)reader.method_8() / 6f, (float)reader.method_8() / 6f);
            this.PlayerDirection = (float)reader.method_8() / 10000f;
            this.IsAlerted = reader.method_1();
            this.Stamina = reader.method_2();
        }

        public PlayerStopTxMessage(sbyte squadNumber, Vector2 playerPosition, Vector2 playerDirection, bool isAlerted, byte stamina)
        {
            this.SquadNumber = squadNumber;
            this.PlayerPosition = playerPosition;
            this.PlayerDirection = GClass97.smethod_15(playerDirection.Y, playerDirection.X);
            this.IsAlerted = isAlerted;
            this.Stamina = stamina;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_11(this.SquadNumber);
            writer.method_8((short)(this.PlayerPosition.X * 6f));
            writer.method_8((short)(this.PlayerPosition.Y * 6f));
            writer.method_8((short)(this.PlayerDirection * 10000f));
            writer.method_1(this.IsAlerted);
            writer.method_2(this.Stamina);
        }

        public override string ToString()
        {
            return string.Concat([
                "PlayerStop - ",
                this.SquadNumber,
                " Pos: ",
                this.PlayerPosition,
                " Dir: ",
                this.PlayerDirection,
                " Alerted: ",
                this.IsAlerted,
                " Stamina: ",
                this.Stamina
            ]);
        }
    }
}
