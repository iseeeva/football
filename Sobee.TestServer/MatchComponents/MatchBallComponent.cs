using System.Numerics;
using Sobee.Common;
using Sobee.Serialization.GameServer;
using Sobee.TestServer.Match;
using Sobee.TestServer.Messages.Ball;

namespace Sobee.TestServer.MatchComponents
{
    public class MatchBallComponent : MatchComponent
    {
        // TODO: Event sistemine geri donmek iyi mi olurdu?

        private readonly Serilog.ILogger _log = Logging.Get<MatchBallComponent>();
        private bool _isDisposed;

        /// <summary>
        /// Minimum speed before stopping the ball movement
        /// </summary>
        public float Epsilon = 10f;

        /// <summary>
        /// Gravity affecting the ball's vertical movement
        /// </summary>
        public float Gravity = (-9.81f * 100f) * 2.5f;

        /// <summary>
        /// Reduction factor for ball speed over time
        /// </summary>
        public float Reduction = 0.985f;

        /// <summary>
        /// Radius for ball collision detection
        /// </summary>
        public float BallCollisionRadius = 30f;

        /// <summary>
        /// Maximum speed of the ball
        /// </summary>
        public float BallMaxSpeed = 2000f;

        /// <summary>
        /// Boundary for the ball
        /// </summary>
        public readonly Vector3 BallBoundry = new(0, 0, 11.254f);

        public MatchBallComponent(MatchRoom room) : base(room)
        {
            _log.Debug("{id} initialized.", Id);
        }

        public override Task Update(double delta)
        {
            var matchInformation = _matchRoom.MatchInformation;
            var actorInformation = matchInformation.Actor;

            if (matchInformation.FieldPositioning == MatchFieldPositioning.Running)
            {
                // Ball collision control
                foreach (var playerInformation in matchInformation.GetPlayers())
                {
                    if (!_matchRoom.Players.TryGet(playerInformation.PlayerId, out var matchPlayer))
                        throw new Exception("Nasil amk");

                    bool isPlayerInCollision = Vector2.Distance(
                        new Vector2(matchInformation.BallPosition.X, matchInformation.BallPosition.Y),
                        playerInformation.Position
                    ) <= BallCollisionRadius;

                    bool isActionerUnset = actorInformation.Actioner == -1;

                    if (isPlayerInCollision && isActionerUnset)
                    {
                        actorInformation.Actioner = (sbyte)matchPlayer.AuthInformation.Entry.ToSquad();

                        matchInformation.BallPosition = new Vector3(playerInformation.Position.X, playerInformation.Position.Y, BallBoundry.Z);
                        matchInformation.BallVelocity = new Vector3(0, 0, 0);

                        _matchRoom.Players.SendMessage(new BallUpdate(
                            matchInformation.BallPosition,
                            matchInformation.BallVelocity
                        ));

                        _matchRoom.Players.SendMessage(new BallGet(
                            actorInformation.Actioner,
                            playerInformation.Position,
                            playerInformation.Direction,
                            0
                        ));
                    }
                }
            }

            // Ball physics 
            if (matchInformation.BallVelocity.Length() > Epsilon)
            {
                // Apply gravity to vertical velocity
                matchInformation.BallVelocity = new Vector3(
                    matchInformation.BallVelocity.X,
                    matchInformation.BallVelocity.Y,
                    matchInformation.BallVelocity.Z + Gravity * (float)delta
                );

                // Update ball position based on velocity
                matchInformation.BallPosition += matchInformation.BallVelocity * (float)delta;
                matchInformation.BallVelocity = new Vector3(
                    matchInformation.BallVelocity.X * Reduction,
                    matchInformation.BallVelocity.Y * Reduction,
                    matchInformation.BallVelocity.Z
                );

                // Ensure ball doesn't go below the ground level
                if (matchInformation.BallPosition.Z < BallBoundry.Z)
                {
                    matchInformation.BallPosition = new Vector3(
                        matchInformation.BallPosition.X,
                        matchInformation.BallPosition.Y,
                        BallBoundry.Z
                    );
                    matchInformation.BallVelocity = new Vector3(
                        matchInformation.BallVelocity.X,
                        matchInformation.BallVelocity.Y,
                        0
                    );
                }

                // Send ball update to all players
                _matchRoom.Players.SendMessage(new BallUpdate(
                    matchInformation.BallPosition,
                    matchInformation.BallVelocity
                ));
            }
            else
            {
                // Stop the ball if below epsilon
                matchInformation.BallVelocity = new Vector3(0, 0, 0);
            }

            return Task.CompletedTask;
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                if (disposing)
                {

                }
            }

            base.Dispose(disposing);
        }
    }
}
