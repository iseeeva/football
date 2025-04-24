// Token: 0x02000006 RID: 6
using System.Numerics;

public static class GClass97
{
    // Token: 0x0600001B RID: 27 RVA: 0x00002096 File Offset: 0x00000296
    public static byte smethod_0(byte byte_0, byte byte_1, float float_3)
    {
        return (byte)((float)byte_1 * float_3 + (float)byte_0 * (1f - float_3));
    }

    // Token: 0x0600001C RID: 28 RVA: 0x000020A8 File Offset: 0x000002A8
    public static sbyte smethod_1(sbyte sbyte_0, sbyte sbyte_1, float float_3)
    {
        return (sbyte)((float)sbyte_1 * float_3 + (float)sbyte_0 * (1f - float_3));
    }

    // Token: 0x0600001D RID: 29 RVA: 0x000020BA File Offset: 0x000002BA
    public static short smethod_2(short short_0, short short_1, float float_3)
    {
        return (short)((float)short_1 * float_3 + (float)short_0 * (1f - float_3));
    }

    // Token: 0x0600001E RID: 30 RVA: 0x000020CC File Offset: 0x000002CC
    public static ushort smethod_3(ushort ushort_0, ushort ushort_1, float float_3)
    {
        return (ushort)((float)ushort_1 * float_3 + (float)ushort_0 * (1f - float_3));
    }

    // Token: 0x0600001F RID: 31 RVA: 0x000020DE File Offset: 0x000002DE
    public static int smethod_4(int int_0, int int_1, float float_3)
    {
        return (int)((float)int_1 * float_3 + (float)int_0 * (1f - float_3));
    }

    // Token: 0x06000020 RID: 32 RVA: 0x000020F0 File Offset: 0x000002F0
    public static uint smethod_5(uint uint_0, uint uint_1, float float_3)
    {
        return (uint)(uint_1 * float_3 + uint_0 * (1f - float_3));
    }

    // Token: 0x06000021 RID: 33 RVA: 0x00002104 File Offset: 0x00000304
    public static float smethod_6(float float_3, float float_4, float float_5)
    {
        return float_4 * float_5 + float_3 * (1f - float_5);
    }

    // Token: 0x06000022 RID: 34 RVA: 0x00002113 File Offset: 0x00000313
    public static bool smethod_7(float float_3, float float_4)
    {
        return GClass97.smethod_8(float_3, float_4, GClass97.float_2);
    }

    // Token: 0x06000023 RID: 35 RVA: 0x00002121 File Offset: 0x00000321
    public static bool smethod_8(float float_3, float float_4, float float_5)
    {
        return (float_3 - float_4) * (float_3 - float_4) < float_5 * float_5;
    }

    // Token: 0x06000024 RID: 36 RVA: 0x0000212F File Offset: 0x0000032F
    public static float smethod_9(float float_3)
    {
        return Math.Abs(float_3);
    }

    // Token: 0x06000025 RID: 37 RVA: 0x00002138 File Offset: 0x00000338
    public static float smethod_10(float float_3)
    {
        return (float)Math.Sin((double)float_3);
    }

    // Token: 0x06000026 RID: 38 RVA: 0x00002142 File Offset: 0x00000342
    public static float smethod_11(float float_3)
    {
        return (float)Math.Cos((double)float_3);
    }

    // Token: 0x06000027 RID: 39 RVA: 0x0000214C File Offset: 0x0000034C
    public static float smethod_12(float float_3)
    {
        return (float)Math.Acos((double)float_3);
    }

    // Token: 0x06000028 RID: 40 RVA: 0x00002156 File Offset: 0x00000356
    public static float smethod_13(float float_3)
    {
        return (float)Math.Asin((double)float_3);
    }

    // Token: 0x06000029 RID: 41 RVA: 0x00002160 File Offset: 0x00000360
    public static float smethod_14(float float_3)
    {
        return (float)Math.Tan((double)float_3);
    }

    // Token: 0x0600002A RID: 42 RVA: 0x0000216A File Offset: 0x0000036A
    public static float smethod_15(float float_3, float float_4)
    {
        return (float)Math.Atan2((double)float_3, (double)float_4);
    }

    // Token: 0x0600002B RID: 43 RVA: 0x00002176 File Offset: 0x00000376
    public static bool smethod_16(float float_3, float float_4, float float_5)
    {
        return float_3 <= float_5 && float_3 >= float_4;
    }

