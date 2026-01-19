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
            if (e.message is not BallPositioningMessage ballPositioningHit) return;

            var ballComponent = matchRoom.Components.GetComponent<MatchBallComponent>();
            if (ballComponent == null)
            {
                _log.Warning("[BallPositioningReceived] MatchBall is null in match {matchId}.", matchRoom.Id);
                return;
            }

            var movementComponent = matchRoom.Components.GetComponent<MatchMovementComponent>();
            if (movementComponent == null)
            {
                _log.Warning("[BallPositioningReceived] MatchMovement is null in match {matchId}.", matchRoom.Id);
                return;
            }

            var senderMatchInfo = matchRoom.MatchInformation.GetPlayer(matchPlayer.Id);
            var ballOwnerMatchInfo = matchRoom.MatchInformation.GetPlayer(matchRoom.MatchInformation.Actor.BallOwner);

            if (ballOwnerMatchInfo == null || senderMatchInfo == null)
            {
                _log.Warning("[BallPositioningReceived] Could not find player info in match.");
                return;
            }

            // Gonderen sadece ballOwner olsun.
            if (ballOwnerMatchInfo.PlayerId != senderMatchInfo.PlayerId)
            {
                _log.Warning("[BallPositioningReceived] Player {playerId} is not the BallOwner {ballOwnerId}.", matchPlayer.Id, ballOwnerMatchInfo.PlayerId);
                return;
            }

            // [INFO]: Topu PositioningHit ile atmak zorundasın.
            // Client tarafı MatchState'i güncellemek için PositioningHit bekliyor.
            // Bu yüzden BallPositioningHit, HitSub'ların kendi mesajını değil PositioningHit göndermek zorunda.
            // !!!!!!! HitSub eventlerinin topu atmasına izin verme. !!!!!!!!

            // HitSub:
            bool isHitSubDispatch;
            switch (ballPositioningHit.HitSubType)
            {
                case HitSubType.Shoot:
                    isHitSubDispatch = matchRoom.DispatchTo(matchPlayer, new BallShootMessage(ballPositioningHit.Strength, ballPositioningHit.Direction));
                    break;

                case HitSubType.LongPass: // Client PositioningHit icin squadNumber gondermiyor?
                    // TODO: BallOwner ondan onceki squadNumber'a pas veriyor. Ilerde squadNumber'lar pozisyonlara gore rastgele olursa sorun cikarir.
                    isHitSubDispatch = matchRoom.DispatchTo(matchPlayer, new BallLongPassMessage((sbyte)(matchRoom.MatchInformation.Actor.BallOwner - 1)));
                    break;

                case HitSubType.Pass: // Client PositioningHit icin squadNumber gondermiyor?
                    // TODO: BallOwner ondan onceki squadNumber'a pas veriyor. Ilerde squadNumber'lar pozisyonlara gore rastgele olursa sorun cikarir.
                    isHitSubDispatch = matchRoom.DispatchTo(matchPlayer, new BallPassMessage((sbyte)(matchRoom.MatchInformation.Actor.BallOwner - 1)));
                    break;

                case HitSubType.Invalid:
                default:
                    isHitSubDispatch = false;
                    _log.Warning("[BallPositioningReceived] Invalid HitSubType received from player {playerId}.", matchPlayer.Id);
                    return;
            }

            if (!isHitSubDispatch)
            {
                matchPlayer.SendMessage(new Messages.Chat.ChatSystemMessage("[BallPositioningReceived] Could not dispatch HitSub event.", Messages.Chat.ChatSystemMessageType.General));
                _log.Warning("[BallPositioningReceived] Could not dispatch HitSub event for player {playerId}.", matchPlayer.Id);
                return;
            }

            if (matchRoom.MatchInformation.BallVelocity == Vector3.Zero)
            {
                matchPlayer.SendMessage(new Messages.Chat.ChatSystemMessage("[BallPositioningReceived] BallVelocity is zero. HitSub possibly failed.", Messages.Chat.ChatSystemMessageType.General));
                _log.Warning("[BallPositioningReceived] BallVelocity is zero after HitSub event for player {playerId}.", matchPlayer.Id);
                return;
            }

            // PositioningHit: 
            switch (matchRoom.MatchInformation.FieldPositioning)
            {
                case MatchFieldPositioning.Kickoff:
                    matchRoom.Players.SendMessage(new BallKickoffHitMessage(
                        matchRoom.MatchInformation.BallVelocity,
                        AnimationType.ShootLeft
                    ));
                    break;

                default:
                    _log.Error($"[BallPositioningReceived] FieldPositioning {matchRoom.MatchInformation.FieldPositioning} not implemented.");
                    matchRoom.Dispose();
                    return;
            }

            matchRoom.Players.SendMessage(new Messages.Chat.ChatSystemMessage($"[BallPositioningReceived] {matchRoom.MatchInformation.FieldPositioning} by {ballOwnerMatchInfo.PlayerName}", Messages.Chat.ChatSystemMessageType.General));
            matchRoom.Players.SendMessage(new Messages.Chat.ChatSystemMessage($"[BallPositioningReceived] Strength: {ballPositioningHit.Strength}, HitSubType: {ballPositioningHit.HitSubType}, Direction: {ballPositioningHit.Direction}", Messages.Chat.ChatSystemMessageType.General));
            _log.Information("[BallPositioningReceived] Player {playerId} hit the ball during {fieldPos}.", ballOwnerMatchInfo.PlayerId, matchRoom.MatchInformation.FieldPositioning);

            matchRoom.MatchInformation.Actor.BallOwner = -1;
            matchRoom.MatchInformation.MatchState = MatchStateType.Running;
            matchRoom.MatchInformation.FieldPositioning = MatchFieldPositioning.Running;
        }

        public static void BallLongPassReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not MatchRoom matchRoom) return;
            if (e.handler is not MatchPlayer matchPlayer) return;
            if (e.message is not BallLongPassMessage ballLongPassMessage) return;

            var ballComponent = matchRoom.Components.GetComponent<MatchBallComponent>();
            if (ballComponent == null)
            {
                _log.Warning("[BallLongPassReceived] MatchBall is null in match {matchId}.", matchRoom.Id);
                return;
            }

            var movementComponent = matchRoom.Components.GetComponent<MatchMovementComponent>();
            if (movementComponent == null)
            {
                _log.Warning("[BallLongPassReceived] MatchMovement is null in match {matchId}.", matchRoom.Id);
                return;
            }

            var senderMatchInfo = matchRoom.MatchInformation.GetPlayer(matchPlayer.Id);
            var ballOwnerMatchInfo = matchRoom.MatchInformation.GetPlayer(matchRoom.MatchInformation.Actor.BallOwner);
            if (ballOwnerMatchInfo == null || senderMatchInfo == null)
            {
                _log.Warning("[BallLongPassReceived] Could not find player info in match.");
                return;
            }

            // Gonderen sadece ballOwner olsun.
            if (ballOwnerMatchInfo.PlayerId != senderMatchInfo.PlayerId)
            {
                _log.Warning("[BallLongPassReceived] Player {playerId} is not the BallOwner {ballOwnerId}.", matchPlayer.Id, ballOwnerMatchInfo.PlayerId);
                return;
            }

            var ballOwnerTeam = matchRoom.MatchInformation.GetSittingSide(ballOwnerMatchInfo.StadiumSitting);
            var passTargetMatchInfo = ballOwnerTeam?.FirstOrDefault(p => p.SquadNumber == ballLongPassMessage.SquadNumber);

            if (passTargetMatchInfo == null)
            {
                _log.Warning("[BallLongPassReceived] Could not find pass target with SquadNumber {squadNumber}.", ballLongPassMessage.SquadNumber);
                return;
            }

            double ballSpeed = ballComponent.BallMaxSpeed * 0.75; // Client, strength gondermiyor.
            double ballHitSafe = ballComponent.BallCollisionRadius + movementComponent.MovementCollisionRadius + (ballSpeed * 0.05);

            var passTargetDirection = Vector2.Normalize(passTargetMatchInfo.Position - ballOwnerMatchInfo.Position);
            ballOwnerMatchInfo.Direction = passTargetDirection;

            matchRoom.MatchInformation.BallPosition = new Vector3(
              (float)(ballOwnerMatchInfo.Position.X + ballOwnerMatchInfo.Direction.X * ballHitSafe),
              (float)(ballOwnerMatchInfo.Position.Y + ballOwnerMatchInfo.Direction.Y * ballHitSafe),
              (float)(ballComponent.BallBoundary.Z)
            );

            matchRoom.MatchInformation.BallVelocity = new Vector3(
              (float)(ballOwnerMatchInfo.Direction.X * ballSpeed),
              (float)(ballOwnerMatchInfo.Direction.Y * ballSpeed),
              (float)(ballSpeed * 1f) // TODO: Hardcoded Z velocity daha iyi bir yere tasinmali.
            );

            // INFO: Bu kontrolün sebebi BallPositioning (PositioningHit)
            if (matchRoom.MatchInformation.MatchState == MatchStateType.Running)
            {
                matchRoom.Players.SendMessage(new BallLongPassHitMessage(
                    (sbyte)matchPlayer.AuthInformation.Entry.ToSquad(),
                    ballOwnerMatchInfo.Position,
                    ballOwnerMatchInfo.Direction,
                    matchRoom.MatchInformation.BallVelocity,
                    0,
                    AnimationType.LongPassLeft
                ));

                // Client, topu attıktan sonra oyuncunun hareketini durduruyor.
                ballOwnerMatchInfo.IsMoving = false;
                ballOwnerMatchInfo.Velocity = Vector3.Zero;

                matchRoom.MatchInformation.Actor.BallOwner = -1;
                _log.Information("[BallLongPassReceived] Player {ballOwnerName} longPass the ball to {targetPlayerName}.", ballOwnerMatchInfo.PlayerName, passTargetMatchInfo.PlayerName);
            }
        }

        public static void BallPassReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not MatchRoom matchRoom) return;
            if (e.handler is not MatchPlayer matchPlayer) return;
            if (e.message is not BallPassMessage ballPassMessage) return;

            var ballComponent = matchRoom.Components.GetComponent<MatchBallComponent>();
            if (ballComponent == null)
            {
                _log.Warning("[BallPassReceived] MatchBall is null in match {matchId}.", matchRoom.Id);
                return;
            }

            var movementComponent = matchRoom.Components.GetComponent<MatchMovementComponent>();
            if (movementComponent == null)
            {
                _log.Warning("[BallPassReceived] MatchMovement is null in match {matchId}.", matchRoom.Id);
                return;
            }

            var senderMatchInfo = matchRoom.MatchInformation.GetPlayer(matchPlayer.Id);
            var ballOwnerMatchInfo = matchRoom.MatchInformation.GetPlayer(matchRoom.MatchInformation.Actor.BallOwner);
            if (ballOwnerMatchInfo == null || senderMatchInfo == null)
            {
                _log.Warning("[BallPassReceived] Could not find player info in match.");
                return;
            }

            // Gonderen sadece ballOwner olsun.
            if (ballOwnerMatchInfo.PlayerId != senderMatchInfo.PlayerId)
            {
                _log.Warning("[BallPassReceived] Player {playerId} is not the BallOwner {ballOwnerId}.", matchPlayer.Id, ballOwnerMatchInfo.PlayerId);
                return;
            }

            var ballOwnerTeam = matchRoom.MatchInformation.GetSittingSide(ballOwnerMatchInfo.StadiumSitting);
            var passTargetMatchInfo = ballOwnerTeam?.FirstOrDefault(p => p.SquadNumber == ballPassMessage.SquadNumber);

            if (passTargetMatchInfo == null)
            {
                _log.Warning("[BallPassReceived] Could not find pass target with SquadNumber {squadNumber}.", ballPassMessage.SquadNumber);
                return;
            }

            double ballSpeed = ballComponent.BallMaxSpeed * 0.75; // Client, strength gondermiyor.
            double ballHitSafe = ballComponent.BallCollisionRadius + movementComponent.MovementCollisionRadius + (ballSpeed * 0.05);

            var passTargetDirection = Vector2.Normalize(passTargetMatchInfo.Position - ballOwnerMatchInfo.Position);
            ballOwnerMatchInfo.Direction = passTargetDirection;

            matchRoom.MatchInformation.BallPosition = new Vector3(
              (float)(ballOwnerMatchInfo.Position.X + ballOwnerMatchInfo.Direction.X * ballHitSafe),
              (float)(ballOwnerMatchInfo.Position.Y + ballOwnerMatchInfo.Direction.Y * ballHitSafe),
              (float)(ballComponent.BallBoundary.Z)
            );

            matchRoom.MatchInformation.BallVelocity = new Vector3(
              (float)(ballOwnerMatchInfo.Direction.X * ballSpeed),
              (float)(ballOwnerMatchInfo.Direction.Y * ballSpeed),
              (float)(0f) // TODO: Hardcoded Z velocity daha iyi bir yere tasinmali.
            );

            // INFO: Bu kontrolün sebebi BallPositioning (PositioningHit)
            if (matchRoom.MatchInformation.MatchState == MatchStateType.Running)
            {
                matchRoom.Players.SendMessage(new BallPassHitMessage(
                    (sbyte)matchPlayer.AuthInformation.Entry.ToSquad(),
                    ballOwnerMatchInfo.Position,
                    ballOwnerMatchInfo.Direction,
                    matchRoom.MatchInformation.BallVelocity,
                    0,
                    AnimationType.PassLeft
                ));

                // Client, topu attıktan sonra oyuncunun hareketini durduruyor.
                ballOwnerMatchInfo.IsMoving = false;
                ballOwnerMatchInfo.Velocity = Vector3.Zero;

                matchRoom.MatchInformation.Actor.BallOwner = -1;
                _log.Information("[BallPassReceived] Player {ballOwnerName} pass the ball to {targetPlayerName}.", ballOwnerMatchInfo.PlayerName, passTargetMatchInfo.PlayerName);
            }
        }

        public static void BallShootReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not MatchRoom matchRoom) return;
            if (e.handler is not MatchPlayer matchPlayer) return;
            if (e.message is not BallShootMessage ballShoot) return;

            var ballComponent = matchRoom.Components.GetComponent<MatchBallComponent>();
            if (ballComponent == null)
            {
                _log.Warning("[BallShootReceived] MatchBall is null in match {matchId}.", matchRoom.Id);
                return;
            }

            var movementComponent = matchRoom.Components.GetComponent<MatchMovementComponent>();
            if (movementComponent == null)
            {
                _log.Warning("[BallShootReceived] MatchMovement is null in match {matchId}.", matchRoom.Id);
                return;
            }

            var senderMatchInfo = matchRoom.MatchInformation.GetPlayer(matchPlayer.Id);
            var ballOwnerMatchInfo = matchRoom.MatchInformation.GetPlayer(matchRoom.MatchInformation.Actor.BallOwner);

            if (ballOwnerMatchInfo == null || senderMatchInfo == null)
            {
                _log.Warning("[BallShootReceived] Could not find player info in match.");
                return;
            }

            // Gonderen sadece ballOwner olsun.
            if (ballOwnerMatchInfo.PlayerId != senderMatchInfo.PlayerId)
            {
                _log.Warning("[BallShootReceived] Player {playerId} is not the BallOwner {ballOwnerId}.", matchPlayer.Id, ballOwnerMatchInfo.PlayerId);
                return;
            }

            double ballSpeed = ballComponent.BallMaxSpeed * ballShoot.Strength;
            double ballHitSafe = ballComponent.BallCollisionRadius + movementComponent.MovementCollisionRadius + (ballSpeed * 0.05);

            ballOwnerMatchInfo.Direction = ballShoot.Direction;

            matchRoom.MatchInformation.BallPosition = new Vector3(
              (float)(ballOwnerMatchInfo.Position.X + ballOwnerMatchInfo.Direction.X * ballHitSafe),
              (float)(ballOwnerMatchInfo.Position.Y + ballOwnerMatchInfo.Direction.Y * ballHitSafe),
              (float)(ballComponent.BallBoundary.Z)
            );

            matchRoom.MatchInformation.BallVelocity = new Vector3(
              (float)(ballShoot.Direction.X * ballSpeed),
              (float)(ballShoot.Direction.Y * ballSpeed),
              (float)(ballSpeed * 0.5) // TODO: Hardcoded Z velocity daha iyi bir yere tasinmali.
            );

            // INFO: Bu kontrolün sebebi BallPositioning (PositioningHit)
            if (matchRoom.MatchInformation.MatchState == MatchStateType.Running)
            {
                matchRoom.Players.SendMessage(new BallShootHitMessage(
                    (sbyte)matchPlayer.AuthInformation.Entry.ToSquad(),
                    ballOwnerMatchInfo.Position,
                    ballOwnerMatchInfo.Direction,
                    matchRoom.MatchInformation.BallVelocity,
                    0,
                    AnimationType.ShootLeft
                ));

                // Client, topu attıktan sonra oyuncunun hareketini durduruyor.
                ballOwnerMatchInfo.IsMoving = false;
                ballOwnerMatchInfo.Velocity = Vector3.Zero;

                matchRoom.MatchInformation.Actor.BallOwner = -1;
                _log.Information("[BallShootReceived] Player {ballOwnerName} shoot the ball.", ballOwnerMatchInfo.PlayerName);
            }
        }
    }
}
