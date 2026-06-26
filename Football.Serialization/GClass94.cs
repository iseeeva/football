using System.Drawing;
using System.Numerics;
using System.Xml;

// Token: 0x02000003 RID: 3
public static class GClass94
{
    // Token: 0x06000003 RID: 3 RVA: 0x00002610 File Offset: 0x00000810
    public static T GetEnum<T>(XmlNode node, string attributeName, T defaultValue)
    {
        XmlAttribute xmlAttribute = node.Attributes[attributeName];
        if (xmlAttribute == null)
        {
            return defaultValue;
        }
        return (T)((object)Enum.Parse(typeof(T), xmlAttribute.Value));
    }

    // Token: 0x06000004 RID: 4 RVA: 0x0000264C File Offset: 0x0000084C
    public static string smethod_0(XmlNode xmlNode_0, string string_0, string string_1)
    {
        XmlAttribute xmlAttribute = xmlNode_0.Attributes[string_0];
        if (xmlAttribute == null)
        {
            return string_1;
        }
        return xmlAttribute.Value;
    }

    // Token: 0x06000005 RID: 5 RVA: 0x00002674 File Offset: 0x00000874
    public static int smethod_1(XmlNode xmlNode_0, string string_0, int int_0)
    {
        XmlAttribute xmlAttribute = xmlNode_0.Attributes[string_0];
        if (xmlAttribute == null)
        {
            return int_0;
        }
        return GClass101.smethod_5(xmlAttribute.Value);
    }

    // Token: 0x06000006 RID: 6 RVA: 0x000026A0 File Offset: 0x000008A0
    public static float smethod_2(XmlNode xmlNode_0, string string_0, float float_0)
    {
        XmlAttribute xmlAttribute = xmlNode_0.Attributes[string_0];
        if (xmlAttribute == null)
        {
            return float_0;
        }
        return GClass101.smethod_6(xmlAttribute.Value);
    }

    // Token: 0x06000007 RID: 7 RVA: 0x000026CC File Offset: 0x000008CC
    public static double smethod_3(XmlNode xmlNode_0, string string_0, double double_0)
    {
        XmlAttribute xmlAttribute = xmlNode_0.Attributes[string_0];
        if (xmlAttribute == null)
        {
            return double_0;
        }
        return GClass101.smethod_15(xmlAttribute.Value);
    }

    // Token: 0x06000008 RID: 8 RVA: 0x000026F8 File Offset: 0x000008F8
    public static bool smethod_4(XmlNode xmlNode_0, string string_0, bool bool_0)
    {
        XmlAttribute xmlAttribute = xmlNode_0.Attributes[string_0];
        if (xmlAttribute == null)
        {
            return bool_0;
        }
        return GClass101.smethod_3(xmlAttribute.Value);
    }

    // Token: 0x06000009 RID: 9 RVA: 0x00002724 File Offset: 0x00000924
    public static XmlAttribute smethod_5(XmlDocument xmlDocument_0, XmlNode xmlNode_0, string string_0, object object_0)
    {
        XmlAttribute xmlAttribute = xmlDocument_0.CreateAttribute(string_0);
        if (object_0 is bool)
        {
            xmlAttribute.Value = GClass101.smethod_13((bool)object_0).ToLower();
        }
        else
        {
            xmlAttribute.Value = ((object_0 != null) ? object_0.ToString() : string.Empty);
        }
        xmlNode_0.Attributes.Append(xmlAttribute);
        return xmlAttribute;
    }

    // Token: 0x0600000A RID: 10 RVA: 0x00002780 File Offset: 0x00000980
    public static XmlElement smethod_6(XmlDocument xmlDocument_0, XmlNode xmlNode_0, string string_0)
    {
        XmlElement xmlElement = xmlDocument_0.CreateElement(string_0);
        xmlNode_0.AppendChild(xmlElement);
        return xmlElement;
    }

