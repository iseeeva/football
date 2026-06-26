using Football.GameServer.Messages.Match;
using Football.Network.Messaging;
using Football.Serialization;

namespace Football.ServerBrowser.Messages
{
    [MessageAttribute(6791)]
    public class BrowserServerListItem : Message
    {
        public readonly Guid MatchId;
        public readonly string MatchName;

        public readonly MatchInformationMessage MatchInfo;

        public BrowserServerListItem(BinaryReader reader) : base(reader)
        {
            MatchId = reader.method_18();
            MatchName = reader.method_14();
            MatchInfo = new MatchInformationMessage(reader);
        }

        public BrowserServerListItem(Guid matchGuid, string matchName, MatchInformationMessage matchInfo)
        {
            MatchId = matchGuid;
            MatchName = matchName;
            MatchInfo = matchInfo;
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.method_18(MatchId);
            writer.method_14(MatchName);
            MatchInfo.Serialize(writer);
        }
    }

    [MessageAttribute(38214)]
    public class BrowserServerListResponseMessage : Message
    {
        // TODO: Maclari bu kadar acik sekilde vermek tehlikeli olabilir.
        public readonly List<BrowserServerListItem> Matches;

        public BrowserServerListResponseMessage(BinaryReader reader)
        {
            ushort num = reader.method_15();
            Matches = new List<BrowserServerListItem>(num);
            for (int i = 0; i < num; i++)
            {
                Matches.Add((BrowserServerListItem)reader.method_25());
            }
        }

        public BrowserServerListResponseMessage(List<BrowserServerListItem> matches)
        {
            Matches = matches;
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.method_15((ushort)Matches.Count);
            for (int i = 0; i < Matches.Count; i++)
            {
                writer.method_25(Matches[i]);
            }
        }

        public override string ToString()
        {
            return $"MatchCount: {Matches.Count}";
        }
    }
}
