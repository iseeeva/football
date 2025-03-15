namespace Sobee.Messaging.Events
{
    class Parser
    {
        public readonly ushort Id;
        public readonly uint Size;
        public readonly byte[] Content;

        public Parser(byte[] Input)
        {
            var Reader = new BinaryReader(new MemoryStream(Input));
            Size = Reader.ReadUInt32();
            Id = Reader.ReadUInt16();

            byte[] Temporary = new byte[4096]; // 4 Kilobayt
            Reader.BaseStream.Read(Temporary, ((int)Reader.BaseStream.Position), (int)(Reader.BaseStream.Length - Reader.BaseStream.Position));

            Content = Temporary;
        }
    }
}
