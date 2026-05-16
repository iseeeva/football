using Sobee.Network;
using Sobee.TestServer.Auth;
using Sobee.TestServer.MatchHelpers;
using Sobee.TestServer.Messages.Player;
using System.Diagnostics.CodeAnalysis;

namespace Sobee.TestServer.Match
{
    public class MatchPlayerManager<TRoom, TPlayer> : SessionManager<TRoom, TPlayer>
        where TRoom : MatchRoom
        where TPlayer : MatchPlayer
    {
        public event Action<TRoom, TPlayer>? PlayerJoinEvent;
        public event Action<TRoom, TPlayer, PlayerMatchInformationMessage>? PlayerLeaveEvent;

        public MatchPlayerManager()
        {
        }

        #region Players
        public override bool TryAdd(TPlayer session)
        {
            throw new NotImplementedException("TryAdd is not implemented. Use TryCreate instead.");
            //_log.Warning("TryAdd is not implemented. Use TryCreate instead.");
            //return false;
        }

        public virtual bool TryCreate(AuthUser authUser)
        {
            if (Owner == null)
            {
                _log.Error("Owner is null. TryCreate method aborted.");
                return false;
            }

            if (!MatchHelper.TryGeneratePlayer(Owner, authUser, out var matchPlayer, out var matchPlayerInfo))
                return false;

            if (matchPlayer is not TPlayer typedPlayer)
            {
                _log.Error("Generated player is not of type {type}. TryCreate aborted.", typeof(TPlayer).Name);
                matchPlayer.Disconnect();
                return false;
            }

            var assignedSittingSide = Owner.MatchInformation.GetSittingSide(matchPlayerInfo.StadiumSitting);

            if (assignedSittingSide == null)
            {
                _log.Warning(
                    "Player ({playerId}) rejected: assigned team not found.",
                    matchPlayerInfo.PlayerId
                );
                typedPlayer.Disconnect();
                return false;
            }

            if (!MatchHelper.TryAssignPlayerInfoToMatchInfo(Owner, matchPlayerInfo))
            {
                _log.Error("Failed to assign player info for ({playerId}). TryCreate aborted.", matchPlayer.Id);
                typedPlayer.Disconnect();
                return false;
            }

            if (!base.TryAdd(typedPlayer))
            {
                MatchHelper.TryRemovePlayerInfoFromMatchInfo(Owner, matchPlayerInfo);
                _log.Error("Failed to add player ({playerId}) to session manager.", matchPlayer.Id);
                typedPlayer.Disconnect();
                return false;
            }

            authUser.Dispose();

            PlayerJoinEvent?.Invoke(Owner, typedPlayer);
            return true;
        }

        public override bool TryRemove(Guid id, [NotNullWhen(true)] out TPlayer? matchPlayer)
        {
            if (Owner == null)
            {
                _log.Error("Owner is null. TryRemove aborted for player ({playerId}).", id);
                matchPlayer = null;
                return false;
            }

            matchPlayer = this[id];
            if (matchPlayer == null)
            {
                _log.Error("Player ({playerId}) not found for removal.", id);
                return false;
            }

            var matchPlayerInfo = Owner.MatchInformation.GetPlayer(matchPlayer.Id);
            if (matchPlayerInfo == null)
            {
                _log.Error("Player ({playerId}) info not found in match info.", matchPlayer.Id);
                return false;
            }

            if (!MatchHelper.TryRemovePlayerInfoFromMatchInfo(Owner, matchPlayerInfo) || !base.TryRemove(matchPlayer.Id, out _))
            {
                _log.Error("Failed to remove player ({playerId}). Disposing room.", matchPlayer.Id);
                Owner.Dispose();
                return false;
            }

            PlayerLeaveEvent?.Invoke(Owner, matchPlayer, matchPlayerInfo);
            return true;
        }
        #endregion

        #region Dispose

        protected override void OnDispose()
        {
            PlayerJoinEvent = null;
            PlayerLeaveEvent = null;
            base.OnDispose();
        }

        #endregion
    }
}