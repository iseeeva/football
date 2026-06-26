using Football.Common;
using Football.GameServer.Match;
using Football.GameServer.Messages;
using Football.GameServer.Messages.Match;
using Football.GameServer.Messages.Player;
using Football.Serialization.GameServer;
using Serilog;
using System.Numerics;

namespace Football.GameServer.MatchHelpers
{
    public class MatchPositioningHelper
    {
        private static readonly ILogger _log = LogFactory.GetContextForType<MatchPositioningHelper>();
        private static readonly int MAX_TEAM_SIZE = ScenarioInfo.MAX_TEAM_SIZE;

        public static MatchFormationHelper? ChangePosition(MatchRoom room, MatchFieldPositioning fieldType)
        {
            var scenario = room.MatchInformation.ScenarioInfo.ScenarioType;

            return scenario switch
            {
                ScenarioType.ScenarioMatch => fieldType switch
                {
                    MatchFieldPositioning.Kickoff => CreateKickoff(room, fieldType),
                    _ => throw new ArgumentException($"Invalid field type {fieldType} for scenario {scenario}.")
                },
                _ => throw new ArgumentException($"Invalid scenario type {scenario}.")
            };
        }

        private static MatchFormationHelper CreateKickoff(MatchRoom matchRoom, MatchFieldPositioning type)
        {
            var matchInfo = matchRoom.MatchInformation;
            var phaseInfo = matchInfo.PhaseInfo;

            var formation = MatchFormationHelper.GetFormationForScenario(matchInfo.ScenarioInfo.ScenarioType);

            var startingTeam = phaseInfo.IsFirstHalf()
                ? matchInfo.HomePlayer
                : matchInfo.AwayPlayer;

            if (startingTeam.Count == 0)
                throw new InvalidOperationException("Starting team has no players.");

            var actionerIndex = startingTeam.Count - 1;
            var actionerMatchInfo = startingTeam[actionerIndex];
            var actionerPlayer = matchRoom.Players[actionerMatchInfo.PlayerId];

            if (actionerPlayer == null)
                throw new InvalidOperationException($"MatchPlayer with ID {actionerMatchInfo.PlayerId} not found.");

            if (phaseInfo.IsFirstHalf())
                formation.Home.Positions[actionerIndex] = Vector2.Zero;
            else
            {
                formation.Away.Positions[actionerIndex] = Vector2.Zero;
                formation = MatchFormationHelper.SwapFormationSides(formation);
            }

            ApplyDefaultToUnuseds(formation.Home.Positions, formation.Home.Directions, matchInfo.HomePlayer.Count);
            ApplyDefaultToUnuseds(formation.Away.Positions, formation.Away.Directions, matchInfo.AwayPlayer.Count);

            matchInfo.MatchState = MatchState.Positioning;
            matchInfo.FieldPositioning = type;

            ApplyPositions(matchInfo.HomePlayer, formation.Home);
            ApplyPositions(matchInfo.AwayPlayer, formation.Away);

            matchRoom.Players.SendMessage(new PositioningCutscene(
                type,
                formation.Home.Positions,
                formation.Away.Positions,
                formation.Home.Directions,
                formation.Away.Directions,
                formation.Home.Animations,
                formation.Away.Animations
            ));

            if (!MatchBallHelper.GetBall(matchRoom, actionerMatchInfo.GetAbsoluteSquadNumber()))
                _log.Error("Failed ball assign for {PlayerId}", actionerPlayer.Id);

            return formation;
        }

        private static void ApplyPositions(List<PlayerMatchInformationMessage> players, MatchFormationHelper.Team team)
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].Position = team.Positions[i];
                players[i].Direction = team.Directions[i];
            }
        }

        private static void ApplyDefaultToUnuseds(List<Vector2> pos, List<Vector2> dir, int count)
        {
            for (int i = count; i < MAX_TEAM_SIZE; i++)
            {
                pos[i] = Vector2.Zero;
                dir[i] = Vector2.Zero;
            }
        }
    }
}