    // Token: 0x0600002C RID: 44 RVA: 0x00002185 File Offset: 0x00000385
    public static double smethod_17(double double_0, double double_1, double double_2)
    {
        return Math.Min(Math.Max(double_0, double_1), double_2);
    }

    // Token: 0x0600002D RID: 45 RVA: 0x00002194 File Offset: 0x00000394
    public static float smethod_18(float float_3, float float_4, float float_5)
    {
        return Math.Min(Math.Max(float_3, float_4), float_5);
    }

    // Token: 0x0600002E RID: 46 RVA: 0x000021A3 File Offset: 0x000003A3
    public static int smethod_19(int int_0, int int_1, int int_2)
    {
        return Math.Min(Math.Max(int_0, int_1), (int_2 > int_1) ? int_2 : int_1);
    }

    // Token: 0x0600002F RID: 47 RVA: 0x000021B9 File Offset: 0x000003B9
    public static float smethod_20(float float_3)
    {
        return float_3 * 180f / GClass97.float_0;
    }

    // Token: 0x06000030 RID: 48 RVA: 0x000021C8 File Offset: 0x000003C8
    public static float smethod_21(float float_3)
    {
        return float_3 * GClass97.float_0 / 180f;
    }

    // Token: 0x06000031 RID: 49 RVA: 0x000021D7 File Offset: 0x000003D7
    public static bool smethod_22(float float_3)
    {
        return !float.IsNaN(float_3) && !float.IsInfinity(float_3);
    }

    // Token: 0x06000032 RID: 50 RVA: 0x00002B70 File Offset: 0x00000D70
    public static GClass97.GClass98 smethod_23(float float_3, float float_4, float float_5, float float_6)
    {
        float num = (float_6 - float_4) / (float_5 - float_3);
        float float_7 = -(float_3 * num - float_4);
        return new GClass97.GClass98(num, float_7);
    }

    // Token: 0x06000033 RID: 51 RVA: 0x00002B94 File Offset: 0x00000D94
    public static float[] smethod_24(float float_3, float float_4, float float_5, float float_6)
    {
        float[] array = new float[3];
        float num = (float_6 * float_3 * float_3 - float_5 * float_5 * float_4) / (float_5 * float_3 * float_3 - float_3 * float_5 * float_5);
        float num2 = (float_4 - num * float_3) / (float_3 * float_3);
        array[0] = num2;
        array[1] = num;
        array[2] = 0f;
        return array;
    }

    // Token: 0x06000034 RID: 52 RVA: 0x00002BDC File Offset: 0x00000DDC
    public static float[] smethod_25(float float_3, float float_4, float float_5)
    {
        float num = float_4 * float_4 - 4f * float_3 * float_5;
        if (num < 0f)
        {
            return null;
        }
        num = (float)Math.Sqrt((double)num);
        return new float[]
        {
            (-float_4 + num) / (2f * float_3),
            (-float_4 - num) / (2f * float_3)
        };
    }

    // Token: 0x06000035 RID: 53 RVA: 0x00002C30 File Offset: 0x00000E30
    public static byte smethod_26(float float_3, float float_4, float float_5)
    {
        float num = (float_3 - float_4) / (float_5 - float_4);
        return (byte)(num * 255f + 0f);
    }

    // Token: 0x06000036 RID: 54 RVA: 0x00002C54 File Offset: 0x00000E54
    public static sbyte smethod_27(float float_3, float float_4, float float_5)
    {
        float num = (float_3 - float_4) / (float_5 - float_4);
        return (sbyte)(num * 255f + -128f);
    }

    // Token: 0x06000037 RID: 55 RVA: 0x00002C78 File Offset: 0x00000E78
    public static short smethod_28(float float_3, float float_4, float float_5)
    {
        float num = (float_3 - float_4) / (float_5 - float_4);
        return (short)(num * 65535f + -32768f);
    }

    // Token: 0x06000038 RID: 56 RVA: 0x00002C9C File Offset: 0x00000E9C
    public static ushort smethod_29(float float_3, float float_4, float float_5)
    {
        float num = (float_3 - float_4) / (float_5 - float_4);
        return (ushort)(num * 65535f + 0f);
    }

    // Token: 0x06000039 RID: 57 RVA: 0x00002CC0 File Offset: 0x00000EC0
    public static float smethod_30(byte byte_0, float float_3, float float_4)
    {
        float num = ((float)byte_0 - 0f) / 255f;
        return num * (float_4 - float_3) + float_3;
    }

