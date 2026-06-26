using Football.Common;
using Football.GameServer.Match;
using Football.GameServer.MatchComponents;
using Football.GameServer.Messages;
using Football.GameServer.Messages.Ball;
using Football.GameServer.Messages.Chat;
using Football.GameServer.Messages.Match;
using Football.Network.Messaging;
using Football.Serialization.GameServer;
using System.Numerics;

namespace Football.GameServer.MatchEvents
{
    public class MatchClientBallEvent
    {
        private static readonly Serilog.ILogger _log = LogFactory.GetContextForType<MatchClientBallEvent>();

        public static void BallPositioningReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not MatchRoom matchRoom) return;
            if (e.Handler is not MatchUser matchPlayer) return;
            if (e.Message is not BallPositioningRxMessage ballPositioningHit) return;

            var ballComponent = matchRoom.GetComponent<MatchBallComponent>();
            if (ballComponent == null)
            {
                _log.Warning("[BallPositioningReceived] MatchBall is null in match {matchId}.", matchRoom.Id);
                return;
            }

            var movementComponent = matchRoom.GetComponent<MatchMovementComponent>();
            if (movementComponent == null)
            {
                _log.Warning("[BallPositioningReceived] MatchMovement is null in match {matchId}.", matchRoom.Id);
                return;
            }

            var senderMatchInfo = matchRoom.MatchInformation.GetPlayer(matchPlayer.Id);
            var ballOwnerMatchInfo = matchRoom.MatchInformation.GetPlayer(matchRoom.MatchInformation.BallOwner);

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
                    isHitSubDispatch = matchRoom.Communication.DispatchToMessageEvent(
                        new MessageEventArgs(
                            matchPlayer,
                            new BallShootRxMessage(ballPositioningHit.HitStrength, ballPositioningHit.HitDirection)
                        )
                    );
                    break;

                case HitSubType.LongPass: // Client PositioningHit icin squadNumber gondermiyor?
                    // TODO: BallOwner ondan onceki squadNumber'a pas veriyor. SquadNumber'lar pozisyonlara gore rastgele olursa sorun cikarir.
                    isHitSubDispatch = matchRoom.Communication.DispatchToMessageEvent(
                        new MessageEventArgs(
                            matchPlayer,
                            new BallPassLongRxMessage((sbyte)(matchRoom.MatchInformation.BallOwner - 1))
                        )
                    );
                    break;

                case HitSubType.Pass: // Client PositioningHit icin squadNumber gondermiyor?
                    // TODO: BallOwner ondan onceki squadNumber'a pas veriyor. SquadNumber'lar pozisyonlara gore rastgele olursa sorun cikarir.
                    isHitSubDispatch = matchRoom.Communication.DispatchToMessageEvent(
                        new MessageEventArgs(
                            matchPlayer,
                            new BallPassNormalRxMessage((sbyte)(matchRoom.MatchInformation.BallOwner - 1))
                        )
                    );
                    break;

