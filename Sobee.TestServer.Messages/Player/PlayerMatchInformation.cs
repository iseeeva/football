using System.IO.Compression;
using System.Numerics;
using System.Text;
using Sobee.Messaging;
using Sobee.Serialization;
using Sobee.TestServer.Messages.Match;

namespace Sobee.TestServer.Messages.Player
{
    [GAttribute0(10483)]
    public class PlayerMatchInformation : Message
    {
        public Guid MatchId { get; set; }
        public Guid PlayerId { get; set; }
        public string PlayerName { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int Stamina { get; set; }
        public StadiumSitting StadiumSitting { get; set; }
        public sbyte SquadNumber { get; set; }
        public bool Moving { get; set; }
        public PlayerAppearance Appearance { get; set; }
        public Vector2 Position { get; set; }
        public Vector3 Velocity { get; set; }
        public Vector2 Direction { get; set; }
        public bool Invite { get; set; }
        public string Unknown { get; set; } = string.Empty;
        public MatchCard CardStatus { get; set; }
        public string XmlCode { get; set; }
        public int LeagueGroupId { get; set; }
        public int LeagueId { get; set; }
        public sbyte LeagueSortOrder { get; set; }
        public float EloPoint { get; set; }
        public int TotalExperience { get; set; }
        public float ContractRevenueRate { get; set; }
        public List<string> Skills1 { get; set; } = new List<string>();
        public List<string> Skills2 { get; set; } = new List<string>();

        public PlayerMatchInformation()
        {
            // TODO: Remove hardcoded values when possible.
            MatchId = Guid.Empty;
            PlayerId = Guid.Empty;
            PlayerName = string.Empty;
            UserName = string.Empty;
            Stamina = 120;
            StadiumSitting = StadiumSitting.Invalid;
            SquadNumber = -1;
            Moving = false;
            Appearance = new PlayerAppearance();
            Position = Vector2.Zero;
            Velocity = Vector3.Zero;
            Direction = Vector2.Zero;
            Invite = false;
            Unknown = string.Empty;
            CardStatus = MatchCard.None;
            XmlCode = string.Empty;
            LeagueGroupId = -1;
            LeagueId = -1;
            LeagueSortOrder = -1;
            EloPoint = 0f;
            TotalExperience = -1;
            ContractRevenueRate = 0f;
            Skills1 = new List<string>();
            Skills2 = new List<string>();
        }

        public PlayerMatchInformation(
            Guid matchId,
            Guid playerId, string playerName, PlayerAppearance appearance,
            StadiumSitting stadiumSitting, sbyte squadNumber,
            Vector2 position, Vector3 velocity, Vector2 direction,
            MatchCard cardStatus,
            string xmlCode
            )
        {
            MatchId = matchId;
            PlayerId = playerId;
            PlayerName = playerName;
            Appearance = appearance;
            StadiumSitting = stadiumSitting;
            SquadNumber = squadNumber;
            Position = position;
            Velocity = velocity;
            Direction = direction;
            CardStatus = cardStatus;
            XmlCode = xmlCode;
        }

        public PlayerMatchInformation(BinaryReader reader)
        {
            MatchId = GuidConverter.ConvertFromInt(reader.method_9());
            PlayerId = GuidConverter.ConvertFromInt(reader.method_9());
            PlayerName = reader.method_14();
            Stamina = reader.method_9();
            StadiumSitting = (StadiumSitting)reader.method_9();
            SquadNumber = reader.method_11();
            Unknown = reader.method_14();
            Moving = reader.method_1();
            Appearance = (PlayerAppearance)reader.method_25();
            Position = reader.method_19();
            Velocity = reader.method_20();
            Direction = GClass97.smethod_16(reader.method_12());
            Invite = reader.method_1();
            CardStatus = (MatchCard)reader.method_9();
            LeagueId = reader.method_9();
            LeagueSortOrder = reader.method_11();
            LeagueGroupId = reader.method_9();
            EloPoint = reader.method_12();

            int skillCount = reader.method_2();
            for (int i = 0; i < skillCount; i++)
                Skills1.Add(reader.method_14());

            skillCount = reader.method_2();
            for (int i = 0; i < skillCount; i++)
                Skills2.Add(reader.method_14());

            if (reader.method_1())
            {
                int decompressedLength = reader.method_9();
                byte[] compressedData = reader.method_3();
                using var memoryStream = new MemoryStream(compressedData);
                using var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress);
                var buffer = new byte[decompressedLength];
                gzipStream.Read(buffer, 0, buffer.Length);
                XmlCode = Encoding.Unicode.GetString(buffer);
            }
            else
            {
                XmlCode = reader.method_14();
            }
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_9(GuidConverter.ConvertToInt(MatchId));
            writer.method_9(GuidConverter.ConvertToInt(PlayerId));
            writer.method_14(PlayerName);
            writer.method_9(Stamina);
            writer.method_9((int)StadiumSitting);
            writer.method_11(SquadNumber);
            writer.method_14(Unknown);
            writer.method_1(Moving);
            writer.method_25(Appearance);
            writer.method_19(Position);
            writer.method_20(Velocity);
            writer.method_12(GClass97.smethod_15(Direction.Y, Direction.X));
            writer.method_1(Invite);
            writer.method_9((int)CardStatus);
            writer.method_9(LeagueId);
            writer.method_11(LeagueSortOrder);
            writer.method_9(LeagueGroupId);
            writer.method_12(EloPoint);

            writer.method_2((byte)Skills1.Count);
            foreach (var skill in Skills1)
                writer.method_14(skill);

            writer.method_2((byte)Skills2.Count);
            foreach (var skill in Skills2)
                writer.method_14(skill);

            bool compressXml = XmlCode.Length > 256;
            writer.method_1(compressXml);

            if (compressXml)
            {
                byte[] xmlBytes = Encoding.Unicode.GetBytes(XmlCode);
                writer.method_9(xmlBytes.Length);
                using var memoryStream = new MemoryStream();
                using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
                {
                    gzipStream.Write(xmlBytes, 0, xmlBytes.Length);
                }
                writer.method_3(memoryStream.ToArray());
            }
            else
            {
                writer.method_14(XmlCode);
            }
        }

        public override string ToString()
        {
            return $"MatchID:{MatchId} PlayerID:{PlayerId} PlayerName:{PlayerName} UserName:{UserName} " +
                   $"StdSit:{StadiumSitting} SquadNumber:{SquadNumber} CardStatus:{CardStatus} ";
        }
    }
}