    // Token: 0x0600003A RID: 58 RVA: 0x00002CE4 File Offset: 0x00000EE4
    public static float smethod_31(sbyte sbyte_0, float float_3, float float_4)
    {
        float num = ((float)sbyte_0 - -128f) / 255f;
        return num * (float_4 - float_3) + float_3;
    }

    // Token: 0x0600003B RID: 59 RVA: 0x00002D08 File Offset: 0x00000F08
    public static float smethod_32(short short_0, float float_3, float float_4)
    {
        float num = ((float)short_0 - -32768f) / 65535f;
        return num * (float_4 - float_3) + float_3;
    }

    // Token: 0x0600003C RID: 60 RVA: 0x00002D2C File Offset: 0x00000F2C
    public static float smethod_33(ushort ushort_0, float float_3, float float_4)
    {
        float num = ((float)ushort_0 - 0f) / 65535f;
        return num * (float_4 - float_3) + float_3;
    }

    // Token: 0x0600003D RID: 61 RVA: 0x000021EC File Offset: 0x000003EC
    public static Vector3 smethod_34(Vector2 vector2_0)
    {
        return GClass97.smethod_35(vector2_0, 0f);
    }

    // Token: 0x0600003E RID: 62 RVA: 0x000021F9 File Offset: 0x000003F9
    public static Vector3 smethod_35(Vector2 vector2_0, float float_3)
    {
        return new Vector3(vector2_0.X, vector2_0.Y, float_3);
    }

    // Token: 0x0600003F RID: 63 RVA: 0x0000220F File Offset: 0x0000040F
    public static Vector2 smethod_36(Vector3 vector3_8)
    {
        return new Vector2(vector3_8.X, vector3_8.Y);
    }

    // Token: 0x06000040 RID: 64 RVA: 0x00002224 File Offset: 0x00000424
    public static Vector3 smethod_37(Vector3 vector3_8)
    {
        return new Vector3(vector3_8.X, vector3_8.Y, 0f);
    }

    // Token: 0x06000041 RID: 65 RVA: 0x0000223E File Offset: 0x0000043E
    public static Vector4 smethod_38(Vector3 vector3_8, float float_3)
    {
        return new Vector4(vector3_8.X, vector3_8.Y, vector3_8.Z, float_3);
    }

    // Token: 0x06000042 RID: 66 RVA: 0x00002D50 File Offset: 0x00000F50
    public static float smethod_39(Vector3 vector3_8, Vector3 vector3_9)
    {
        float num = vector3_9.X - vector3_8.X;
        float num2 = vector3_9.Y - vector3_8.Y;
        float num3 = vector3_9.Z - vector3_8.Z;
        return num * num + num2 * num2 + num3 * num3;
    }

    // Token: 0x06000043 RID: 67 RVA: 0x00002D98 File Offset: 0x00000F98
    public static float smethod_40(Vector2 vector2_0, Vector2 vector2_1)
    {
        float num = vector2_1.X - vector2_0.X;
        float num2 = vector2_1.Y - vector2_0.Y;
        return num * num + num2 * num2;
    }

    // Token: 0x06000044 RID: 68 RVA: 0x00002DCC File Offset: 0x00000FCC
    public static float smethod_41(Vector3 vector3_8, Vector3 vector3_9)
    {
        float num = vector3_9.X - vector3_8.X;
        float num2 = vector3_9.Y - vector3_8.Y;
        float num3 = vector3_9.Z - vector3_8.Z;
        return (float)Math.Sqrt((double)(num * num + num2 * num2 + num3 * num3));
    }

    // Token: 0x06000045 RID: 69 RVA: 0x00002E1C File Offset: 0x0000101C
    public static float smethod_42(Vector2 vector2_0, Vector2 vector2_1)
    {
        float num = vector2_1.X - vector2_0.X;
        float num2 = vector2_1.Y - vector2_0.Y;
        return (float)Math.Sqrt((double)(num * num + num2 * num2));
    }

    // Token: 0x06000046 RID: 70 RVA: 0x0000225B File Offset: 0x0000045B
    public static Vector3 smethod_43(Vector3 vector3_8, Vector3 vector3_9)
    {
        return new Vector3(vector3_8.X + vector3_9.X, vector3_8.Y + vector3_9.Y, vector3_8.Z + vector3_9.Z);
    }

