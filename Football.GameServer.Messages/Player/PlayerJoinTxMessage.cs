using Football.Network.Messaging;
using Football.Serialization;

namespace Football.GameServer.Messages.Player
{
    [Message(9203)]
    public class PlayerJoinTxMessage : Message
    {
        public readonly PlayerMatchInformationMessage PlayerMatchInfo;

        public PlayerJoinTxMessage(BinaryReader reader) : base(reader)
        {
            PlayerMatchInfo = (PlayerMatchInformationMessage)reader.method_25();
        }

        public PlayerJoinTxMessage(PlayerMatchInformationMessage playerMatchInfo)
        {
            PlayerMatchInfo = playerMatchInfo;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_25(PlayerMatchInfo);
        }

        public override string ToString()
        {
            return PlayerMatchInfo.ToString();
        }
    }
}
