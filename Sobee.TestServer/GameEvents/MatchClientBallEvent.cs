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

        public static void BallPositioningReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not MatchRoom matchRoom) return;
            if (e.handler is not MatchPlayer matchPlayer) return;
            if (e.message is not BallPositioning ballActionerHit) return;

            var ballComponent = matchRoom.Components.GetComponent<MatchBall>();
            if (ballComponent == null)
            {
                _log.Warning("[BallPositioningReceived] MatchBallComponent is null in MatchRoom {matchId}.", matchRoom.Id);
                return;
            }

            var movementComponent = matchRoom.Components.GetComponent<MatchMovement>();
            if (movementComponent == null)
            {
                _log.Warning("[BallPositioningReceived] MatchMovementComponent is null in MatchRoom {matchId}.", matchRoom.Id);
                return;
            }

            var matchPlayerInfo = matchRoom.MatchInformation.GetPlayer(matchPlayer.Id);
            var actionerPlayerInfo = matchRoom.MatchInformation.GetPlayer(matchRoom.MatchInformation.Actor.BallOwner);

            if (actionerPlayerInfo == null || matchPlayerInfo == null)
            {
                _log.Warning("[BallPositioningReceived] Could not find player info in match.");
                return;
            }

            // Gonderen sadece actioner olsun.
            if (actionerPlayerInfo.PlayerId != matchPlayerInfo.PlayerId)
            {
                _log.Warning("[BallPositioningReceived] Player {playerId} is not the actioner {actionerId}.", matchPlayer.Id, actionerPlayerInfo.PlayerId);
                return;
            }

            // [INFO]: Topu PositioningHit ile atmak zorundasın.
            // Client tarafı MatchState'i güncellemek için PositioningHit bekliyor.
            // Bu yüzden BallActionerHit, HitSub'ların kendi mesajını değil PositioningHit göndermek zorunda.
            // !!!!!!! HitSub eventlerinin topu atmasına izin verme. !!!!!!!!

            // HitSub:
            bool isHitSubDispatch;
            switch (ballActionerHit.HitSubType)
            {
                case HitSubType.Shoot:
                    isHitSubDispatch = matchRoom.DispatchTo(matchPlayer, new BallShoot(ballActionerHit.Strength, ballActionerHit.Direction));
                    break;

                case HitSubType.Pass: // Client PositioningHit için squad numarası göndermiyor?
                case HitSubType.LongPass:
                    isHitSubDispatch = false;
                    break;

                case HitSubType.Invalid:
                default:
                    _log.Warning("[BallPositioningReceived] Invalid HitSubType received from player {playerId}.", matchPlayer.Id);
                    return;
            }

            if (!isHitSubDispatch)
            {
                matchPlayer.SendMessage(new Messages.Chat.ChatSystemMessage("[BallPositioningReceived] Could not dispatch HitSub event.", Messages.Chat.ChatSystemMessageType.General));
                _log.Warning("[BallPositioningReceived] Could not dispatch HitSub event for player {playerId}.", matchPlayer.Id);
                return;
            }

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
                    _log.Error($"[BallPositioningReceived] FieldPositioning {matchRoom.MatchInformation.FieldPositioning} not implemented.");
                    matchRoom.Dispose();
                    return;
            }

            matchRoom.Players.SendMessage(new Messages.Chat.ChatSystemMessage($"[BallPositioningReceived] {matchRoom.MatchInformation.FieldPositioning} by {actionerPlayerInfo.PlayerName}", Messages.Chat.ChatSystemMessageType.General));
            matchRoom.Players.SendMessage(new Messages.Chat.ChatSystemMessage($"[BallPositioningReceived] Strength: {ballActionerHit.Strength}, HitSubType: {ballActionerHit.HitSubType}, Direction: {ballActionerHit.Direction}", Messages.Chat.ChatSystemMessageType.General));
            _log.Information("[BallPositioningReceived] Player {playerId} hit the ball during {fieldPos}.", actionerPlayerInfo.PlayerId, matchRoom.MatchInformation.FieldPositioning);

            matchRoom.MatchInformation.Actor.BallOwner = -1;
            matchRoom.MatchInformation.MatchState = MatchStateType.Running;
            matchRoom.MatchInformation.FieldPositioning = MatchFieldPositioning.Running;
        }

        //public static void BallPassReceived(object? sender, MessageEventArgs e)
        //{
        //    if (sender is not MatchRoom matchRoom) return;
        //    if (e.handler is not MatchPlayer matchPlayer) return;
        //    if (e.message is not BallPass ballPass) return;

        //    var ballComponent = matchRoom.Components.GetComponent<MatchBall>();
        //    if (ballComponent == null)
        //    {
        //        _log.Warning("[BallPassReceived] MatchBallComponent is null in MatchRoom {matchId}.", matchRoom.Id);
        //        return;
        //    }

        //    var movementComponent = matchRoom.Components.GetComponent<MatchMovement>();
        //    if (movementComponent == null)
        //    {
        //        _log.Warning("[BallPassReceived] MatchMovementComponent is null in MatchRoom {matchId}.", matchRoom.Id);
        //        return;
        //    }

        //    var matchPlayerInfo = matchRoom.MatchInformation.GetPlayer(matchPlayer.Id);
        //    var actionerPlayerInfo = matchRoom.MatchInformation.GetPlayer(matchRoom.MatchInformation.Actor.Actioner);
        //    var passPlayerInfo = matchRoom.MatchInformation.GetPlayer(ballPass.SquadNumber);

        //    if (actionerPlayerInfo == null || matchPlayerInfo == null)
        //    {
        //        _log.Warning("[BallPassReceived] Could not find player info in match.");
        //        return;
        //    }

        //    // Gonderen sadece actioner olsun.
        //    if (actionerPlayerInfo.PlayerId != matchPlayerInfo.PlayerId)
        //    {
        //        _log.Warning("[BallPassReceived] Player {playerId} is not the actioner {actionerId}.", matchPlayer.Id, actionerPlayerInfo.PlayerId);
        //        return;
        //    }

        //    if (passPlayerInfo == null)
        //    {
        //        matchPlayer.SendMessage(new Messages.Chat.ChatSystemMessage(
        //            $"[BallPassReceived] Could not find pass target player with SquadNumber {ballPass.SquadNumber}",
        //            Messages.Chat.ChatSystemMessageType.General
        //        ));

        //        _log.Warning("[BallPassReceived] Could not find pass target player with SquadNumber {squadNumber}.", ballPass.SquadNumber);
        //        return;
        //    }

        //    // TODO: Implement pass logic here.
        //    _log.Information("[BallPassReceived] Player '{actionerName}' attempted a pass ball to {passPlayerName}.", actionerPlayerInfo.PlayerName, passPlayerInfo.PlayerName);
        //}

        public static void BallShootReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not MatchRoom matchRoom) return;
            if (e.handler is not MatchPlayer matchPlayer) return;
            if (e.message is not BallShoot ballShoot) return;

            var ballComponent = matchRoom.Components.GetComponent<MatchBall>();
            if (ballComponent == null)
            {
                _log.Warning("[BallShootReceived] MatchBallComponent is null in MatchRoom {matchId}.", matchRoom.Id);
                return;
            }

            var movementComponent = matchRoom.Components.GetComponent<MatchMovement>();
            if (movementComponent == null)
            {
                _log.Warning("[BallShootReceived] MatchMovementComponent is null in MatchRoom {matchId}.", matchRoom.Id);
                return;
            }

            var matchPlayerInfo = matchRoom.MatchInformation.GetPlayer(matchPlayer.Id);
            var actionerPlayerInfo = matchRoom.MatchInformation.GetPlayer(matchRoom.MatchInformation.Actor.BallOwner);

            if (actionerPlayerInfo == null || matchPlayerInfo == null)
            {
                _log.Warning("[BallShootReceived] Could not find player info in match.");
                return;
            }

            // Gonderen sadece actioner olsun.
            if (actionerPlayerInfo.PlayerId != matchPlayerInfo.PlayerId)
            {
                _log.Warning("[BallShootReceived] Player {playerId} is not the actioner {actionerId}.", matchPlayer.Id, actionerPlayerInfo.PlayerId);
                return;
            }

            double ballSpeed = ballComponent.BallMaxSpeed * ballShoot.Strength;
            double ballHitSafe = (ballComponent.BallCollisionRadius + movementComponent.MovementCollisionRadius) + 0.1;

            actionerPlayerInfo.Direction = ballShoot.Direction;

            matchRoom.MatchInformation.BallPosition = new Vector3(
              (float)(actionerPlayerInfo.Position.X + actionerPlayerInfo.Direction.X * ballHitSafe),
              (float)(actionerPlayerInfo.Position.Y + actionerPlayerInfo.Direction.Y * ballHitSafe),
              (float)(ballComponent.BallBoundary.Z)
            );

            matchRoom.MatchInformation.BallVelocity = new Vector3(
              (float)(ballShoot.Direction.X * ballSpeed),
              (float)(ballShoot.Direction.Y * ballSpeed),
              (float)(ballSpeed * 0.5) // TODO: Put somewhere else hardcoded Z velocity
            );

            // INFO: Bu kontrolün sebebi BallPositioning (PositioningHit)
            if (matchRoom.MatchInformation.MatchState == MatchStateType.Running)
            {
                matchRoom.Players.SendMessage(new BallShootHit(
                    (sbyte)matchPlayer.AuthInformation.Entry.ToSquad(),
                    actionerPlayerInfo.Position,
                    actionerPlayerInfo.Direction,
                    matchRoom.MatchInformation.BallVelocity,
                    0,
                    AnimationType.ShootLeft
                ));

                matchRoom.MatchInformation.Actor.BallOwner = -1;
                _log.Information("[BallShootReceived] Player '{actionerName}' attempted a shoot.", actionerPlayerInfo.PlayerName);
            }
        }
    }
}
