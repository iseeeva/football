using Sobee.Common;
using Sobee.Messaging;
using Sobee.Serialization.GameServer;
using Sobee.TestServer.Match;
using Sobee.TestServer.Messages.Ball;

namespace Sobee.TestServer.GameEvents
{
    public class MatchBallEvent
    {
        private static readonly Serilog.ILogger _log = Logging.Get<MatchBallEvent>();

        public static async void ActionerHitReceived(object? sender, MessageEventArgs e)
        {
            if (sender is not MatchRoom matchRoom) return;
            if (e.handler is not MatchPlayer matchPlayer) return;
            if (e.message is not BallActionerHit actionerHit) return;

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

            matchPlayer.SendMessage(new Messages.Chat.ChatSystemMessage($"[ActionerHitReceived] Strength: {actionerHit.Strength}, HitSubType: {actionerHit.HitSubType}, Direction: {actionerHit.Direction}", Messages.Chat.ChatSystemMessageType.General));
            // TODO: Hala islenmedi.

            switch (matchRoom.MatchInformation.FieldPositioning)
            {
                case MatchFieldPositioning.Kickoff:
                    {
                        //matchRoom.Players.SendMessage(new BallKickoffHit(
                        //    matchRoom.MatchInformation.BallVelocity,
                        //    Messages.AnimationType.ShootLeft
                        //));

                        matchRoom.Players.SendMessage(new Messages.Chat.ChatSystemMessage($"[ActionerHitReceived] Kickoff by {actionerPlayerInfo.PlayerName}", Messages.Chat.ChatSystemMessageType.General));
                        _log.Information("[ActionerHitReceived] Actioner {playerId} hit the ball during Kickoff.", actionerPlayerInfo.PlayerId);
                    }
                    break;
                default:
                    return;
            }

            matchRoom.MatchInformation.Actor.Actioner = -1;
            matchRoom.MatchInformation.FieldPositioning = MatchFieldPositioning.Running;
        }
    }
}
