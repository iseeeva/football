using System.IO.Compression;
using System.Numerics;
using System.Text;

namespace Sobee.Messages.Common.Player
{
    [GAttribute0(10483)]
    public class MatchInformation : Message
    {
        public int MatchID = -1;
        public int PlayerID = -1;
        public string PlayerName;
        public string UserName;
        public int int_2;
        public StadiumSitting stadiumSitting_0;
        public sbyte SquadNumber = -1;
        public bool bool_0;
        public Appearance Appearance = new Appearance();
        public Vector2 vector2_0;
        public Vector3 vector3_0;
        public float float_0;
        public bool bool_1;
        public bool bool_2;
        public string string_2 = string.Empty;
        public MatchCard CardStatus;
        public string xmlCode;
        public string string_4 = string.Empty;
        public int LeagueGroupId = -1;
        public int LeagueId = -1;
        public sbyte LeagueSortOrder = -1;
        public float EloPoint;
        public int TotalExperience = -1;
        public float ContractRevenueRate;
        public int int_6;
        public List<string> list_0 = new List<string>();
        public List<string> list_1 = new List<string>();


        public MatchInformation(int int_7, int int_8, string string_6, StadiumSitting stadiumSitting_1, sbyte sbyte_2, Vector2 vector2_1, Vector3 vector3_1, float float_3, MatchCard matchCard_1, string string_7)
        {
            MatchID = int_7;
            PlayerID = int_8;
            PlayerName = string_6;
            stadiumSitting_0 = stadiumSitting_1;
            SquadNumber = sbyte_2;
            vector2_0 = vector2_1;
            vector3_0 = vector3_1;
            float_0 = float_3;
            CardStatus = matchCard_1;
            xmlCode = string_7;
        }

        public MatchInformation(BinaryReader gclass315_0)
        {
            MatchID = gclass315_0.method_9();
            PlayerID = gclass315_0.method_9();
            PlayerName = gclass315_0.method_14();
            int_2 = gclass315_0.method_9();
            stadiumSitting_0 = (StadiumSitting)gclass315_0.method_9();
            SquadNumber = gclass315_0.method_11();
            string_2 = gclass315_0.method_14();
            bool_0 = gclass315_0.method_1();
            Appearance = (Appearance)gclass315_0.method_25();
            vector2_0 = gclass315_0.method_19();
            vector3_0 = gclass315_0.method_20();
            float_0 = gclass315_0.method_12();
            bool_1 = gclass315_0.method_1();
            CardStatus = (MatchCard)gclass315_0.method_9();
            LeagueId = gclass315_0.method_9();
            LeagueSortOrder = gclass315_0.method_11();
            LeagueGroupId = gclass315_0.method_9();
            EloPoint = gclass315_0.method_12();
            int num = gclass315_0.method_2();
            for (int i = 0; i < num; i++)
            {
                list_0.Add(gclass315_0.method_14());
            }
            num = gclass315_0.method_2();
            for (int j = 0; j < num; j++)
            {
                list_1.Add(gclass315_0.method_14());
            }
            if (gclass315_0.method_1())
            {
                int num2 = gclass315_0.method_9();
                byte[] buffer = gclass315_0.method_3();
                byte[] array = new byte[num2];
                MemoryStream stream = new MemoryStream(buffer);
                GZipStream gzipStream = new GZipStream(stream, CompressionMode.Decompress);
                gzipStream.Read(array, 0, array.Length);
                xmlCode = Encoding.Unicode.GetString(array);
                return;
            }
            xmlCode = gclass315_0.method_14();
        }

        public override void Deserialize(BinaryWriter gclass316_0)
        {
            base.Deserialize(gclass316_0);
            gclass316_0.method_9(MatchID);
            gclass316_0.method_9(PlayerID);
            gclass316_0.method_14(PlayerName);
            gclass316_0.method_9(int_2);
            gclass316_0.method_9((int)stadiumSitting_0);
            gclass316_0.method_11(SquadNumber);
            gclass316_0.method_14(string_2);
            gclass316_0.method_1(bool_0);
            gclass316_0.method_25(Appearance);
            gclass316_0.method_19(vector2_0);
            gclass316_0.method_20(vector3_0);
            gclass316_0.method_12(float_0);
            gclass316_0.method_1(bool_1);
            gclass316_0.method_9((int)CardStatus);
            gclass316_0.method_9(LeagueId);
            gclass316_0.method_11(LeagueSortOrder);
            gclass316_0.method_9(LeagueGroupId);
            gclass316_0.method_12(EloPoint);
            gclass316_0.method_2((byte)list_0.Count);
            for (int i = 0; i < list_0.Count; i++)
            {
                gclass316_0.method_14(list_0[i]);
            }
            gclass316_0.method_2((byte)list_1.Count);
            for (int j = 0; j < list_1.Count; j++)
            {
                gclass316_0.method_14(list_1[j]);
            }
            bool flag = xmlCode.Length > 256;
            gclass316_0.method_1(flag);
            if (flag)
            {
                byte[] bytes = Encoding.Unicode.GetBytes(xmlCode);
                gclass316_0.method_9(bytes.Length);
                MemoryStream memoryStream = new MemoryStream(bytes.Length);
                GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Compress);
                gzipStream.Write(bytes, 0, bytes.Length);
                gzipStream.Close();
                gclass316_0.method_3(memoryStream.ToArray());
                return;
            }
            gclass316_0.method_14(xmlCode);
        }

        public bool method_2()
        {
            return CardStatus == MatchCard.Red || CardStatus == MatchCard.YellowToRed;
        }

        public sbyte method_3()
        {
            if (SquadNumber == -1 || method_6())
            {
                return SquadNumber;
            }
            if (!method_4())
            {
                return (sbyte)(SquadNumber + 11);
            }
            return SquadNumber;
        }

        public bool method_4()
        {
            return stadiumSitting_0 == StadiumSitting.HomePlayer;
        }

        public bool method_5()
        {
            return stadiumSitting_0 == StadiumSitting.AwayPlayer;
        }

        public bool method_6()
        {
            return stadiumSitting_0 == StadiumSitting.HomeSpectator || stadiumSitting_0 == StadiumSitting.AwaySpectator;
        }

        public bool method_7()
        {
            return method_4() || stadiumSitting_0 == StadiumSitting.HomeSpectator;
        }

        public bool method_8()
        {
            return method_5() || stadiumSitting_0 == StadiumSitting.AwaySpectator;
        }

        public virtual string ToString()
        {
            return string.Concat(
               "MatchID:", MatchID,
               " PlayerID:", PlayerID,
               " PlayerName:", PlayerName,
               " UserName:", UserName,
               " StdSit:", stadiumSitting_0.ToString(),
               " SquadNumber:", SquadNumber,
               " CardStatus:", CardStatus,
               " LeagueGroupId:", LeagueGroupId,
               " LeagueID:", LeagueId,
               " LeagueSortOrder:", LeagueSortOrder,
               " EloPoint:", EloPoint,
               " ContractRevenueRate:", ContractRevenueRate,
               " TotalExperience:", TotalExperience
            );
        }
    }
}
