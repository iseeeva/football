using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Match
{
    public class MatchActor : ISerialize
    {
        /// <summary>
        /// Squad number (not splited) of the actor for camera 
        /// </summary>
        public sbyte Camera;

        /// <summary>
        /// Squad number (not splited) of the actor who have ball
        /// </summary>
        public sbyte Actioner;

        /// <summary>
        /// Entry number of the actor for map mark
        /// </summary>
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

        public void Serialize(BinaryWriter writer)
        {
            writer.method_11(Camera);
            writer.method_11(Actioner);
            writer.method_9(Mark);
        }
    }
}
