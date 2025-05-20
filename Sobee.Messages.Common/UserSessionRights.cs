using System.Collections;
using System.Reflection;
using System.Text;
using Sobee.Messaging;

namespace Sobee.Messages.Common
{
    // Token: 0x0200001E RID: 30
    [GAttribute0(18579)]
    public class UserSessionRights : Message
    {
        // Token: 0x0600000D RID: 13 RVA: 0x00005BCC File Offset: 0x00003DCC
        public UserSessionRights()
        {
            Clear();
        }

        // Token: 0x0600000E RID: 14 RVA: 0x00005C1C File Offset: 0x00003E1C
        public UserSessionRights(BinaryReader gclass315_0)
        {
            int num = gclass315_0.method_9();
            for (int i = 0; i < num; i++)
            {
                string fieldName = gclass315_0.method_14();
                string fieldValue = gclass315_0.method_14();
                SetUserSessionRight(fieldName, fieldValue, true);
            }
        }

        // Token: 0x0600000F RID: 15 RVA: 0x00005C90 File Offset: 0x00003E90
        public void Clear()
        {
            gclass96_0 = new GClass96(0);
            gclass96_1 = new GClass96(10);
            CanWriteChat = false;
            CanSendComplaint = false;
            CanVote = false;
            CanStartVoting = false;
            CanSendGMCommand = false;
            CanBanPlayers = false;
            CanSilencePlayers = false;
            CanKickPlayers = false;
            IsSilenced = false;
            CanSeeFilteredChat = false;
            CanSeeMatchDebugStats = false;
            CanSetGameProperties = false;
            CanChangeAppearance = false;
            IsHiddenAdmin = false;
            AvailableForeheads = new List<int>();
            VisibleForeheads = new List<int>();
            AvailableNoses = new List<int>();
            VisibleNoses = new List<int>();
            AvailableMouths = new List<int>();
            VisibleMouths = new List<int>();
            AvailableEyes = new List<int>();
            VisibleEyes = new List<int>();
            AvailableEyeColors = new List<int>();
            VisibleEyeColors = new List<int>();
            AvailableHairColors = new List<int>();
            VisibleHairColors = new List<int>();
            AvailableHairs = new List<int>();
            VisibleHairs = new List<int>();
            gclass103_0 = new GClass103(0f);
            float_0 = 1f;
        }

        // Token: 0x06000010 RID: 16 RVA: 0x00005DD0 File Offset: 0x00003FD0
        public override void Deserialize(BinaryWriter archive)
        {
            Type typeFromHandle = typeof(UserSessionRights);
            FieldInfo[] fields = typeFromHandle.GetFields();
            archive.method_9(fields.Length);
            foreach (FieldInfo fieldInfo in fields)
            {
                archive.method_14(fieldInfo.Name);
                archive.method_14(GetString(fieldInfo.GetValue(this), fieldInfo.FieldType));
            }
        }

        // Token: 0x06000011 RID: 17 RVA: 0x00005E38 File Offset: 0x00004038
        public void SetUserSessionRight(string fieldName, string fieldValue, bool createNew)
        {
            Type typeFromHandle = typeof(UserSessionRights);
            FieldInfo field = typeFromHandle.GetField(fieldName);
            if (field != null)
            {
                object value = GetValue(fieldValue, field.FieldType);
                if (createNew)
                {
                    field.SetValue(this, value);
                    return;
                }
                Type fieldType = field.FieldType;
                object value2 = field.GetValue(this);
                if (value2 is GClass103)
                {
                    ((GClass103)value2).float_0 += ((GClass103)value).float_0;
                    return;
                }
                if (value2 is GClass96)
                {
                    ((GClass96)value2).int_0 += ((GClass96)value).int_0;
                    return;
                }
                if (value2 is IList)
                {
                    IList list = (IList)value;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (!((IList)value2).Contains(list[i]))
                        {
                            ((IList)value2).Add(list[i]);
                        }
                    }
                    return;
                }
                field.SetValue(this, value);
            }
        }

