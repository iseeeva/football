using Sobee.Network.Messaging;
using Sobee.Serialization;
using System.ComponentModel;
using System.IO.Compression;
using System.Text;

namespace Sobee.TestServer.Messages
{
    // Token: 0x02000088 RID: 136
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [MessageAttribute(26929)]
    public class ScenarioInfo : Message
    {
        // Token: 0x06000363 RID: 867 RVA: 0x00004D46 File Offset: 0x00002F46
        public ScenarioType method_0()
        {
            return ScenarioType;
        }

        // Token: 0x06000366 RID: 870 RVA: 0x00004D5F File Offset: 0x00002F5F
        public string method_3()
        {
            return xmlCode;
        }

        // Token: 0x06000367 RID: 871 RVA: 0x00004D67 File Offset: 0x00002F67
        public string method_4()
        {
            return string_8;
        }

        // Token: 0x06000368 RID: 872 RVA: 0x00004D6F File Offset: 0x00002F6F
        public bool method_5()
        {
            return team1Size != -1 && team2Size != -1;
        }

        public ScenarioInfo()
        {
            // TODO: Remove hardcoded values when possible
            ScenarioType = ScenarioType.ScenarioMatch;

            var teamCapacities = GetDefaultScenarioCapacity(ScenarioType);
            if (teamCapacities == null)
                throw new Exception("ScenarioType invalid for getting player capacity.");

            team1Color = 2;
            team1Name = "HomeTeam.NameFull";
            team1ShortName = "HomeTeam.NameShort";
            team1Size = teamCapacities.Value.homeCapacity;

            team2Color = 3;
            team2Name = "AwayTeam.NameFull";
            team2ShortName = "AwayTeam.NameShort";
            team2Size = teamCapacities.Value.awayCapacity;

            xmlCode = "<XMLData><Script></Script></XMLData>";
        }

        // Token: 0x06000369 RID: 873 RVA: 0x0000D69C File Offset: 0x0000B89C
        public ScenarioInfo(BinaryReader gclass315_0)
        {
            int_0 = gclass315_0.method_9();
            bool_0 = gclass315_0.method_1();
            team1Color = gclass315_0.method_9();
            int_2 = gclass315_0.method_9();
            team2Color = gclass315_0.method_9();
            int_4 = gclass315_0.method_9();
            int_5 = gclass315_0.method_9();
            int_6 = gclass315_0.method_9();
            int_7 = gclass315_0.method_9();
            string_0 = gclass315_0.method_14();
            ScenarioType = (ScenarioType)gclass315_0.method_9();
            team1Size = gclass315_0.method_9();
            team2Size = gclass315_0.method_9();
            team1Name = gclass315_0.method_14();
            team2Name = gclass315_0.method_14();
            team1ShortName = gclass315_0.method_14();
            team2ShortName = gclass315_0.method_14();
            string_5 = gclass315_0.method_14();
            string_6 = gclass315_0.method_14();
            canInvite = gclass315_0.method_1();
            int_10 = gclass315_0.method_9();
            float_0 = gclass315_0.method_12();
            int_11 = gclass315_0.method_9();
            sbyte_0 = gclass315_0.method_11();
            int_12 = gclass315_0.method_9();
            float_1 = gclass315_0.method_12();
            int_13 = gclass315_0.method_9();
            sbyte_1 = gclass315_0.method_11();
            int_14 = gclass315_0.method_9();
            if (gclass315_0.method_1())
            {
                int num = gclass315_0.method_9();
                byte[] buffer = gclass315_0.method_3();
                byte[] array = new byte[num];
                MemoryStream stream = new MemoryStream(buffer);
                GZipStream gzipStream = new GZipStream(stream, CompressionMode.Decompress);
                gzipStream.Read(array, 0, array.Length);
                xmlCode = Encoding.Unicode.GetString(array);
                return;
            }
            xmlCode = gclass315_0.method_14();
        }

        // Token: 0x0600036A RID: 874 RVA: 0x0000D964 File Offset: 0x0000BB64
        public ScenarioInfo(int int_23, bool bool_3, bool bool_4, int int_24, string string_11, int int_25, int int_26, string string_12, int int_27, int int_28)
        {
            int_0 = int_23;
            bool_0 = bool_3;
            bool_1 = bool_4;
            team1Color = GClass165.int_2;
            int_2 = GClass165.int_4;
            team2Color = GClass165.int_3;
            int_4 = GClass165.int_5;
            int_5 = GClass165.int_0;
            int_6 = GClass165.int_1;
            int_7 = int_24;
            string_0 = string_11;
            team1Size = int_25;
            team2Size = int_26;
            xmlCode = string_12;
            int_17 = int_27;
            int_18 = int_28;
        }

