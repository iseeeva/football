using Football.Serialization;
using System.Numerics;

namespace Football.GameServer.Messages.Ball
{
    [Message(50549)]
    public class BallInterceptTxMessage : AnimationMessage
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
            return AnimStartTime / 1000.0;
        }

        public BallInterceptTxMessage(BinaryReader reader) : base(reader)
        {
            SquadNumber = reader.method_11();
            PlayerPosition = reader.method_19();
            PlayerDirection = GClass97.smethod_10_11_c(reader.method_12());
            BallVelocity = reader.method_20();
            IsInterceptSuccess = reader.method_1();
            InterceptAnimType = (InterceptType)reader.method_9();
            AnimStartTime = reader.method_15();
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
            SquadNumber = squadNumber;
            PlayerPosition = playerPosition;
            PlayerDirection = playerDirection;
            BallVelocity = ballVelocity;
            IsInterceptSuccess = isInterceptSuccess;
            InterceptAnimType = interceptType;
            AnimStartTime = animStartType;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.method_11(SquadNumber);
            writer.method_19(PlayerPosition);
            writer.method_12(GClass97.smethod_15(PlayerDirection.Y, PlayerDirection.X));
            writer.method_20(BallVelocity);
            writer.method_1(IsInterceptSuccess);
            writer.method_9((int)InterceptAnimType);
            writer.method_15(AnimStartTime);
        }

        public override string ToString()
        {
            return string.Concat(new object[]
            {
            "PlayerIntercept - ",
            SquadNumber,
            " - ",
            BallVelocity.ToString(),
            " - ",
            IsInterceptSuccess,
            " - ",
            InterceptAnimType.ToString()
            });
        }
    }
}
