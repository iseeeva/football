using System.Numerics;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages.Ball
{
    // Token: 0x02000090 RID: 144
    [MessageAttribute(50549)]
    public class BallInterceptHitMessage : AnimationMessageAbstract
    {
        // Token: 0x040006B8 RID: 1720
        public readonly sbyte SquadNumber;

        // Token: 0x040006B9 RID: 1721
        public readonly Vector2 PlayerPosition;

        // Token: 0x040006BA RID: 1722
        public readonly Vector2 PlayerDirection;

        // Token: 0x040006BB RID: 1723
        public readonly Vector3 BallVelocity;

        public readonly bool IsInterceptSuccess;

        // Token: 0x040006BD RID: 1725
        public readonly InterceptType InterceptAnimType;

        // INFO: Intercept'in default animasyon suresinden daha kisa surede gerceklesmesi gerekiyorsa animasyonu istedigin andan baslatabilirsin. 
        // AnimStartTime 0 ise intercept animasyonu bastan baslar.
        // AnimStartTime 0'dan buyuk ise intercept animasyonu belirledigin AnimStartTime'dan (EndTime'i client belirliyor) baslar.
        /// <summary> Animasyon baslangic offseti (milisaniye cinsinden) <br/>
        /// INFO: Sadece IsInterceptSuccess true ise gecerli. </summary>
        public readonly ushort AnimStartTime;

        // Token: 0x06000397 RID: 919 RVA: 0x000050A4 File Offset: 0x000032A4
        public double method_7()
        {
            return (double)this.AnimStartTime / 1000.0;
        }

        // Token: 0x06000398 RID: 920 RVA: 0x0000DD44 File Offset: 0x0000BF44
        public BallInterceptHitMessage(BinaryReader reader) : base(reader)
        {
            this.SquadNumber = reader.method_11();
            this.PlayerPosition = reader.method_19();
            this.PlayerDirection = GClass97.smethod_10_11_c(reader.method_12());
            this.BallVelocity = reader.method_20();
            this.IsInterceptSuccess = reader.method_1();
            this.InterceptAnimType = (InterceptType)reader.method_9();
            this.AnimStartTime = reader.method_15();
        }

        // Token: 0x06000399 RID: 921 RVA: 0x0000DDAC File Offset: 0x0000BFAC
        public BallInterceptHitMessage(sbyte sbyte_1, Vector2 vector2_1, Vector2 vector2_2, Vector3 vector3_1, bool bool_1, InterceptType interceptType_1, ushort ushort_1, AnimationType animationType_1) : base(animationType_1)
        {
            this.SquadNumber = sbyte_1;
            this.PlayerPosition = vector2_1;
            this.PlayerDirection = vector2_2;
            this.BallVelocity = vector3_1;
            this.IsInterceptSuccess = bool_1;
            this.InterceptAnimType = interceptType_1;
            this.AnimStartTime = ushort_1;
        }

        // Token: 0x0600039A RID: 922 RVA: 0x0000DE08 File Offset: 0x0000C008
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

        // Token: 0x0600039B RID: 923 RVA: 0x0000DE80 File Offset: 0x0000C080
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
    }
}
