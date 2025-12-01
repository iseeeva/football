using Sobee.Network.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Player
{
    [MessageAttribute(9203)]
    public class PlayerJoinMessage : Message
    {
        public PlayerMatchInformationMessage PlayerInfo { get; }

        public PlayerJoinMessage(BinaryReader reader) : base(reader)
        {
            PlayerInfo = (PlayerMatchInformationMessage)reader.method_25();
        }

        public PlayerJoinMessage(PlayerMatchInformationMessage playerMatchInformation)
        {
            PlayerInfo = playerMatchInformation;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_25(PlayerInfo);
        }

        public override string ToString()
        {
            return PlayerInfo.ToString();
        }
    }
}