    // Token: 0x0600000B RID: 11 RVA: 0x000027A0 File Offset: 0x000009A0
    public static XmlComment smethod_7(XmlDocument xmlDocument_0, XmlNode xmlNode_0, string string_0)
    {
        XmlComment xmlComment = xmlDocument_0.CreateComment(string_0);
        xmlNode_0.AppendChild(xmlComment);
        return xmlComment;
    }

    // Token: 0x0600000C RID: 12 RVA: 0x000027C0 File Offset: 0x000009C0
    public static void smethod_8(XmlDocument xmlDocument_0, XmlNode xmlNode_0, Quaternion quaternion_0)
    {
        GClass94.smethod_5(xmlDocument_0, xmlNode_0, "x", GClass101.smethod_7(quaternion_0.X));
        GClass94.smethod_5(xmlDocument_0, xmlNode_0, "y", GClass101.smethod_7(quaternion_0.Y));
        GClass94.smethod_5(xmlDocument_0, xmlNode_0, "z", GClass101.smethod_7(quaternion_0.Z));
        GClass94.smethod_5(xmlDocument_0, xmlNode_0, "w", GClass101.smethod_7(quaternion_0.W));
    }

    // Token: 0x0600000D RID: 13 RVA: 0x00002834 File Offset: 0x00000A34
    public static void smethod_9(XmlDocument xmlDocument_0, XmlNode xmlNode_0, Vector3 vector3_0)
    {
        GClass94.smethod_5(xmlDocument_0, xmlNode_0, "x", GClass101.smethod_7(vector3_0.X));
        GClass94.smethod_5(xmlDocument_0, xmlNode_0, "y", GClass101.smethod_7(vector3_0.Y));
        GClass94.smethod_5(xmlDocument_0, xmlNode_0, "z", GClass101.smethod_7(vector3_0.Z));
    }

    // Token: 0x0600000E RID: 14 RVA: 0x0000288C File Offset: 0x00000A8C
    public static XmlElement smethod_10(XmlDocument xmlDocument_0, XmlNode xmlNode_0, string string_0, Vector3 vector3_0)
    {
        XmlElement xmlElement = GClass94.smethod_6(xmlDocument_0, xmlNode_0, string_0);
        GClass94.smethod_9(xmlDocument_0, xmlElement, vector3_0);
        return xmlElement;
    }

    // Token: 0x0600000F RID: 15 RVA: 0x000028AC File Offset: 0x00000AAC
    public static XmlElement smethod_11(XmlDocument xmlDocument_0, XmlNode xmlNode_0, string string_0, Quaternion quaternion_0)
    {
        XmlElement xmlElement = GClass94.smethod_6(xmlDocument_0, xmlNode_0, string_0);
        GClass94.smethod_8(xmlDocument_0, xmlElement, quaternion_0);
        return xmlElement;
    }

    // Token: 0x06000010 RID: 16 RVA: 0x000028CC File Offset: 0x00000ACC
    public static void smethod_12(XmlDocument xmlDocument_0, XmlNode xmlNode_0, Color color_0)
    {
        GClass94.smethod_5(xmlDocument_0, xmlNode_0, "r", color_0.R);
        GClass94.smethod_5(xmlDocument_0, xmlNode_0, "g", color_0.G);
        GClass94.smethod_5(xmlDocument_0, xmlNode_0, "b", color_0.B);
        GClass94.smethod_5(xmlDocument_0, xmlNode_0, "a", color_0.A);
    }

    // Token: 0x06000011 RID: 17 RVA: 0x00002940 File Offset: 0x00000B40
    public static void smethod_13(XmlDocument xmlDocument_0, XmlNode xmlNode_0, string string_0, Color color_0)
    {
        XmlElement xmlNode_ = GClass94.smethod_6(xmlDocument_0, xmlNode_0, string_0);
        GClass94.smethod_12(xmlDocument_0, xmlNode_, color_0);
    }
}
