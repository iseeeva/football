using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Player
{
    public class PlayerSquadNumber : ISerialize
    {
        // INFO:
        // Q: What is the "Absolute" squad number? (h0-a11)
        // A: Absolute squad number is used by client for identify players by combines both squads squad numbers into a single range.
        // For example, look GetSittingFromAbsoluteSquadNumber and ConvertToAbsoluteSquadNumber methods.

        private sbyte _value;
        public sbyte Value
        {
            get => _value;
            set
            {
                if (value == -1)
                {
                    // Allow -1 as unassigned because client.
                    _value = value;
                    return;
                }

                if (IsValidSquadNumber(value))
                    _value = value;
                else
                    throw new ArgumentOutOfRangeException(nameof(value), "Squad number is not in valid range.");
            }
        }

        public static implicit operator sbyte(PlayerSquadNumber squadNumber)
            => squadNumber.Value;

        public PlayerSquadNumber(sbyte squadNumber)
        {
            Value = squadNumber;
        }

        public PlayerSquadNumber(BinaryReader reader)
        {
            ArgumentNullException.ThrowIfNull(reader);
            Value = reader.method_11();
        }

        public static bool IsValidSquadNumber(sbyte squadNumber)
        {
            return
                Math.Abs(squadNumber) % 1 == 0 &&
                squadNumber >= 0 &&
                squadNumber <= ScenarioInfo.MAX_TEAM_SIZE - 1;
        }

        public static bool IsValidAbsoluteSquadNumber(sbyte absoluteSquadNumber)
        {
            return
                Math.Abs(absoluteSquadNumber) % 1 == 0 &&
                absoluteSquadNumber >= 0 &&
                absoluteSquadNumber <= (ScenarioInfo.MAX_TEAM_SIZE * 2) - 1;
        }

        public static sbyte ConvertToSquadNumber(sbyte absoluteSquadNumber)
        {
            if (!IsValidAbsoluteSquadNumber(absoluteSquadNumber))
                throw new ArgumentOutOfRangeException(nameof(absoluteSquadNumber), "Squad number (absolute) not in valid range");

            return (sbyte)(absoluteSquadNumber % ScenarioInfo.MAX_TEAM_SIZE);
        }

        public static sbyte ConvertToAbsoluteSquadNumber(StadiumSitting sitting, sbyte squadNumber)
        {
            if (!IsValidSquadNumber(squadNumber))
                throw new ArgumentOutOfRangeException(nameof(squadNumber), "Squad number not in valid range");

            return sitting switch
            {
                StadiumSitting.HomePlayer => squadNumber,
                StadiumSitting.AwayPlayer => (sbyte)(squadNumber + ScenarioInfo.MAX_TEAM_SIZE),
                StadiumSitting.HomeSpectator => squadNumber,
                StadiumSitting.AwaySpectator => (sbyte)(squadNumber + ScenarioInfo.MAX_TEAM_SIZE),
                StadiumSitting.Spectator => squadNumber,
                _ => squadNumber,
            };
        }

        public static StadiumSitting GetSittingFromAbsoluteSquadNumber(sbyte absoluteSquadNumber, StadiumSitting[] stadiumSittings)
        {
            if (!IsValidAbsoluteSquadNumber(absoluteSquadNumber))
                return StadiumSitting.Invalid;
            // throw new ArgumentOutOfRangeException(nameof(absoluteSquadNumber), "Squad number (absolute) not in valid range");

            var teamIndex = Math.Floor((double)(absoluteSquadNumber / ScenarioInfo.MAX_TEAM_SIZE));
            //if (Math.Abs(teamIndex) % 1 != 0)
            //    throw new ArgumentException("Team index is not an integer", nameof(absoluteSquadNumber));
            //else if (teamIndex < 0 || teamIndex >= stadiumSittings.Length)
            //    throw new ArgumentOutOfRangeException(nameof(absoluteSquadNumber), "Team index derived from absolute squad number is out of range");

            if (
                Math.Abs(teamIndex) % 1 != 0 || // not integer
                teamIndex < 0 || teamIndex >= stadiumSittings.Length // out of range
               )
                return StadiumSitting.Invalid;

            return stadiumSittings[(int)teamIndex];
        }

        public void Serialize(BinaryWriter writer)
        {
            ArgumentNullException.ThrowIfNull(writer);
            writer.method_11(Value);
        }
    }
}
