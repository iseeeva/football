using System.Numerics;
using Sobee.Common;
using Sobee.TestServer.Match;
using Sobee.TestServer.Messages.Ball;

namespace Sobee.TestServer.MatchComponents
{
    public class MatchBallComponent : MatchComponent
    {
        private readonly Serilog.ILogger _log = Logging.Get<MatchBallComponent>();
        private bool _isDisposed;

        /// <summary>Minimum speed before stopping the ball movement</summary>
        public float Epsilon { get; set; } = 10f;
        /// <summary>Gravity affecting the ball's vertical movement</summary>
        public float Gravity { get; set; } = -9.81f * 100f * 2.5f;
        /// <summary>Reduction factor for ball speed over time</summary>
        public float Reduction { get; set; } = 0.985f;

        /// <summary>Radius for ball collision detection</summary>
        public double BallCollisionRadius { get; set; } = 15;
        /// <summary>Maximum speed of the ball</summary>
        public double BallMaxSpeed { get; set; } = 2000;
        /// <summary>Boundary for the ball (Z axis is ground level)</summary>
        public Vector3 BallBoundary { get; set; } = new(0, 0, 11.254f);

        public MatchBallComponent(MatchRoom room) : base(room)
        {
            _log.Debug("{id} initialized.", Id);
        }

        public override void Update(double delta)
        {
            var matchInfo = _matchRoom.MatchInformation;
            var ballOwnerInfo = matchInfo.GetPlayer(matchInfo.Actor.BallOwner);

            // INFO: Eger mac akisina esitlemek istersen TimeMultiplier kullan.
            var dt = (float)delta; // * matchInfo.TimeMultiplier;

            if (ballOwnerInfo != null)
            {
                //roomMatchInfo.BallPosition += ballOwnerMatchInfo.Velocity * dt;
                //roomMatchInfo.BallVelocity = Vector3.Zero;
            }
            else
            {
                if (matchInfo.BallVelocity.Length() <= Epsilon)
                    return;

                // Velocity 
                matchInfo.BallVelocity = new Vector3(
                    matchInfo.BallVelocity.X * Reduction,
                    matchInfo.BallVelocity.Y * Reduction,
                    (matchInfo.BallVelocity.Z + Gravity * dt) * Reduction
                );

                // Position 
                matchInfo.BallPosition += matchInfo.BallVelocity * dt;

                // Ground 
                if (matchInfo.BallPosition.Z <= BallBoundary.Z)
                {
                    matchInfo.BallPosition.Z = BallBoundary.Z;

                    matchInfo.BallVelocity = new Vector3(
                        matchInfo.BallVelocity.X,
                        matchInfo.BallVelocity.Y,
                        -matchInfo.BallVelocity.Z * Reduction
                    );
                }

                _matchRoom.Players.SendMessage(new BallUpdateMessage(
                    matchInfo.BallPosition,
                    matchInfo.BallVelocity
                ));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            if (disposing)
                _log.Debug("{id} disposed.", Id);

            base.Dispose(disposing);
        }
    }
}
