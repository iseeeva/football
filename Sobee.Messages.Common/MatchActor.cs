namespace Sobee.Messages.Common
{
    public struct MatchActor : IDeserialize
    {
        public sbyte Camera;
        public sbyte Actioner;
        public int Mark;

        public MatchActor(sbyte camera, sbyte actioner, int mark)
        {
            Camera = camera;
            Actioner = actioner;
            Mark = mark;
        }

        public MatchActor(BinaryReader reader)
        {
            Camera = reader.method_11();
            Actioner = reader.method_11();
            Mark = reader.method_9();
        }

        public void Deserialize(BinaryWriter writer)
        {
            writer.method_11(Camera);
            writer.method_11(Actioner);
            writer.method_9(Mark);
        }
    }
}
