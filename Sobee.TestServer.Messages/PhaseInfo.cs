using Sobee.Network.Messaging;
using Sobee.Serialization;
using Sobee.TestServer.Messages.Match;

namespace Sobee.TestServer.Messages
{
    [MessageAttribute(12908)]
    public class PhaseInfo : Message
    {
        public byte ScoreHome { get; set; }
        public byte ScoreAway { get; set; }
        public double MatchTime { get; set; }
        public MatchPhase MatchPhase { get; set; }
        public bool[] UnknownFlag1 { get; set; }
        public bool[] UnknownFlag2 { get; set; }
        public byte byte_2 { get; set; }
        public byte byte_3 { get; set; }
        public short short_0 { get; set; }
        public short short_1 { get; set; }

        public bool IsFirstHalf() => MatchPhase is MatchPhase.FirstHalf or MatchPhase.ExtraFirstHalf;
        public bool IsSecondHalf() => MatchPhase is MatchPhase.SecondHalf or MatchPhase.ExtraSecondHalf;
        public int MatchTimeMilliseconds() => (int)(MatchTime * 1000.0);

        public PhaseInfo(int flagsLength)
        {
            ScoreHome = 0;
            ScoreAway = 0;
            MatchTime = 0.0;
            MatchPhase = MatchPhase.FirstHalf;
            UnknownFlag1 = new bool[flagsLength];
            UnknownFlag2 = new bool[flagsLength];
            byte_2 = byte_3 = 0;
            short_0 = short_1 = 0;
        }

        public PhaseInfo(BinaryReader reader)
        {
            ScoreHome = reader.method_2();
            ScoreAway = reader.method_2();
            MatchTime = reader.method_7();
            MatchPhase = (MatchPhase)reader.method_9();

            ushort num = reader.method_15();
            UnknownFlag1 = new bool[num];
            for (int i = 0; i < num; i++) UnknownFlag1[i] = reader.method_1();

            num = reader.method_15();
            UnknownFlag2 = new bool[num];
            for (int i = 0; i < num; i++) UnknownFlag2[i] = reader.method_1();

            byte_2 = reader.method_2();
            byte_3 = reader.method_2();
            short_0 = reader.method_8();
            short_1 = reader.method_8();
        }

        public string GetFormattedTime()
        {
            int minutes = (int)MatchTime / 60;
            int seconds = (int)MatchTime % 60;
            return $"{minutes:D2}:{seconds:D2}";
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.method_2(ScoreHome);
            writer.method_2(ScoreAway);
            writer.method_7(MatchTime);
            writer.method_9((int)MatchPhase);

            writer.method_15((ushort)UnknownFlag1.Length);
            for (int i = 0; i < UnknownFlag1.Length; i++) writer.method_1(UnknownFlag1[i]);

            writer.method_15((ushort)UnknownFlag2.Length);
            for (int i = 0; i < UnknownFlag2.Length; i++) writer.method_1(UnknownFlag2[i]);

            writer.method_2(byte_2);
            writer.method_2(byte_3);
            writer.method_8(short_0);
            writer.method_8(short_1);
        }
    }
}
