using Sobee.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Player
{
    [MessageAttribute(8993)]
    public class PlayerAppearance : Message
    {
        public byte Part0 { get; set; }
        public byte Part1 { get; set; }
        public byte Part2 { get; set; }
        public byte Part3 { get; set; }
        public byte Part4 { get; set; }
        public byte Part5 { get; set; }
        public byte Part6 { get; set; }
        public byte Part7 { get; set; }
        public byte Part8 { get; set; }
        public byte Part9 { get; set; }
        public byte Part10 { get; set; }
        public byte Part11 { get; set; }
        public byte Part12 { get; set; }
        public byte Part13 { get; set; }
        public byte Part14 { get; set; }
        public byte Part15 { get; set; }
        public byte Part16 { get; set; }
        public byte Part17 { get; set; }
        public byte Part18 { get; set; }
        public byte Part19 { get; set; }
        public byte Part20 { get; set; }
        public byte Part21 { get; set; }
        public byte Part22 { get; set; }
        public byte Part23 { get; set; }
        public byte Part24 { get; set; }
        public byte Part25 { get; set; }
        public byte Part26 { get; set; }
        public byte Part27 { get; set; }
        public byte Part28 { get; set; }
        public byte Part29 { get; set; }
        public byte Part30 { get; set; }
        public byte Part31 { get; set; }
        public byte Part32 { get; set; }
        public byte Part33 { get; set; }
        public byte Part34 { get; set; }
        public byte Part35 { get; set; }
        public byte Part36 { get; set; }
        public byte Part37 { get; set; }
        public byte Part38 { get; set; }
        public byte Part39 { get; set; }
        public byte Part40 { get; set; }
        public byte Part41 { get; set; }
        public byte Part42 { get; set; }
        public byte Part43 { get; set; }
        public byte Part44 { get; set; }
        public byte Part45 { get; set; }
        public byte Part46 { get; set; }
        public byte Part47 { get; set; }
        public byte Part48 { get; set; }
        public byte Part49 { get; set; }
        public byte Part50 { get; set; }
        public byte Part51 { get; set; }
        public byte Part52 { get; set; }
        public byte Part53 { get; set; }
        public byte Part54 { get; set; }
        public byte Part55 { get; set; }
        public byte Part56 { get; set; }
        public byte Part57 { get; set; }
        public byte Part58 { get; set; }
        public byte Part59 { get; set; }

        public PlayerAppearance()
        {
            Part0 = 128; Part1 = 128; Part2 = 128; Part3 = 128; Part4 = 128;
            Part5 = 128; Part6 = 128; Part7 = 128; Part8 = 128; Part9 = 128;
            Part10 = 128; Part11 = 128; Part12 = 128; Part13 = 128; Part14 = 128;
            Part15 = 128; Part16 = 128; Part17 = 128; Part18 = 128; Part19 = 128;
            Part20 = 128; Part21 = 128; Part22 = 128; Part23 = 128; Part24 = 128;
            Part25 = 128; Part26 = 128; Part27 = 128; Part28 = 128; Part29 = 128;
            Part30 = 128; Part31 = 128; Part32 = 128; Part33 = 128; Part34 = 128;
            Part35 = 128; Part36 = 128; Part37 = 128; Part38 = 128; Part39 = 128;
            Part40 = 128; Part41 = 128; Part42 = 128; Part43 = 128; Part44 = 128;

            Part45 = 0; Part46 = 0; Part47 = 0; Part48 = 0; Part49 = 0;
            Part50 = 0; Part51 = 0; Part52 = 0;
            Part53 = 44; Part54 = 79; Part55 = 0;
            Part56 = 128; Part57 = 128; Part58 = 0; Part59 = 0;
        }

        public PlayerAppearance(
            byte part0, byte part1, byte part2, byte part3, byte part4, byte part5,
            byte part6, byte part7, byte part8, byte part9, byte part10, byte part11,
            byte part12, byte part13, byte part14, byte part15, byte part16, byte part17,
            byte part18, byte part19, byte part20, byte part21, byte part22, byte part23,
            byte part24, byte part25, byte part26, byte part27, byte part28, byte part29,
            byte part30, byte part31, byte part32, byte part33, byte part34, byte part35,
            byte part36, byte part37, byte part38, byte part39, byte part40, byte part41,
            byte part42, byte part43, byte part44, byte part45, byte part46, byte part47,
            byte part48, byte part49, byte part50, byte part51, byte part52, byte part53,
            byte part54, byte part55, byte part56, byte part57, byte part58, byte part59)
        {
            Part0 = part0; Part1 = part1; Part2 = part2; Part3 = part3; Part4 = part4;
            Part5 = part5; Part6 = part6; Part7 = part7; Part8 = part8; Part9 = part9;
            Part10 = part10; Part11 = part11; Part12 = part12; Part13 = part13; Part14 = part14;
            Part15 = part15; Part16 = part16; Part17 = part17; Part18 = part18; Part19 = part19;
            Part20 = part20; Part21 = part21; Part22 = part22; Part23 = part23; Part24 = part24;
            Part25 = part25; Part26 = part26; Part27 = part27; Part28 = part28; Part29 = part29;
            Part30 = part30; Part31 = part31; Part32 = part32; Part33 = part33; Part34 = part34;
            Part35 = part35; Part36 = part36; Part37 = part37; Part38 = part38; Part39 = part39;
            Part40 = part40; Part41 = part41; Part42 = part42; Part43 = part43; Part44 = part44;
            Part45 = part45; Part46 = part46; Part47 = part47; Part48 = part48; Part49 = part49;
            Part50 = part50; Part51 = part51; Part52 = part52; Part53 = part53; Part54 = part54;
            Part55 = part55; Part56 = part56; Part57 = part57; Part58 = part58; Part59 = part59;
        }

        public PlayerAppearance(BinaryReader reader)
        {
            Part0 = reader.method_2(); Part1 = reader.method_2(); Part2 = reader.method_2();
            Part3 = reader.method_2(); Part4 = reader.method_2(); Part5 = reader.method_2();
            Part6 = reader.method_2(); Part7 = reader.method_2(); Part8 = reader.method_2();
            Part9 = reader.method_2(); Part10 = reader.method_2(); Part11 = reader.method_2();
            Part12 = reader.method_2(); Part13 = reader.method_2(); Part14 = reader.method_2();
            Part15 = reader.method_2(); Part16 = reader.method_2(); Part17 = reader.method_2();
            Part18 = reader.method_2(); Part19 = reader.method_2(); Part20 = reader.method_2();
            Part21 = reader.method_2(); Part22 = reader.method_2(); Part23 = reader.method_2();
            Part24 = reader.method_2(); Part25 = reader.method_2(); Part26 = reader.method_2();
            Part27 = reader.method_2(); Part28 = reader.method_2(); Part29 = reader.method_2();
            Part30 = reader.method_2(); Part31 = reader.method_2(); Part32 = reader.method_2();
            Part33 = reader.method_2(); Part34 = reader.method_2(); Part35 = reader.method_2();
            Part36 = reader.method_2(); Part37 = reader.method_2(); Part38 = reader.method_2();
            Part39 = reader.method_2(); Part40 = reader.method_2(); Part41 = reader.method_2();
            Part42 = reader.method_2(); Part43 = reader.method_2(); Part44 = reader.method_2();
            Part45 = reader.method_2(); Part46 = reader.method_2(); Part47 = reader.method_2();
            Part48 = reader.method_2(); Part49 = reader.method_2(); Part50 = reader.method_2();
            Part51 = reader.method_2(); Part52 = reader.method_2(); Part53 = reader.method_2();
            Part54 = reader.method_2(); Part55 = reader.method_2(); Part56 = reader.method_2();
            Part57 = reader.method_2(); Part58 = reader.method_2(); Part59 = reader.method_2();
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.method_2(Part0); writer.method_2(Part1); writer.method_2(Part2); writer.method_2(Part3);
            writer.method_2(Part4); writer.method_2(Part5); writer.method_2(Part6); writer.method_2(Part7);
            writer.method_2(Part8); writer.method_2(Part9); writer.method_2(Part10); writer.method_2(Part11);
            writer.method_2(Part12); writer.method_2(Part13); writer.method_2(Part14); writer.method_2(Part15);
            writer.method_2(Part16); writer.method_2(Part17); writer.method_2(Part18); writer.method_2(Part19);
            writer.method_2(Part20); writer.method_2(Part21); writer.method_2(Part22); writer.method_2(Part23);
            writer.method_2(Part24); writer.method_2(Part25); writer.method_2(Part26); writer.method_2(Part27);
            writer.method_2(Part28); writer.method_2(Part29); writer.method_2(Part30); writer.method_2(Part31);
            writer.method_2(Part32); writer.method_2(Part33); writer.method_2(Part34); writer.method_2(Part35);
            writer.method_2(Part36); writer.method_2(Part37); writer.method_2(Part38); writer.method_2(Part39);
            writer.method_2(Part40); writer.method_2(Part41); writer.method_2(Part42); writer.method_2(Part43);
            writer.method_2(Part44); writer.method_2(Part45); writer.method_2(Part46); writer.method_2(Part47);
            writer.method_2(Part48); writer.method_2(Part49); writer.method_2(Part50); writer.method_2(Part51);
            writer.method_2(Part52); writer.method_2(Part53); writer.method_2(Part54); writer.method_2(Part55);
            writer.method_2(Part56); writer.method_2(Part57); writer.method_2(Part58); writer.method_2(Part59);
        }

        public void CopyFrom(PlayerAppearance other)
        {
            ArgumentNullException.ThrowIfNull(other);
            Part0 = other.Part0; Part1 = other.Part1; Part2 = other.Part2; Part3 = other.Part3;
            Part4 = other.Part4; Part5 = other.Part5; Part6 = other.Part6; Part7 = other.Part7;
            Part8 = other.Part8; Part9 = other.Part9; Part10 = other.Part10; Part11 = other.Part11;
            Part12 = other.Part12; Part13 = other.Part13; Part14 = other.Part14; Part15 = other.Part15;
            Part16 = other.Part16; Part17 = other.Part17; Part18 = other.Part18; Part19 = other.Part19;
            Part20 = other.Part20; Part21 = other.Part21; Part22 = other.Part22; Part23 = other.Part23;
            Part24 = other.Part24; Part25 = other.Part25; Part26 = other.Part26; Part27 = other.Part27;
            Part28 = other.Part28; Part29 = other.Part29; Part30 = other.Part30; Part31 = other.Part31;
            Part32 = other.Part32; Part33 = other.Part33; Part34 = other.Part34; Part35 = other.Part35;
            Part36 = other.Part36; Part37 = other.Part37; Part38 = other.Part38; Part39 = other.Part39;
            Part40 = other.Part40; Part41 = other.Part41; Part42 = other.Part42; Part43 = other.Part43;
            Part44 = other.Part44; Part45 = other.Part45; Part46 = other.Part46; Part47 = other.Part47;
            Part48 = other.Part48; Part49 = other.Part49; Part50 = other.Part50; Part51 = other.Part51;
            Part52 = other.Part52; Part53 = other.Part53; Part54 = other.Part54; Part55 = other.Part55;
            Part56 = other.Part56; Part57 = other.Part57; Part58 = other.Part58; Part59 = other.Part59;
        }
    }
}