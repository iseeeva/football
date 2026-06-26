using Football.Network.Messaging;
using Football.Serialization;

namespace Football.ServerBrowser.Messages
{
    [MessageAttribute(17281)]
    public class BrowserRegisterAuthMessage : Message
    {
        public readonly string PlayerName;
        public readonly string UserName;
        public readonly string Password;

        public BrowserRegisterAuthMessage(BinaryReader reader)
        {
            PlayerName = reader.method_14();
            UserName = reader.method_14();
            Password = reader.method_14();
        }

        public BrowserRegisterAuthMessage(string playername, string username, string password)
        {
            PlayerName = playername;
            UserName = username;
            Password = password;
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.method_14(PlayerName);
            writer.method_14(UserName);
            writer.method_14(Password);
        }

        public override string ToString()
        {
            return $"Playername:{PlayerName}, Username: {UserName}, Password: {Password}";
        }
    }
}
