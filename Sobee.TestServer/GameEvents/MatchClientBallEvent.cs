using System.Numerics;
using Sobee.Common;
using Sobee.Network.Messaging;
using Sobee.Serialization.GameServer;
using Sobee.TestServer.Match;
using Sobee.TestServer.MatchComponents;
using Sobee.TestServer.Messages;
using Sobee.TestServer.Messages.Ball;
using Sobee.TestServer.Messages.Match;

namespace Sobee.TestServer.GameEvents
{
    public class MatchClientBallEvent
    {
        private static readonly Serilog.ILogger _log = Logging.Get<MatchClientBallEvent>();

        public static void BallPositioningHitReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not MatchRoom matchRoom) return;
            if (e.handler is not MatchPlayer matchPlayer) return;
            if (e.message is not BallPositioningHit ballActionerHit) return;

            var ballComponent = matchRoom.Components.GetComponent<MatchBall>();
            if (ballComponent == null)
            {
                _log.Warning("[BallPositioningHitReceived] MatchBallComponent is null in MatchRoom {matchId}.", matchRoom.Id);
                return;
            }

            var movementComponent = matchRoom.Components.GetComponent<MatchMovement>();
            if (movementComponent == null)
            {
                _log.Warning("[BallPositioningHitReceived] MatchMovementComponent is null in MatchRoom {matchId}.", matchRoom.Id);
                return;
            }

            var matchPlayerInfo = matchRoom.MatchInformation.GetPlayer(matchPlayer.Id);
            var actionerPlayerInfo = matchRoom.MatchInformation.GetPlayer(matchRoom.MatchInformation.Actor.Actioner);

            if (actionerPlayerInfo == null || matchPlayerInfo == null)
            {
                _log.Warning("[BallPositioningHitReceived] Could not find player info in match.");
                return;
            }

            // Gonderen sadece actioner olsun.
            if (actionerPlayerInfo.PlayerId != matchPlayerInfo.PlayerId)
            {
                _log.Warning("[BallPositioningHitReceived] Player {playerId} is not the actioner {actionerId}.", matchPlayer.Id, actionerPlayerInfo.PlayerId);
                return;
            }

            double ballSpeed = ballComponent.BallMaxSpeed * ballActionerHit.Strength;
            double ballHitSafe = (ballComponent.BallCollisionRadius + movementComponent.MovementCollisionRadius) + 0.1;
            float ballVelocityZ;

            // [INFO]: Topu PositioningHit ile atmak zorundasın.
            // Client tarafı MatchState'i güncellemek için PositioningHit bekliyor.
            // Bu yüzden BallActionerHit, HitSub'ların kendi mesajını değil PositioningHit göndermek zorunda.
            // !!!!!!! HitSub eventlerinin topu atmasına izin verme. !!!!!!!!

            // (Temporary) HitSub:
            switch (ballActionerHit.HitSubType)
            {
                case HitSubType.Shoot:
                    ballVelocityZ = (float)(ballSpeed * 0.5);
                    break;
                case HitSubType.LongPass:
                    ballVelocityZ = (float)(ballSpeed);
                    break;
                case HitSubType.Pass: // Client PositioningHit için squad numarası göndermiyor?
                    ballVelocityZ = 0f;
                    break;
                case HitSubType.Invalid:
                default:
                    _log.Warning("[BallPositioningHitReceived] Invalid HitSubType received from player {playerId}.", matchPlayer.Id);
                    return;
            }

            actionerPlayerInfo.Direction = ballActionerHit.Direction;

            matchRoom.MatchInformation.BallPosition = new Vector3(
              (float)(actionerPlayerInfo.Position.X + actionerPlayerInfo.Direction.X * ballHitSafe),
              (float)(actionerPlayerInfo.Position.Y + actionerPlayerInfo.Direction.Y * ballHitSafe),
              (float)(ballComponent.BallBoundary.Z)
            );

            matchRoom.MatchInformation.BallVelocity = new Vector3(
              (float)(ballActionerHit.Direction.X * ballSpeed),
              (float)(ballActionerHit.Direction.Y * ballSpeed),
              (float)(ballVelocityZ)
            );

