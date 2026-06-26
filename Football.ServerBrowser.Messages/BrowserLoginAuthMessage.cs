using Football.Network.Messaging;
using Football.Serialization;

namespace Football.ServerBrowser.Messages
{
    [MessageAttribute(15346)]
    public class BrowserLoginAuthMessage : Message
    {
        public readonly string Username;
        public readonly string Password;

        public BrowserLoginAuthMessage(BinaryReader reader)
        {
            Username = reader.method_14();
            Password = reader.method_14();
        }

        public BrowserLoginAuthMessage(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.method_14(Username);
            writer.method_14(Password);
        }

        public override string ToString()
        {
            return $"Username: {Username}, Password: {Password}";
        }
    }
}