    // Token: 0x06000047 RID: 71 RVA: 0x0000228F File Offset: 0x0000048F
    public static Vector3 smethod_44(Vector3 vector3_8, double double_0)
    {
        return new Vector3((float)((double)vector3_8.X * double_0), (float)((double)vector3_8.Y * double_0), (float)((double)vector3_8.Z * double_0));
    }

    // Token: 0x06000048 RID: 72 RVA: 0x000022B7 File Offset: 0x000004B7
    public static Vector3 smethod_45(Vector3 vector3_8, Vector3 vector3_9)
    {
        return new Vector3(vector3_8.X * vector3_9.X, vector3_8.Y * vector3_9.Y, vector3_8.Z * vector3_9.Z);
    }

    // Token: 0x06000049 RID: 73 RVA: 0x000022EB File Offset: 0x000004EB
    public static float smethod_46(Vector3 vector3_8, Vector3 vector3_9)
    {
        return GClass97.smethod_12(GClass97.smethod_18(Vector3.Dot(GClass97.smethod_64(vector3_8), GClass97.smethod_64(vector3_9)), -1f, 1f));
    }

    // Token: 0x0600004A RID: 74 RVA: 0x00002312 File Offset: 0x00000512
    public static float smethod_47(Vector2 vector2_0, Vector2 vector2_1)
    {
        return GClass97.smethod_12(GClass97.smethod_18(Vector2.Dot(GClass97.smethod_65(vector2_0), GClass97.smethod_65(vector2_1)), -1f, 1f));
    }

    // Token: 0x0600004B RID: 75 RVA: 0x00002339 File Offset: 0x00000539
    public static bool smethod_48(Vector3 vector3_8, Vector3 vector3_9)
    {
        return GClass97.smethod_49(vector3_8, vector3_9, GClass97.float_2);
    }

    // Token: 0x0600004C RID: 76 RVA: 0x00002E58 File Offset: 0x00001058
    public static bool smethod_49(Vector3 vector3_8, Vector3 vector3_9, float float_3)
    {
        return (vector3_8 - vector3_9).LengthSquared() < float_3 * float_3;
    }

    // Token: 0x0600004D RID: 77 RVA: 0x00002347 File Offset: 0x00000547
    public static bool smethod_50(Quaternion quaternion_0, Quaternion quaternion_1)
    {
        return GClass97.smethod_51(quaternion_0, quaternion_1, GClass97.float_2);
    }

    // Token: 0x0600004E RID: 78 RVA: 0x00002E7C File Offset: 0x0000107C
    public static bool smethod_51(Quaternion quaternion_0, Quaternion quaternion_1, float float_3)
    {
        float float_4 = Quaternion.Dot(quaternion_0, quaternion_1);
        return GClass97.smethod_8(float_4, 1f, float_3);
    }

    // Token: 0x0600004F RID: 79 RVA: 0x00002EA0 File Offset: 0x000010A0
    public static Quaternion smethod_52(Vector3 vector3_8, Vector3 vector3_9)
    {
        vector3_8 = GClass97.smethod_64(vector3_8);
        vector3_9 = GClass97.smethod_64(vector3_9);
        Vector3 v;
        if (GClass97.smethod_48(vector3_8, -vector3_9))
        {
            if (GClass97.smethod_48(vector3_8, GClass97.vector3_0))
            {
                v = Vector3.Cross(GClass97.vector3_1, vector3_9);
            }
            else if (!GClass97.smethod_48(GClass97.vector3_0, vector3_9) && !GClass97.smethod_48(GClass97.vector3_0, -vector3_9))
            {
                v = Vector3.Cross(GClass97.vector3_0, vector3_9);
            }
            else
            {
                v = Vector3.Cross(GClass97.vector3_1, vector3_9);
            }
        }
        else
        {
            v = Vector3.Cross(vector3_8, vector3_9);
        }
        float angle = GClass97.smethod_46(vector3_8, vector3_9);
        return Quaternion.CreateFromAxisAngle(v, angle);
    }

    // Token: 0x06000050 RID: 80 RVA: 0x00002355 File Offset: 0x00000555
    public static Vector3 smethod_53(Vector3 vector3_8, Quaternion quaternion_0)
    {
        return Vector3.TransformNormal(vector3_8, Matrix4x4.CreateFromQuaternion(quaternion_0));
    }

