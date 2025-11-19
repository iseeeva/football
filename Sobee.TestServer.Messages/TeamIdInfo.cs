using Sobee.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages
{
    [MessageAttribute(4932)]
    public sealed class TeamIdInfo : Message
    {
        public Guid InvalidId { get; }
        public Guid HomePlayerId { get; }
        public Guid AwayPlayerId { get; }
        public Guid SpectatorId { get; }
        public Guid HomeSpectatorId { get; }
        public Guid AwaySpectatorId { get; }

        public TeamIdInfo()
        {
            InvalidId = Guid.NewGuid();
            HomePlayerId = Guid.NewGuid();
            AwayPlayerId = Guid.NewGuid();
            SpectatorId = Guid.NewGuid();
            HomeSpectatorId = Guid.NewGuid();
            AwaySpectatorId = Guid.NewGuid();
        }

        public TeamIdInfo(BinaryReader reader)
        {
            InvalidId = GuidConverter.ConvertFromInt(reader.method_9());
            HomePlayerId = GuidConverter.ConvertFromInt(reader.method_9());
            AwayPlayerId = GuidConverter.ConvertFromInt(reader.method_9());
            SpectatorId = GuidConverter.ConvertFromInt(reader.method_9());
            HomeSpectatorId = GuidConverter.ConvertFromInt(reader.method_9());
            AwaySpectatorId = GuidConverter.ConvertFromInt(reader.method_9());
        }

        public TeamIdInfo(
            Guid invalidId,
            Guid homePlayerId,
            Guid awayPlayerId,
            Guid spectatorId,
            Guid homeSpectatorId,
            Guid awaySpectatorId)
        {
            InvalidId = invalidId;
            HomePlayerId = homePlayerId;
            AwayPlayerId = awayPlayerId;
            SpectatorId = spectatorId;
            HomeSpectatorId = homeSpectatorId;
            AwaySpectatorId = awaySpectatorId;
        }

        public Guid GetId(StadiumSitting sitting) => sitting switch
        {
            StadiumSitting.Invalid => InvalidId,
            StadiumSitting.HomePlayer => HomePlayerId,
            StadiumSitting.AwayPlayer => AwayPlayerId,
            StadiumSitting.Spectator => SpectatorId,
            StadiumSitting.HomeSpectator => HomeSpectatorId,
            StadiumSitting.AwaySpectator => AwaySpectatorId,
            _ => Guid.Empty
        };

        public override void Serialize(BinaryWriter writer)
        {
            writer.method_9(GuidConverter.ConvertToInt(InvalidId));
            writer.method_9(GuidConverter.ConvertToInt(HomePlayerId));
            writer.method_9(GuidConverter.ConvertToInt(AwayPlayerId));
            writer.method_9(GuidConverter.ConvertToInt(SpectatorId));
            writer.method_9(GuidConverter.ConvertToInt(HomeSpectatorId));
            writer.method_9(GuidConverter.ConvertToInt(AwaySpectatorId));
        }
    }
}
