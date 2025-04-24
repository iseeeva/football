using System.Drawing;
using System.Globalization;
using System.Numerics;

// Token: 0x0200000B RID: 11
public static class GClass101
{
    // Token: 0x06000071 RID: 113 RVA: 0x00002519 File Offset: 0x00000719
    public static bool smethod_0(string string_0, out bool bool_0)
    {
        return bool.TryParse(string_0, out bool_0);
    }

    // Token: 0x06000072 RID: 114 RVA: 0x00002522 File Offset: 0x00000722
    public static bool smethod_1(string string_0, out int int_2)
    {
        return int.TryParse(string_0, NumberStyles.Any, GClass101.cultureInfo_0, out int_2);
    }

    // Token: 0x06000073 RID: 115 RVA: 0x00002535 File Offset: 0x00000735
    public static bool smethod_2(string string_0, out float float_0)
    {
        return float.TryParse(string_0, NumberStyles.Any, GClass101.cultureInfo_0, out float_0);
    }

    // Token: 0x06000074 RID: 116 RVA: 0x00002548 File Offset: 0x00000748
    public static bool smethod_3(string string_0)
    {
        return bool.Parse(string_0);
    }

    // Token: 0x06000075 RID: 117 RVA: 0x00002550 File Offset: 0x00000750
    public static ushort smethod_4(string string_0)
    {
        return ushort.Parse(string_0, GClass101.cultureInfo_0);
    }

    // Token: 0x06000076 RID: 118 RVA: 0x0000255D File Offset: 0x0000075D
    public static int smethod_5(string string_0)
    {
        return int.Parse(string_0, GClass101.cultureInfo_0);
    }

    // Token: 0x06000077 RID: 119 RVA: 0x0000256A File Offset: 0x0000076A
    public static float smethod_6(string string_0)
    {
        return float.Parse(string_0, GClass101.cultureInfo_0);
    }

    // Token: 0x06000078 RID: 120 RVA: 0x00002577 File Offset: 0x00000777
    public static string smethod_7(float float_0)
    {
        return float_0.ToString("F3", GClass101.cultureInfo_0);
    }

    // Token: 0x06000079 RID: 121 RVA: 0x0000258A File Offset: 0x0000078A
    public static string smethod_8(double double_0)
    {
        return double_0.ToString("F3", GClass101.cultureInfo_0);
    }

    // Token: 0x0600007A RID: 122 RVA: 0x0000259D File Offset: 0x0000079D
    public static string smethod_9(int int_2)
    {
        return int_2.ToString(GClass101.cultureInfo_0);
    }

    // Token: 0x0600007B RID: 123 RVA: 0x0000377C File Offset: 0x0000197C
    public static string smethod_10(Vector3 vector3_0)
    {
        return string.Concat(new string[]
        {
            GClass101.smethod_7(vector3_0.X),
            ",",
            GClass101.smethod_7(vector3_0.Y),
            ",",
            GClass101.smethod_7(vector3_0.Z)
        });
    }

    // Token: 0x0600007C RID: 124 RVA: 0x000037D4 File Offset: 0x000019D4
    public static string smethod_11(Vector4 vector4_0)
    {
        return string.Concat(new string[]
        {
            GClass101.smethod_7(vector4_0.X),
            ",",
            GClass101.smethod_7(vector4_0.Y),
            ",",
            GClass101.smethod_7(vector4_0.Z),
            ",",
            GClass101.smethod_7(vector4_0.W)
        });
    }

    // Token: 0x0600007D RID: 125 RVA: 0x000025AB File Offset: 0x000007AB
    public static string smethod_12(Vector2 vector2_0)
    {
        return GClass101.smethod_7(vector2_0.X) + "," + GClass101.smethod_7(vector2_0.Y);
    }

    // Token: 0x0600007E RID: 126 RVA: 0x000025CF File Offset: 0x000007CF
    public static string smethod_13(bool bool_0)
    {
        return bool_0.ToString(GClass101.cultureInfo_0);
    }

    // Token: 0x0600007F RID: 127 RVA: 0x00003844 File Offset: 0x00001A44
    public static Color smethod_14(string string_0)
    {
        string[] array = string_0.Split(new char[]
        {
            ','
        });
        return Color.FromArgb(GClass101.smethod_5(array[3]), GClass101.smethod_5(array[0]), GClass101.smethod_5(array[1]), GClass101.smethod_5(array[2]));
    }

    // Token: 0x06000080 RID: 128 RVA: 0x000025DD File Offset: 0x000007DD
    public static double smethod_15(string string_0)
    {
        return double.Parse(string_0, GClass101.cultureInfo_0);
    }

    // Token: 0x06000081 RID: 129 RVA: 0x000025EA File Offset: 0x000007EA
    public static T ParseEnum<T>(string string_0)
    {
        return (T)((object)Enum.Parse(typeof(T), string_0));
    }

    // Token: 0x04000025 RID: 37
    public static CultureInfo cultureInfo_0 = CultureInfo.GetCultureInfo("en-US");

    // Token: 0x04000026 RID: 38
    public static CultureInfo cultureInfo_1 = CultureInfo.GetCultureInfo("tr-TR");

    // Token: 0x04000027 RID: 39
    public static CultureInfo cultureInfo_2 = CultureInfo.GetCultureInfo("ar-SA");

    // Token: 0x04000028 RID: 40
    public static CultureInfo cultureInfo_3 = CultureInfo.GetCultureInfo("ar-EG");

    // Token: 0x04000029 RID: 41
    public static int int_0 = 10;

    // Token: 0x0400002A RID: 42
    public static int int_1 = 50;
}
