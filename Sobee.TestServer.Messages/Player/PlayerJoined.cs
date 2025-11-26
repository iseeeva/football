using Sobee.Network.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Player
{
    [MessageAttribute(9203)]
    public class PlayerJoined : Message
    {
        public PlayerMatchInformation PlayerInfo { get; }

        public PlayerJoined(BinaryReader reader) : base(reader)
        {
            PlayerInfo = (PlayerMatchInformation)reader.method_25();
        }

        public PlayerJoined(PlayerMatchInformation playerMatchInformation)
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
