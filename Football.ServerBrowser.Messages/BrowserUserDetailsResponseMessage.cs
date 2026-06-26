using Football.Network.Messaging;
using Football.Serialization;

namespace Football.ServerBrowser.Messages
{
    [MessageAttribute(7942)]
    public class BrowserUserDetailsResponseMessage : Message
    {
        public readonly string PlayerName;
        public readonly string UserName;

        public BrowserUserDetailsResponseMessage(BinaryReader reader)
        {
            PlayerName = reader.method_14();
            UserName = reader.method_14();
        }

        public BrowserUserDetailsResponseMessage(string playerName, string userName)
        {
            PlayerName = playerName;
            UserName = userName;
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.method_14(PlayerName);
            writer.method_14(UserName);
        }

        public override string ToString()
        {
            return $"PlayerName: {PlayerName}, UserName: {UserName}";
        }
    }
}
