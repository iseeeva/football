using System.Numerics;
using Sobee.Common;
using Sobee.TestServer.Match;
using Sobee.TestServer.Messages.Ball;

namespace Sobee.TestServer.MatchComponents
{
    public class MatchBall : MatchComponent
    {
        private readonly Serilog.ILogger _log = Logging.Get<MatchBall>();
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

        public MatchBall(MatchRoom room) : base(room)
        {
            _log.Debug("{id} initialized.", Id);
        }

        public override Task Update(double delta)
        {
            var matchInformation = _matchRoom.MatchInformation;
            bool isActionerUnset = matchInformation.Actor.BallOwner == -1;

            if (isActionerUnset)
            {
                if (matchInformation.BallVelocity.Length() > Epsilon)
                {
                    matchInformation.BallVelocity = new Vector3(
                        matchInformation.BallVelocity.X * Reduction,
                        matchInformation.BallVelocity.Y * Reduction,
                        (matchInformation.BallVelocity.Z + Gravity * (float)delta) * Reduction
                    );

                    matchInformation.BallPosition += matchInformation.BallVelocity * (float)delta;

                    if (matchInformation.BallPosition.Z <= BallBoundary.Z)
                    {
                        matchInformation.BallPosition.Z = BallBoundary.Z;

                        matchInformation.BallVelocity = new Vector3(
                            matchInformation.BallVelocity.X,
                            matchInformation.BallVelocity.Y,
                            -matchInformation.BallVelocity.Z * Reduction
                        );
                    }

                    _matchRoom.Players.SendMessage(new BallUpdateMessage(
                        matchInformation.BallPosition,
                        matchInformation.BallVelocity
                    ));
                }
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
                    _log.Information("{id} disposed.", Id);
                }
            }

            base.Dispose(disposing);
        }
    }
}
