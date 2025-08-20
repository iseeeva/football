using Sobee.Messaging;
using Sobee.TestServer.Messages.Match;

namespace Sobee.TestServer.Messages.Player
{
    [GAttribute0(29475)]
    public sealed class PlayerInformation : Message
    {
        public Version Version { get; }
        public long Unknown { get; }
        public string Password { get; }
        public string Cpu { get; }
        public string Gpu { get; }
        public int Ram { get; }
        public string Software { get; }
        public string Mac { get; }
        public MatchEntry Entry { get; }
        public string Session { get; }
        public bool Autorun { get; }

        public PlayerInformation(BinaryReader reader) : base(reader)
        {
            Version = reader.method_24();
            Unknown = reader.method_10();
            Password = reader.method_14();
            Cpu = reader.method_14();
            Gpu = reader.method_14();
            Ram = reader.method_9();
            Software = reader.method_14();
            Mac = reader.method_14();
            Entry = new MatchEntry(reader);
            Session = reader.method_14();
            Autorun = reader.method_1();
        }

        public PlayerInformation(
            Version version,
            long unknown,
            string password,
            string cpu, string gpu, int ram, string software,
            string mac, MatchEntry entry, string session, bool autorun)
        {
            Version = version;
            Unknown = unknown;
            Password = password;
            Cpu = cpu;
            Gpu = gpu;
            Ram = ram;
            Software = software;
            Mac = mac;
            Entry = entry;
            Session = session;
            Autorun = autorun;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_23(Version);
            writer.method_10(Unknown);
            writer.method_14(Password);
            writer.method_14(Cpu);
            writer.method_14(Gpu);
            writer.method_9(Ram);
            writer.method_14(Software);
            writer.method_14(Mac);
            Entry.Serialize(writer);
            writer.method_14(Session);
            writer.method_1(Autorun);
        }

        public override string ToString()
        {
            return $"Ver: {Version} MP: {Unknown} Pass: {Password} EntryID: {Entry.EntryId} WEB: {Session} Auto: {Autorun}";
        }
    }
}