                case HitSubType.Invalid:
                default:
                    isHitSubDispatch = false;
                    _log.Warning("[BallPositioningReceived] Invalid HitSubType received from player {playerId}.", matchPlayer.Id);
                    return;
            }

            if (!isHitSubDispatch)
            {
                matchPlayer.SendMessage(new ChatSystemTextTxMessage("[BallPositioningReceived] Could not dispatch HitSub event.", ChatSystemMessageType.General));
                _log.Warning("[BallPositioningReceived] Could not dispatch HitSub event for player {playerId}.", matchPlayer.Id);
                return;
            }

            if (matchRoom.MatchInformation.BallVelocity == Vector3.Zero)
            {
                matchPlayer.SendMessage(new ChatSystemTextTxMessage("[BallPositioningReceived] BallVelocity is zero. HitSub possibly failed.", ChatSystemMessageType.General));
                _log.Warning("[BallPositioningReceived] BallVelocity is zero after HitSub event for player {playerId}.", matchPlayer.Id);
                return;
            }

            // PositioningHit: 
            switch (matchRoom.MatchInformation.FieldPositioning)
            {
                case MatchFieldPositioning.Kickoff:
                    matchRoom.Players.SendMessage(new BallKickoffTxMessage(
                        matchRoom.MatchInformation.BallVelocity,
                        AnimationType.ShootLeft
                    ));
                    break;

                default:
                    _log.Error($"[BallPositioningReceived] FieldPositioning {matchRoom.MatchInformation.FieldPositioning} not implemented.");
                    matchRoom.Dispose();
                    return;
            }

            matchRoom.Players.SendMessage(new ChatSystemTextTxMessage($"[BallPositioningReceived] {matchRoom.MatchInformation.FieldPositioning} by {ballOwnerMatchInfo.PlayerName}", ChatSystemMessageType.General));
            matchRoom.Players.SendMessage(new ChatSystemTextTxMessage($"[BallPositioningReceived] Strength: {ballPositioningHit.HitStrength}, HitSubType: {ballPositioningHit.HitSubType}, Direction: {ballPositioningHit.HitDirection}", ChatSystemMessageType.General));
            _log.Information("[BallPositioningReceived] Player {playerId} hit the ball during {fieldPos}.", ballOwnerMatchInfo.PlayerId, matchRoom.MatchInformation.FieldPositioning);

            matchRoom.MatchInformation.BallOwner = -1;
            matchRoom.MatchInformation.MatchState = MatchState.Running;
            matchRoom.MatchInformation.FieldPositioning = MatchFieldPositioning.Running;
        }

        public static void BallInterceptReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not MatchRoom matchRoom) return;
            if (e.Handler is not MatchUser matchPlayer) return;
            if (e.Message is not BallInterceptRxMessage ballInterceptMessage) return;

            var ballComponent = matchRoom.GetComponent<MatchBallComponent>();
            if (ballComponent == null)
            {
                _log.Warning("[BallInterceptReceived] MatchBall is null in match {matchId}.", matchRoom.Id);
                return;
            }

            var movementComponent = matchRoom.GetComponent<MatchMovementComponent>();
            if (movementComponent == null)
            {
                _log.Warning("[BallInterceptReceived] MatchMovement is null in match {matchId}.", matchRoom.Id);
                return;
            }

            var senderMatchInfo = matchRoom.MatchInformation.GetPlayer(matchPlayer.Id);
            var ballOwnerMatchInfo = matchRoom.MatchInformation.GetPlayer(matchRoom.MatchInformation.BallOwner);
            if (ballOwnerMatchInfo == null || senderMatchInfo == null)
            {
                _log.Warning("[BallInterceptReceived] Could not find player info in match.");
                return;
            }

            throw new NotImplementedException("[BallInterceptReceived] This event not implemented yet.");
        }

        public static void BallTackleReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not MatchRoom matchRoom) return;
            if (e.Handler is not MatchUser matchPlayer) return;
            if (e.Message is not BallTackleRxMessage ballTackleMessage) return;

            // INFO: Client, top 500 den yakinsa tackle degilse intercept gonderiyor

            var ballComponent = matchRoom.GetComponent<MatchBallComponent>();
            if (ballComponent == null)
            {
                _log.Warning("[BallTackleReceived] MatchBall is null in match {matchId}.", matchRoom.Id);
                return;
            }

            var movementComponent = matchRoom.GetComponent<MatchMovementComponent>();
            if (movementComponent == null)
            {
                _log.Warning("[BallTackleReceived] MatchMovement is null in match {matchId}.", matchRoom.Id);
                return;
            }

            var senderMatchInfo = matchRoom.MatchInformation.GetPlayer(matchPlayer.Id);
            var ballOwnerMatchInfo = matchRoom.MatchInformation.GetPlayer(matchRoom.MatchInformation.BallOwner);
            if (ballOwnerMatchInfo == null || senderMatchInfo == null)
            {
                _log.Warning("[BallTackleReceived] Could not find player info in match.");
                return;
            }

            throw new NotImplementedException("[BallTackleReceived] This event not implemented yet.");
        }

        public static void BallPassLongReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not MatchRoom matchRoom) return;
            if (e.Handler is not MatchUser matchPlayer) return;
            if (e.Message is not BallPassLongRxMessage ballLongPassMessage) return;

            var ballComponent = matchRoom.GetComponent<MatchBallComponent>();
            if (ballComponent == null)
            {
                _log.Warning("[BallPassLongReceived] MatchBall is null in match {matchId}.", matchRoom.Id);
                return;
            }

            var movementComponent = matchRoom.GetComponent<MatchMovementComponent>();
            if (movementComponent == null)
            {
                _log.Warning("[BallPassLongReceived] MatchMovement is null in match {matchId}.", matchRoom.Id);
                return;
            }

            var senderMatchInfo = matchRoom.MatchInformation.GetPlayer(matchPlayer.Id);
            var ballOwnerMatchInfo = matchRoom.MatchInformation.GetPlayer(matchRoom.MatchInformation.BallOwner);
            if (ballOwnerMatchInfo == null || senderMatchInfo == null)
            {
                _log.Warning("[BallPassLongReceived] Could not find player info in match.");
                return;
            }

            // Gonderen sadece ballOwner olsun.
            if (ballOwnerMatchInfo.PlayerId != senderMatchInfo.PlayerId)
            {
                _log.Warning("[BallPassLongReceived] Player {playerId} is not the BallOwner {ballOwnerId}.", matchPlayer.Id, ballOwnerMatchInfo.PlayerId);
                return;
            }

            var ballOwnerTeam = matchRoom.MatchInformation.GetSittingSide(ballOwnerMatchInfo.StadiumSitting);
            var passTargetMatchInfo = ballOwnerTeam?.FirstOrDefault(p => p.SquadNumber == ballLongPassMessage.SquadNumber);

            if (passTargetMatchInfo == null)
            {
                _log.Warning("[BallPassLongReceived] Could not find pass target with SquadNumber {squadNumber}.", ballLongPassMessage.SquadNumber);
                return;
            }

            double ballSpeed = ballComponent.BallMaxSpeed * 0.75; // Client, strength gondermiyor.
            double ballHitSafe = ballComponent.BallCollisionRadius + movementComponent.MovementCollisionRadius + ballSpeed * 0.05;

            var passTargetDirection = Vector2.Normalize(passTargetMatchInfo.Position - ballOwnerMatchInfo.Position);
            ballOwnerMatchInfo.Direction = passTargetDirection;

            matchRoom.MatchInformation.BallPosition = new Vector3(
              (float)(ballOwnerMatchInfo.Position.X + ballOwnerMatchInfo.Direction.X * ballHitSafe),
              (float)(ballOwnerMatchInfo.Position.Y + ballOwnerMatchInfo.Direction.Y * ballHitSafe),
              ballComponent.BallBoundary.Z
            );

            matchRoom.MatchInformation.BallVelocity = new Vector3(
              (float)(ballOwnerMatchInfo.Direction.X * ballSpeed),
              (float)(ballOwnerMatchInfo.Direction.Y * ballSpeed),
              (float)(ballSpeed * 1f) // TODO: Hardcoded Z velocity daha iyi bir yere tasinmali.
            );

            // INFO: Bu kontrolün sebebi BallPositioning (PositioningHit)
            if (matchRoom.MatchInformation.MatchState == MatchState.Running)
            {
                matchRoom.Players.SendMessage(new BallPassLongTxMessage(
                    ballOwnerMatchInfo.GetAbsoluteSquadNumber(),
                    ballOwnerMatchInfo.Position,
                    ballOwnerMatchInfo.Direction,
                    matchRoom.MatchInformation.BallVelocity,
                    0,
                    AnimationType.LongPassLeft
                ));

                // Client, topu attıktan sonra oyuncunun hareketini durduruyor.
                ballOwnerMatchInfo.IsMoving = false;
                ballOwnerMatchInfo.Velocity = Vector3.Zero;

                matchRoom.MatchInformation.BallOwner = -1;
                _log.Information("[BallPassLongReceived] Player {ballOwnerName} longPass the ball to {targetPlayerName}.", ballOwnerMatchInfo.PlayerName, passTargetMatchInfo.PlayerName);
            }
        }

        public static void BallPassNormalReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not MatchRoom matchRoom) return;
            if (e.Handler is not MatchUser matchPlayer) return;
            if (e.Message is not BallPassNormalRxMessage ballPassMessage) return;

            var ballComponent = matchRoom.GetComponent<MatchBallComponent>();
            if (ballComponent == null)
            {
                _log.Warning("[BallPassNormalReceived] MatchBall is null in match {matchId}.", matchRoom.Id);
                return;
            }

            var movementComponent = matchRoom.GetComponent<MatchMovementComponent>();
            if (movementComponent == null)
            {
                _log.Warning("[BallPassNormalReceived] MatchMovement is null in match {matchId}.", matchRoom.Id);
                return;
            }

            var senderMatchInfo = matchRoom.MatchInformation.GetPlayer(matchPlayer.Id);
            var ballOwnerMatchInfo = matchRoom.MatchInformation.GetPlayer(matchRoom.MatchInformation.BallOwner);
            if (ballOwnerMatchInfo == null || senderMatchInfo == null)
            {
                _log.Warning("[BallPassNormalReceived] Could not find player info in match.");
                return;
            }

            // Gonderen sadece ballOwner olsun.
            if (ballOwnerMatchInfo.PlayerId != senderMatchInfo.PlayerId)
            {
                _log.Warning("[BallPassNormalReceived] Player {playerId} is not the BallOwner {ballOwnerId}.", matchPlayer.Id, ballOwnerMatchInfo.PlayerId);
                return;
            }

            var ballOwnerTeam = matchRoom.MatchInformation.GetSittingSide(ballOwnerMatchInfo.StadiumSitting);
            var passTargetMatchInfo = ballOwnerTeam?.FirstOrDefault(p => p.SquadNumber == ballPassMessage.SquadNumber);

            if (passTargetMatchInfo == null)
            {
                _log.Warning("[BallPassNormalReceived] Could not find pass target with SquadNumber {squadNumber}.", ballPassMessage.SquadNumber);
                return;
            }

            double ballSpeed = ballComponent.BallMaxSpeed * 0.75; // Client, strength gondermiyor.
            double ballHitSafe = ballComponent.BallCollisionRadius + movementComponent.MovementCollisionRadius + ballSpeed * 0.05;

            var passTargetDirection = Vector2.Normalize(passTargetMatchInfo.Position - ballOwnerMatchInfo.Position);
            ballOwnerMatchInfo.Direction = passTargetDirection;

            matchRoom.MatchInformation.BallPosition = new Vector3(
              (float)(ballOwnerMatchInfo.Position.X + ballOwnerMatchInfo.Direction.X * ballHitSafe),
              (float)(ballOwnerMatchInfo.Position.Y + ballOwnerMatchInfo.Direction.Y * ballHitSafe),
              ballComponent.BallBoundary.Z
            );

            matchRoom.MatchInformation.BallVelocity = new Vector3(
              (float)(ballOwnerMatchInfo.Direction.X * ballSpeed),
              (float)(ballOwnerMatchInfo.Direction.Y * ballSpeed),
              (float)0f // TODO: Hardcoded Z velocity daha iyi bir yere tasinmali.
            );

            // INFO: Bu kontrolün sebebi BallPositioning (PositioningHit)
            if (matchRoom.MatchInformation.MatchState == MatchState.Running)
            {
                matchRoom.Players.SendMessage(new BallPassNormalTxMessage(
                    ballOwnerMatchInfo.GetAbsoluteSquadNumber(),
                    ballOwnerMatchInfo.Position,
                    ballOwnerMatchInfo.Direction,
                    matchRoom.MatchInformation.BallVelocity,
                    0,
                    AnimationType.PassLeft
                ));

                // Client, topu attıktan sonra oyuncunun hareketini durduruyor.
                ballOwnerMatchInfo.IsMoving = false;
                ballOwnerMatchInfo.Velocity = Vector3.Zero;

                matchRoom.MatchInformation.BallOwner = -1;
                _log.Information("[BallPassNormalReceived] Player {ballOwnerName} pass the ball to {targetPlayerName}.", ballOwnerMatchInfo.PlayerName, passTargetMatchInfo.PlayerName);
            }
        }

        public static void BallPassThroughReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not MatchRoom matchRoom) return;
            if (e.Handler is not MatchUser matchPlayer) return;
            if (e.Message is not BallPassThroughRxMessage ballPassMessage) return;

            var ballComponent = matchRoom.GetComponent<MatchBallComponent>();
            if (ballComponent == null)
            {
                _log.Warning("[BallPassThroughReceived] MatchBall is null in match {matchId}.", matchRoom.Id);
                return;
            }

            var movementComponent = matchRoom.GetComponent<MatchMovementComponent>();
            if (movementComponent == null)
            {
                _log.Warning("[BallPassThroughReceived] MatchMovement is null in match {matchId}.", matchRoom.Id);
                return;
            }

            throw new NotImplementedException("[BallPassThroughReceived] This event not implemented yet.");
        }

        public static void BallShootReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not MatchRoom matchRoom) return;
            if (e.Handler is not MatchUser matchPlayer) return;
            if (e.Message is not BallShootRxMessage ballShoot) return;

            var ballComponent = matchRoom.GetComponent<MatchBallComponent>();
            if (ballComponent == null)
            {
                _log.Warning("[BallShootReceived] MatchBall is null in match {matchId}.", matchRoom.Id);
                return;
            }

            var movementComponent = matchRoom.GetComponent<MatchMovementComponent>();
            if (movementComponent == null)
            {
                _log.Warning("[BallShootReceived] MatchMovement is null in match {matchId}.", matchRoom.Id);
                return;
            }

            var senderMatchInfo = matchRoom.MatchInformation.GetPlayer(matchPlayer.Id);
            var ballOwnerMatchInfo = matchRoom.MatchInformation.GetPlayer(matchRoom.MatchInformation.BallOwner);

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

            double ballSpeed = ballComponent.BallMaxSpeed * ballShoot.ShootStrength;
            double ballHitSafe = ballComponent.BallCollisionRadius + movementComponent.MovementCollisionRadius + ballSpeed * 0.05;

            ballOwnerMatchInfo.Direction = ballShoot.ShootDirection;

            matchRoom.MatchInformation.BallPosition = new Vector3(
              (float)(ballOwnerMatchInfo.Position.X + ballOwnerMatchInfo.Direction.X * ballHitSafe),
              (float)(ballOwnerMatchInfo.Position.Y + ballOwnerMatchInfo.Direction.Y * ballHitSafe),
              ballComponent.BallBoundary.Z
            );

            matchRoom.MatchInformation.BallVelocity = new Vector3(
              (float)(ballShoot.ShootDirection.X * ballSpeed),
              (float)(ballShoot.ShootDirection.Y * ballSpeed),
              (float)(ballSpeed * 0.5) // TODO: Hardcoded Z velocity daha iyi bir yere tasinmali.
            );

            // INFO: Bu kontrolün sebebi BallPositioning (PositioningHit)
            if (matchRoom.MatchInformation.MatchState == MatchState.Running)
            {
                matchRoom.Players.SendMessage(new BallShootTxMessage(
                    ballOwnerMatchInfo.GetAbsoluteSquadNumber(),
                    ballOwnerMatchInfo.Position,
                    ballOwnerMatchInfo.Direction,
                    matchRoom.MatchInformation.BallVelocity,
                    0,
                    AnimationType.ShootLeft
                ));

                // Client, topu attıktan sonra oyuncunun hareketini durduruyor.
                ballOwnerMatchInfo.IsMoving = false;
                ballOwnerMatchInfo.Velocity = Vector3.Zero;

                matchRoom.MatchInformation.BallOwner = -1;
                _log.Information("[BallShootReceived] Player {ballOwnerName} shoot the ball.", ballOwnerMatchInfo.PlayerName);
            }
        }
    }
}