        // Token: 0x06000012 RID: 18 RVA: 0x00005F30 File Offset: 0x00004130
        private object GetValue(string value, Type valueType)
        {
            if (valueType == typeof(GClass96))
            {
                return new GClass96(GClass101.smethod_5(value));
            }
            if (valueType == typeof(GClass103))
            {
                return new GClass103(GClass101.smethod_6(value));
            }
            if (valueType == typeof(bool))
            {
                return GClass101.smethod_3(value);
            }
            if (valueType == typeof(float))
            {
                return GClass101.smethod_6(value);
            }
            if (valueType == typeof(List<int>))
            {
                string[] array = value.Split(new char[]
                {
                    ','
                });
                List<int> list = new List<int>();
                for (int i = 0; i < array.Length; i++)
                {
                    if (!string.IsNullOrEmpty(array[i]))
                    {
                        list.Add(GClass101.smethod_5(array[i]));
                    }
                }
                return list;
            }
            return null;
        }

        // Token: 0x06000013 RID: 19 RVA: 0x00005FF4 File Offset: 0x000041F4
        private string GetString(object value, Type valueType)
        {
            if (valueType == typeof(GClass96))
            {
                return GClass101.smethod_9(((GClass96)value).int_0);
            }
            if (valueType == typeof(GClass103))
            {
                return GClass101.smethod_7(((GClass103)value).float_0);
            }
            if (valueType == typeof(float))
            {
                return GClass101.smethod_7((float)value);
            }
            if (valueType == typeof(bool))
            {
                return GClass101.smethod_13((bool)value);
            }
            if (valueType == typeof(List<int>))
            {
                List<int> list = (List<int>)value;
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < list.Count; i++)
                {
                    stringBuilder.Append(list[i]);
                    if (i != list.Count - 1)
                    {
                        stringBuilder.Append(",");
                    }
                }
                return stringBuilder.ToString();
            }
            return null;
        }

        // Token: 0x04000421 RID: 1057
        public bool CanWriteChat;

        // Token: 0x04000422 RID: 1058
        public bool CanSendComplaint;

        // Token: 0x04000423 RID: 1059
        public bool CanVote;

        // Token: 0x04000424 RID: 1060
        public bool CanStartVoting;

        // Token: 0x04000425 RID: 1061
        public bool CanSendGMCommand;

        // Token: 0x04000426 RID: 1062
        public bool CanBanPlayers;

        // Token: 0x04000427 RID: 1063
        public bool CanSilencePlayers;

        // Token: 0x04000428 RID: 1064
        public bool CanKickPlayers;

        // Token: 0x04000429 RID: 1065
        public bool IsSilenced;

        // Token: 0x0400042A RID: 1066
        public bool CanSeeFilteredChat;

        // Token: 0x0400042B RID: 1067
        public bool CanSeeMatchDebugStats;

        // Token: 0x0400042C RID: 1068
        public bool CanSetGameProperties;

        // Token: 0x0400042D RID: 1069
        public bool CanChangeAppearance;

        // Token: 0x0400042E RID: 1070
        public bool IsHiddenAdmin;

        // Token: 0x0400042F RID: 1071
        public GClass96 gclass96_0 = new GClass96(0);

        // Token: 0x04000430 RID: 1072
        public GClass96 gclass96_1 = new GClass96(10);

        // Token: 0x04000431 RID: 1073
        public List<int> AvailableForeheads;

        // Token: 0x04000432 RID: 1074
        public List<int> VisibleForeheads;

        // Token: 0x04000433 RID: 1075
        public List<int> AvailableNoses;

        // Token: 0x04000434 RID: 1076
        public List<int> VisibleNoses;

        // Token: 0x04000435 RID: 1077
        public List<int> AvailableMouths;

        // Token: 0x04000436 RID: 1078
        public List<int> VisibleMouths;

        // Token: 0x04000437 RID: 1079
        public List<int> AvailableEyes;

        // Token: 0x04000438 RID: 1080
        public List<int> VisibleEyes;

        // Token: 0x04000439 RID: 1081
        public List<int> AvailableEyeColors;

        // Token: 0x0400043A RID: 1082
        public List<int> VisibleEyeColors;

        // Token: 0x0400043B RID: 1083
        public List<int> AvailableHairs;

        // Token: 0x0400043C RID: 1084
        public List<int> VisibleHairs;

        // Token: 0x0400043D RID: 1085
        public List<int> AvailableHairColors;

        // Token: 0x0400043E RID: 1086
        public List<int> VisibleHairColors;

        // Token: 0x0400043F RID: 1087
        public GClass103 gclass103_0 = new GClass103(0f);

        // Token: 0x04000440 RID: 1088
        public float float_0 = 1f;
    }
}
