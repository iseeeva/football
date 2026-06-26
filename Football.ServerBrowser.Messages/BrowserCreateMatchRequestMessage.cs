using Football.GameServer.Messages;
using Football.Network.Messaging;
using Football.Serialization;

namespace Football.ServerBrowser.Messages
{
    [Message(7415)]
    public class BrowserCreateMatchRequestMessage : Message
    {
        // TODO: EVENT YAZILMADI!!!!!!!!!!!!!!!!!!!!!!?!?!?!11111111111111
        public readonly string MatchName;
        public readonly string MatchPassword;
        public readonly ScenarioType ScenarioType;

        public readonly string HomeFullName;
        public readonly string HomeShortName;
        public readonly int HomeCapacity;

        public readonly string AwayFullName;
        public readonly string AwayShortName;
        public readonly int AwayCapacity;

        public BrowserCreateMatchRequestMessage(BinaryReader reader)
        {
            MatchName = reader.method_14();
            MatchPassword = reader.method_14();
            ScenarioType = (ScenarioType)reader.method_9();

            HomeFullName = reader.method_14();
            HomeShortName = reader.method_14();
            HomeCapacity = reader.method_9();

            AwayFullName = reader.method_14();
            AwayShortName = reader.method_14();
            AwayCapacity = reader.method_9();
        }

        public BrowserCreateMatchRequestMessage(
            string matchName, string matchPassword, ScenarioType scenarioType,
            string homeFullName, string homeShortName, int homeCapacity,
            string awayFullName, string awayShortName, int awayCapacity
            )
        {
            MatchName = matchName;
            MatchPassword = matchPassword;
            ScenarioType = scenarioType;

            HomeFullName = homeFullName;
            HomeShortName = homeShortName;
            HomeCapacity = homeCapacity;

            AwayFullName = awayFullName;
            AwayShortName = awayShortName;
            AwayCapacity = awayCapacity;
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.method_14(MatchName);
            writer.method_14(MatchPassword);
            writer.method_9((int)ScenarioType);

            writer.method_14(HomeFullName);
            writer.method_14(HomeShortName);
            writer.method_9(HomeCapacity);

            writer.method_14(AwayFullName);
            writer.method_14(AwayShortName);
            writer.method_9(AwayCapacity);
        }
    }
}