    // Token: 0x06000051 RID: 81 RVA: 0x00002F38 File Offset: 0x00001138
    public static Vector3 smethod_54(Vector3 vector3_8, float float_3)
    {
        return new Vector3(vector3_8.X, vector3_8.Y * GClass97.smethod_11(float_3) - vector3_8.Z * GClass97.smethod_10(float_3), vector3_8.Y * GClass97.smethod_10(float_3) + vector3_8.Z * GClass97.smethod_11(float_3));
    }

    // Token: 0x06000052 RID: 82 RVA: 0x00002F8C File Offset: 0x0000118C
    public static Vector3 smethod_55(Vector3 vector3_8, float float_3)
    {
        return new Vector3(vector3_8.Z * GClass97.smethod_11(float_3) - vector3_8.X * GClass97.smethod_10(float_3), vector3_8.Y, vector3_8.Z * GClass97.smethod_10(float_3) + vector3_8.X * GClass97.smethod_11(float_3));
    }

    // Token: 0x06000053 RID: 83 RVA: 0x00002FE0 File Offset: 0x000011E0
    public static Vector3 smethod_56(Vector3 vector3_8, float float_3)
    {
        return new Vector3(vector3_8.X * GClass97.smethod_11(float_3) - vector3_8.Y * GClass97.smethod_10(float_3), vector3_8.X * GClass97.smethod_10(float_3) + vector3_8.Y * GClass97.smethod_11(float_3), vector3_8.Z);
    }

    // Token: 0x06000054 RID: 84 RVA: 0x00002363 File Offset: 0x00000563
    public static Vector3 smethod_57(float float_3, Vector3 vector3_8)
    {
        return new Vector3(float_3 / vector3_8.X, float_3 / vector3_8.Y, float_3 / vector3_8.Z);
    }

    // Token: 0x06000055 RID: 85 RVA: 0x00002385 File Offset: 0x00000585
    public static Vector3 smethod_58(Vector3 vector3_8, float float_3)
    {
        return new Vector3(vector3_8.X / float_3, vector3_8.Y / float_3, vector3_8.Z / float_3);
    }

    // Token: 0x06000056 RID: 86 RVA: 0x00003034 File Offset: 0x00001234
    public static bool smethod_59(Vector3 vector3_8, Vector3 vector3_9, Vector3 vector3_10, float float_3)
    {
        float num = vector3_8.X - vector3_10.X;
        float num2 = vector3_9.X - vector3_10.X;
        float num3 = vector3_8.Y - vector3_10.Y;
        float num4 = vector3_9.Y - vector3_10.Y;
        float num5 = num2 - num;
        float num6 = num4 - num3;
        float num7 = num5 * num5 + num6 * num6;
        float num8 = num * num4 - num2 * num3;
        float num9 = float_3 * float_3 * num7 - num8 * num8;
        return num9 >= 0f;
    }

    // Token: 0x06000057 RID: 87 RVA: 0x000030BC File Offset: 0x000012BC
    public static bool smethod_60(Vector2 vector2_0, Vector2 vector2_1, Vector2 vector2_2, float float_3)
    {
        Vector2 vector = vector2_1 + vector2_0 * 1f;
        Vector2 vector2 = vector2_1;
        Vector2 vector3 = vector2_2;
        double num = Math.Pow((double)(vector.X - vector2.X), 2.0) + Math.Pow((double)(vector.Y - vector2.Y), 2.0);
        double num2 = (double)(2f * ((vector.X - vector2.X) * (vector2.X - vector3.X) + (vector.Y - vector2.Y) * (vector2.Y - vector3.Y)));
        double num3 = Math.Pow((double)vector3.X, 2.0) + Math.Pow((double)vector3.Y, 2.0) + Math.Pow((double)vector2.X, 2.0) + Math.Pow((double)vector2.Y, 2.0) - (double)(2f * (vector3.X * vector2.X + vector3.Y * vector2.Y)) - (double)(float_3 * float_3);
        double num4 = num2 * num2 - 4.0 * num * num3;
        return num4 >= 0.0;
    }

    // Token: 0x06000058 RID: 88 RVA: 0x000023A7 File Offset: 0x000005A7
    public static Quaternion smethod_61(Quaternion quaternion_0, float float_3)
    {
        return new Quaternion(quaternion_0.X * float_3, quaternion_0.Y * float_3, quaternion_0.Z * float_3, quaternion_0.W * float_3);
    }

    // Token: 0x06000059 RID: 89 RVA: 0x000023D2 File Offset: 0x000005D2
    public static Quaternion smethod_62(Quaternion quaternion_0, Quaternion quaternion_1, float float_3)
    {
        return Quaternion.Normalize(GClass97.smethod_61(quaternion_0, 1f - float_3) + GClass97.smethod_61(quaternion_1, float_3));
    }