        // Token: 0x0600036B RID: 875 RVA: 0x0000DB10 File Offset: 0x0000BD10
        public override void Serialize(BinaryWriter gclass316_0)
        {
            gclass316_0.method_9(int_0);
            gclass316_0.method_1(bool_0);
            gclass316_0.method_9(team1Color);
            gclass316_0.method_9(int_2);
            gclass316_0.method_9(team2Color);
            gclass316_0.method_9(int_4);
            gclass316_0.method_9(int_5);
            gclass316_0.method_9(int_6);
            gclass316_0.method_9(int_7);
            gclass316_0.method_14(string_0);
            gclass316_0.method_9((int)ScenarioType);
            gclass316_0.method_9(team1Size);
            gclass316_0.method_9(team2Size);
            gclass316_0.method_14(team1Name);
            gclass316_0.method_14(team2Name);
            gclass316_0.method_14(team1ShortName);
            gclass316_0.method_14(team2ShortName);
            gclass316_0.method_14(string_5);
            gclass316_0.method_14(string_6);
            gclass316_0.method_1(canInvite);
            gclass316_0.method_9(int_10);
            gclass316_0.method_12(float_0);
            gclass316_0.method_9(int_11);
            gclass316_0.method_11(sbyte_0);
            gclass316_0.method_9(int_12);
            gclass316_0.method_12(float_1);
            gclass316_0.method_9(int_13);
            gclass316_0.method_11(sbyte_1);
            gclass316_0.method_9(int_14);
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

        public static (int homeCapacity, int awayCapacity)? GetDefaultScenarioCapacity(ScenarioType scenarioType)
        {
            return scenarioType switch
            {
                ScenarioType.ScenarioMatch1v0 => (1, 0),
                ScenarioType.ScenarioMatch1v1 => (1, 1),
                ScenarioType.ScenarioMatch2v2 => (2, 2),
                ScenarioType.ScenarioMatch3v3 => (3, 3),
                ScenarioType.ScenarioMatch6v6 => (6, 6),
                ScenarioType.ScenarioMatch => (11, 11),
                _ => null,
            };
        }

        public bool IsMatchScenario()
        {
            return ScenarioType == ScenarioType.ScenarioMatch ||
                   ScenarioType == ScenarioType.ScenarioMatch1v0 ||
                   ScenarioType == ScenarioType.ScenarioMatch1v1 ||
                   ScenarioType == ScenarioType.ScenarioMatch2v2 ||
                   ScenarioType == ScenarioType.ScenarioMatch3v3 ||
                   ScenarioType == ScenarioType.ScenarioMatch6v6;
        }

        public bool IsSpecialScenario() => !IsMatchScenario();

        public bool IsMatch1v1() =>
            ScenarioType == ScenarioType.ScenarioMatch1v0 || ScenarioType == ScenarioType.ScenarioMatch1v1;

        public static readonly int MAX_TEAM_SIZE = 11;

        // Token: 0x0400067D RID: 1661
        public int int_0;

        // Token: 0x0400067E RID: 1662
        public bool bool_0;

        // Token: 0x0400067F RID: 1663
        public bool bool_1;

        // Token: 0x04000680 RID: 1664
        public int team1Color = -1;

        // Token: 0x04000681 RID: 1665
        public int int_2 = -1;

        // Token: 0x04000682 RID: 1666
        public int team2Color = -1;

        // Token: 0x04000683 RID: 1667
        public int int_4 = -1;

        // Token: 0x04000684 RID: 1668
        public int int_5;

        // Token: 0x04000685 RID: 1669
        public int int_6;

        // Token: 0x04000686 RID: 1670
        public int int_7 = -1;

        // Token: 0x04000687 RID: 1671
        public ScenarioType ScenarioType;

        // Token: 0x04000688 RID: 1672
        public string string_0 = string.Empty;

        // Token: 0x04000689 RID: 1673
        public int team1Size = -1;

        // Token: 0x0400068A RID: 1674
        public int team2Size = -1;

        // Token: 0x0400068B RID: 1675
        public bool canInvite;

        // Token: 0x0400068C RID: 1676
        public string team1Name = "$Home$";

        // Token: 0x0400068D RID: 1677
        public string team2Name = "$Away$";

        // Token: 0x0400068E RID: 1678
        public string team1ShortName = "$HomeShort$";

        // Token: 0x0400068F RID: 1679
        public string team2ShortName = "$AwayShort$";

        // Token: 0x04000690 RID: 1680
        public string string_5 = string.Empty;

        // Token: 0x04000691 RID: 1681
        public string string_6 = string.Empty;

        // Token: 0x04000692 RID: 1682
        public string xmlCode;

        // Token: 0x04000693 RID: 1683
        public string string_8 = string.Empty;

        // Token: 0x04000694 RID: 1684
        public int int_10;

        // Token: 0x04000695 RID: 1685
        public float float_0;

        // Token: 0x04000696 RID: 1686
        public int int_11 = -1;

        // Token: 0x04000697 RID: 1687
        public sbyte sbyte_0 = -1;

        // Token: 0x04000698 RID: 1688
        public int int_12 = -1;

        // Token: 0x04000699 RID: 1689
        public float float_1;

        // Token: 0x0400069A RID: 1690
        public int int_13 = -1;

        // Token: 0x0400069B RID: 1691
        public sbyte sbyte_1 = -1;

        // Token: 0x0400069C RID: 1692
        public int int_14 = -1;

        // Token: 0x0400069D RID: 1693
        public string string_9 = string.Empty;

        // Token: 0x0400069E RID: 1694
        public int int_15 = -1;

        // Token: 0x0400069F RID: 1695
        public int int_16 = -1;

        // Token: 0x040006A0 RID: 1696
        public int int_17 = -1;

        // Token: 0x040006A1 RID: 1697
        public int int_18 = -1;

        // Token: 0x040006A2 RID: 1698
        public int int_19 = -1;

        // Token: 0x040006A3 RID: 1699
        public int int_20;

        // Token: 0x040006A4 RID: 1700
        public float float_2;

        // Token: 0x040006A5 RID: 1701
        public float float_3;

        // Token: 0x040006A6 RID: 1702
        public float float_4;

        // Token: 0x040006A7 RID: 1703
        public float float_5;

        // Token: 0x040006A8 RID: 1704
        public double double_0 = 100.0;

        // Token: 0x040006A9 RID: 1705
        public double double_1 = 100.0;

        // Token: 0x040006AA RID: 1706
        public int int_21;

        // Token: 0x040006AB RID: 1707
        public int int_22;

    }
}