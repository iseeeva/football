using System.Numerics;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Ball
{
    [MessageAttribute(50549)]
    public class BallInterceptTxMessage : AnimationAbstractMessage
    {
        /// <summary> absolute </summary>
        public readonly sbyte SquadNumber;
        public readonly Vector2 PlayerPosition;
        public readonly Vector2 PlayerDirection;
        public readonly Vector3 BallVelocity;
        public readonly bool IsInterceptSuccess;
        public readonly InterceptType InterceptAnimType;

        // INFO: Intercept'in default animasyon suresinden daha kisa surede gerceklesmesi gerekiyorsa animasyonu istedigin andan baslatabilirsin. 
        // AnimStartTime 0 ise intercept animasyonu bastan baslar.
        // AnimStartTime 0'dan buyuk ise intercept animasyonu belirledigin AnimStartTime'dan (EndTime'i client belirliyor) baslar.
        /// <summary> Animasyon baslangic offseti (milisaniye cinsinden) <br/>
        /// INFO: Sadece IsInterceptSuccess true ise gecerli. </summary>
        public readonly ushort AnimStartTime;

        public double AnimStartTimeBySecond()
        {
            return (double)this.AnimStartTime / 1000.0;
        }

        public BallInterceptTxMessage(BinaryReader reader) : base(reader)
        {
            this.SquadNumber = reader.method_11();
            this.PlayerPosition = reader.method_19();
            this.PlayerDirection = GClass97.smethod_10_11_c(reader.method_12());
            this.BallVelocity = reader.method_20();
            this.IsInterceptSuccess = reader.method_1();
            this.InterceptAnimType = (InterceptType)reader.method_9();
            this.AnimStartTime = reader.method_15();
        }

        public BallInterceptTxMessage(
            sbyte squadNumber,
            Vector2 playerPosition,
            Vector2 playerDirection,
            Vector3 ballVelocity,
            bool isInterceptSuccess,
            InterceptType interceptType,
            ushort animStartType,
            AnimationType animationType_1
        ) : base(animationType_1)
        {
            this.SquadNumber = squadNumber;
            this.PlayerPosition = playerPosition;
            this.PlayerDirection = playerDirection;
            this.BallVelocity = ballVelocity;
            this.IsInterceptSuccess = isInterceptSuccess;
            this.InterceptAnimType = interceptType;
            this.AnimStartTime = animStartType;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_11(this.SquadNumber);
            writer.method_19(this.PlayerPosition);
            writer.method_12(GClass97.smethod_15(PlayerDirection.Y, PlayerDirection.X));
            writer.method_20(this.BallVelocity);
            writer.method_1(this.IsInterceptSuccess);
            writer.method_9((int)this.InterceptAnimType);
            writer.method_15(this.AnimStartTime);
        }

        public override string ToString()
        {
            return string.Concat(new object[]
            {
            "PlayerIntercept - ",
            this.SquadNumber,
            " - ",
            this.BallVelocity.ToString(),
            " - ",
            this.IsInterceptSuccess,
            " - ",
            this.InterceptAnimType.ToString()
            });
        }
    }
}
