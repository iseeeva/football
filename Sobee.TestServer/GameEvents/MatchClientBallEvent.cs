using System.Numerics;
using Sobee.Common;
using Sobee.Messaging;
using Sobee.Serialization.GameServer;
using Sobee.TestServer.Match;
using Sobee.TestServer.MatchComponents;
using Sobee.TestServer.Messages.Ball;
using Sobee.TestServer.Messages.Match;

namespace Sobee.TestServer.GameEvents
{
    public class MatchClientBallEvent
    {
        private static readonly Serilog.ILogger _log = Logging.Get<MatchClientBallEvent>();

        public static void ActionerHitReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not MatchRoom matchRoom) return;
            if (e.handler is not MatchPlayer matchPlayer) return;
            if (e.message is not BallActionerHit actionerHit) return;

            var ballComponent = matchRoom.Components.GetComponent<MatchBallComponent>();
            if (ballComponent == null)
            {
                _log.Warning("[ActionerHitReceived] MatchBallComponent is null in MatchRoom {matchId}.", matchRoom.Id);
                return;
            }

            var matchPlayerInfo = matchRoom.MatchInformation.GetPlayer(matchPlayer.Id);
            var actionerPlayerInfo = matchRoom.MatchInformation.GetPlayer(matchRoom.MatchInformation.Actor.Actioner);

            if (actionerPlayerInfo == null || matchPlayerInfo == null)
            {
                _log.Warning("[ActionerHitReceived] Could not find player info in match.");
                return;
            }

            // Gonderen sadece actioner olsun.
            if (actionerPlayerInfo.PlayerId != matchPlayerInfo.PlayerId)
            {
                _log.Warning("[ActionerHitReceived] Player {playerId} is not the actioner {actionerId}.", matchPlayer.Id, actionerPlayerInfo.PlayerId);
                return;
            }

            double ballSpeed = ballComponent.BallMaxSpeed * actionerHit.Strength;
            double ballHitSafe = ballComponent.BallCollisionRadius + 0.1;

            actionerPlayerInfo.Direction = actionerHit.Direction;

            matchRoom.MatchInformation.BallPosition = new Vector3(
              (float)(actionerPlayerInfo.Position.X + actionerPlayerInfo.Direction.X * ballHitSafe),
              (float)(actionerPlayerInfo.Position.Y + actionerPlayerInfo.Direction.Y * ballHitSafe),
              (float)(ballComponent.BallBoundry.Z)
            );

            matchRoom.MatchInformation.BallVelocity = new Vector3(
              (float)(actionerHit.Direction.X * ballSpeed),
              (float)(actionerHit.Direction.Y * ballSpeed),
              0
            );

            switch (matchRoom.MatchInformation.FieldPositioning)
            {
                case MatchFieldPositioning.Kickoff:
                    {
                        matchRoom.Players.SendMessage(new BallKickoffHit(
                            matchRoom.MatchInformation.BallVelocity,
                            Messages.AnimationType.ShootLeft
                        ));
                    }
                    break;
            }

            matchRoom.Players.SendMessage(new Messages.Chat.ChatSystemMessage($"[ActionerHitReceived] {matchRoom.MatchInformation.FieldPositioning} by {actionerPlayerInfo.PlayerName}", Messages.Chat.ChatSystemMessageType.General));
            matchRoom.Players.SendMessage(new Messages.Chat.ChatSystemMessage($"[ActionerHitReceived] Strength: {actionerHit.Strength}, HitSubType: {actionerHit.HitSubType}, Direction: {actionerHit.Direction}", Messages.Chat.ChatSystemMessageType.General));
            _log.Information("[ActionerHitReceived] Actioner {playerId} hit the ball during {fieldPos}.", actionerPlayerInfo.PlayerId, matchRoom.MatchInformation.FieldPositioning);

            matchRoom.MatchInformation.Actor.Actioner = -1;
            matchRoom.MatchInformation.MatchState = MatchStateType.Running;
        }
    }
}
