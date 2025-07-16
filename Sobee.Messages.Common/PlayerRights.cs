using System.Collections;
using System.Reflection;
using System.Text;
using Sobee.Messaging;

namespace Sobee.Messages.Common
{
    // Token: 0x02000014 RID: 20
    [GAttribute0(13602)]
    public class PlayerRights : Message
    {
        // Token: 0x06000006 RID: 6 RVA: 0x000021F4 File Offset: 0x000003F4
        public PlayerRights()
        {
            Clear();
        }

        // Token: 0x06000007 RID: 7 RVA: 0x00005980 File Offset: 0x00003B80
        public PlayerRights(BinaryReader gclass315_0)
        {
            int num = gclass315_0.method_9();
            for (int i = 0; i < num; i++)
            {
                string fieldName = gclass315_0.method_14();
                string fieldValue = gclass315_0.method_14();
                SetPlayerRight(fieldName, fieldValue, true);
            }
        }

        // Token: 0x06000008 RID: 8 RVA: 0x00002202 File Offset: 0x00000402
        public void Clear()
        {
            UseBrazilCycleKick = false;
            UseBonusHeadFly = false;
            AvailableCheers = new List<int>();
            list_0 = new List<int>();
        }

        // Token: 0x06000009 RID: 9 RVA: 0x000059C0 File Offset: 0x00003BC0
        public override void Serialize(BinaryWriter archive)
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

        // Token: 0x0600000A RID: 10 RVA: 0x00005A28 File Offset: 0x00003C28
        public void SetPlayerRight(string fieldName, string fieldValue, bool createNew)
        {
            Type typeFromHandle = typeof(PlayerRights);
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

        // Token: 0x0600000B RID: 11 RVA: 0x00005AD4 File Offset: 0x00003CD4
        private object GetValue(string value, Type valueType)
        {
            if (valueType == typeof(bool))
            {
                return GClass101.smethod_3(value);
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

        // Token: 0x0600000C RID: 12 RVA: 0x00005B4C File Offset: 0x00003D4C
        private string GetString(object value, Type valueType)
        {
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

        // Token: 0x040003E9 RID: 1001
        public bool UseBrazilCycleKick;

        // Token: 0x040003EA RID: 1002
        public bool UseBonusHeadFly;

        // Token: 0x040003EB RID: 1003
        public List<int> AvailableCheers;

        // Token: 0x040003EC RID: 1004
        public List<int> list_0;
    }
}
