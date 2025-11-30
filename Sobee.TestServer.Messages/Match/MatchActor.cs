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
        public sbyte BallOwner;

        /// <summary>
        /// Client id of the actor
        /// </summary>
        public Guid ClientId;

        public MatchActor(sbyte camera, sbyte actioner, Guid clientId)
        {
            Camera = camera;
            BallOwner = actioner;
            ClientId = clientId;
        }

        public MatchActor(BinaryReader reader)
        {
            Camera = reader.method_11();
            BallOwner = reader.method_11();
            ClientId = GuidConverter.ConvertFromInt(reader.method_9());
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.method_11(Camera);
            writer.method_11(BallOwner);
            writer.method_9(GuidConverter.ConvertToInt(ClientId));
        }
    }
}