            // PositioningHit: 
            switch (matchRoom.MatchInformation.FieldPositioning)
            {
                case MatchFieldPositioning.Kickoff:
                    {
                        matchRoom.Players.SendMessage(new BallKickoffHit(
                            matchRoom.MatchInformation.BallVelocity,
                            AnimationType.ShootLeft
                        ));
                    }
                    break;
                default:
                    _log.Error($"[BallPositioningHitReceived] FieldPositioning {matchRoom.MatchInformation.FieldPositioning} not implemented.");
                    matchRoom.Dispose();
                    return;
            }

            matchRoom.Players.SendMessage(new Messages.Chat.ChatSystemMessage($"[BallPositioningHitReceived] {matchRoom.MatchInformation.FieldPositioning} by {actionerPlayerInfo.PlayerName}", Messages.Chat.ChatSystemMessageType.General));
            matchRoom.Players.SendMessage(new Messages.Chat.ChatSystemMessage($"[BallPositioningHitReceived] Strength: {ballActionerHit.Strength}, HitSubType: {ballActionerHit.HitSubType}, Direction: {ballActionerHit.Direction}", Messages.Chat.ChatSystemMessageType.General));

            matchRoom.MatchInformation.Actor.Actioner = -1;
            matchRoom.MatchInformation.MatchState = MatchStateType.Running;
            matchRoom.MatchInformation.FieldPositioning = MatchFieldPositioning.Running;

            _log.Information("[BallPositioningHitReceived] Actioner {playerId} hit the ball during {fieldPos}.", actionerPlayerInfo.PlayerId, matchRoom.MatchInformation.FieldPositioning);
        }

        public static void BallPassHitReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not MatchRoom matchRoom) return;
            if (e.handler is not MatchPlayer matchPlayer) return;
            if (e.message is not BallPassHit ballPassHit) return;

            var ballComponent = matchRoom.Components.GetComponent<MatchBall>();
            if (ballComponent == null)
            {
                _log.Warning("[BallPassHitReceived] MatchBallComponent is null in MatchRoom {matchId}.", matchRoom.Id);
                return;
            }

            var movementComponent = matchRoom.Components.GetComponent<MatchMovement>();
            if (movementComponent == null)
            {
                _log.Warning("[BallPassHitReceived] MatchMovementComponent is null in MatchRoom {matchId}.", matchRoom.Id);
                return;
            }

            var matchPlayerInfo = matchRoom.MatchInformation.GetPlayer(matchPlayer.Id);
            var actionerPlayerInfo = matchRoom.MatchInformation.GetPlayer(matchRoom.MatchInformation.Actor.Actioner);
            var passPlayerInfo = matchRoom.MatchInformation.GetPlayer(ballPassHit.SquadNumber);

            if (actionerPlayerInfo == null || matchPlayerInfo == null)
            {
                _log.Warning("[BallPassHitReceived] Could not find player info in match.");
                return;
            }

            // Gonderen sadece actioner olsun.
            if (actionerPlayerInfo.PlayerId != matchPlayerInfo.PlayerId)
            {
                _log.Warning("[BallPassHitReceived] Player {playerId} is not the actioner {actionerId}.", matchPlayer.Id, actionerPlayerInfo.PlayerId);
                return;
            }

            if (passPlayerInfo == null)
            {
                matchPlayer.SendMessage(new Messages.Chat.ChatSystemMessage(
                    $"[BallPassHitReceived] Could not find pass target player with SquadNumber {ballPassHit.SquadNumber}",
                    Messages.Chat.ChatSystemMessageType.General
                ));

                _log.Warning("[BallPassHitReceived] Could not find pass target player with SquadNumber {squadNumber}.", ballPassHit.SquadNumber);
                return;
            }

            // TODO: Implement pass logic here.
            _log.Information("[BallPassHitReceived] Actioner '{actionerName}' attempted a pass ball to {passPlayerName}.", actionerPlayerInfo.PlayerName, passPlayerInfo.PlayerName);
        }
    }
}
