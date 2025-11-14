using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Chat
{
    [GAttribute0(9210)]
    public class ChatPlayerMessage : ChatBase
    {
        public Guid PlayerId { get; }
        public string MessageText { get; }
        public byte ColorNumber { get; }

        public ChatPlayerMessage(Guid teamId, Guid playerGuid, string messageText, byte colorNumber) : base(teamId)
        {
            this.PlayerId = playerGuid;
            this.MessageText = messageText;
            this.ColorNumber = colorNumber;
        }

        public ChatPlayerMessage(BinaryReader gclass315_0) : base(gclass315_0)
        {
            this.PlayerId = GuidConverter.ConvertFromInt(gclass315_0.method_9());
            this.MessageText = gclass315_0.method_14();
            this.ColorNumber = gclass315_0.method_2();
        }

        public override void Serialize(BinaryWriter gclass316_0)
        {
            base.Serialize(gclass316_0);
            gclass316_0.method_9(GuidConverter.ConvertToInt(this.PlayerId));
            gclass316_0.method_14(this.MessageText);
            gclass316_0.method_2(this.ColorNumber);
        }
    }
}
