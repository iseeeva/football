using Sobee.Network.Messaging;
using Sobee.Serialization;
using Sobee.TestServer.Messages.Match;

namespace Sobee.TestServer.Messages.Auth
{
    [MessageAttribute(29475)]
    public sealed class AuthInformationRxMessage : Message
    {
        public readonly Version Version;
        public readonly long Unknown;
        public readonly string Password;
        public readonly string Cpu;
        public readonly string Gpu;
        public readonly int Ram;
        public readonly string Software;
        public readonly string Mac;
        public readonly MatchEntryNumber Entry;
        public readonly string Session;
        public readonly bool Autorun;

        public AuthInformationRxMessage(BinaryReader reader) : base(reader)
        {
            Version = reader.method_24();
            Unknown = reader.method_10();
            Password = reader.method_14();
            Cpu = reader.method_14();
            Gpu = reader.method_14();
            Ram = reader.method_9();
            Software = reader.method_14();
            Mac = reader.method_14();
            Entry = new MatchEntryNumber(reader);
            Session = reader.method_14();
            Autorun = reader.method_1();
        }

        public AuthInformationRxMessage(
            Version version,
            long unknown,
            string password,
            string cpu, string gpu, int ram, string software,
            string mac, MatchEntryNumber entry, string session, bool autorun)
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
            return $"Ver: {Version} MP: {Unknown} Pass: {Password} EntryID: {Entry.Value} WEB: {Session} Auto: {Autorun}";
        }
    }
}