    // Token: 0x0600005A RID: 90 RVA: 0x00003214 File Offset: 0x00001414
    public static Vector3 smethod_63(Vector3 vector3_8)
    {
        float num = (float)Math.Sqrt((double)(vector3_8.X * vector3_8.X + vector3_8.Y * vector3_8.Y));
        return new Vector3(vector3_8.X / num, vector3_8.Y / num, 0f);
    }

    // Token: 0x0600005B RID: 91 RVA: 0x00003264 File Offset: 0x00001464
    public static Vector3 smethod_64(Vector3 vector3_8)
    {
        float num = (float)Math.Sqrt((double)(vector3_8.X * vector3_8.X + vector3_8.Y * vector3_8.Y + vector3_8.Z * vector3_8.Z));
        return new Vector3(vector3_8.X / num, vector3_8.Y / num, vector3_8.Z / num);
    }

    // Token: 0x0600005C RID: 92 RVA: 0x000032C8 File Offset: 0x000014C8
    public static Vector2 smethod_65(Vector2 vector2_0)
    {
        float num = (float)Math.Sqrt((double)(vector2_0.X * vector2_0.X + vector2_0.Y * vector2_0.Y));
        return new Vector2(vector2_0.X / num, vector2_0.Y / num);
    }

    // Token: 0x04000007 RID: 7
    public static float float_0 = 3.1415927f;

    // Token: 0x04000008 RID: 8
    public static float float_1 = 6.2831855f;

    // Token: 0x04000009 RID: 9
    public static float float_2 = 0.001f;

    // Token: 0x0400000A RID: 10
    public static Vector3 vector3_0 = new Vector3(1f, 0f, 0f);

    // Token: 0x0400000B RID: 11
    public static Vector3 vector3_1 = new Vector3(0f, 1f, 0f);

    // Token: 0x0400000C RID: 12
    public static Vector3 vector3_2 = new Vector3(0f, 0f, 1f);

    // Token: 0x0400000D RID: 13
    public static Vector3 vector3_3 = new Vector3(-1f, 0f, 0f);

    // Token: 0x0400000E RID: 14
    public static Vector3 vector3_4 = new Vector3(0f, -1f, 0f);

    // Token: 0x0400000F RID: 15
    public static Vector3 vector3_5 = new Vector3(0f, 0f, -1f);

    // Token: 0x04000010 RID: 16
    public static Plane plane_0 = new Plane(1f, 0f, 0f, 0f);

    // Token: 0x04000011 RID: 17
    public static Plane plane_1 = new Plane(-1f, 0f, 0f, 0f);

    // Token: 0x04000012 RID: 18
    public static Plane plane_2 = new Plane(0f, 1f, 0f, 0f);

    // Token: 0x04000013 RID: 19
    public static Plane plane_3 = new Plane(0f, -1f, 0f, 0f);

    // Token: 0x04000014 RID: 20
    public static Plane plane_4 = new Plane(0f, 0f, 1f, 0f);

    // Token: 0x04000015 RID: 21
    public static Plane plane_5 = new Plane(0f, 0f, -1f, 0f);

    // Token: 0x04000016 RID: 22
    public static Vector3 vector3_6 = new Vector3(float.MinValue, float.MinValue, float.MinValue);

    // Token: 0x04000017 RID: 23
    public static Vector3 vector3_7 = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

    // Token: 0x04000018 RID: 24
    public static Matrix4x4 matrix_0 = Matrix4x4.Identity;

    // Token: 0x02000007 RID: 7
    public class GClass98
    {
        // Token: 0x0600005E RID: 94 RVA: 0x000023F2 File Offset: 0x000005F2
        public GClass98(float float_2, float float_3)
        {
            this.float_0 = float_2;
            this.float_1 = float_3;
        }

        // Token: 0x0600005F RID: 95 RVA: 0x00002408 File Offset: 0x00000608
        public float method_0(float float_2)
        {
            return this.float_0 * float_2 + this.float_1;
        }

        // Token: 0x06000060 RID: 96 RVA: 0x00002419 File Offset: 0x00000619
        public float method_1(float float_2)
        {
            return (float_2 - this.float_1) / this.float_0;
        }

        // Token: 0x04000019 RID: 25
        private float float_0;

        // Token: 0x0400001A RID: 26
        private float float_1;
    }
}
