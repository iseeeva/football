using System.Collections;
using System.Reflection;
using Sobee.Messaging;
using Sobee.Serialization;

namespace Sobee.TestServer.Messages
{
    [MessageAttribute(13602)]
    public class PlayerRights : Message
    {
        public bool UseBrazilCycleKick { get; set; }
        public bool UseBonusHeadFly { get; set; }
        public List<int> AvailableCheers { get; private set; } = new List<int>();

        public PlayerRights()
        {
            UseBrazilCycleKick = false;
            UseBonusHeadFly = false;
            AvailableCheers = new List<int>();
        }

        public PlayerRights(BinaryReader reader)
        {
            int fieldCount = reader.method_9();
            for (int i = 0; i < fieldCount; i++)
            {
                string fieldName = reader.method_14();
                string fieldValue = reader.method_14();
                SetPlayerRight(fieldName, fieldValue, overwrite: true);
            }
        }

        public override void Serialize(BinaryWriter archive)
        {
            var fields = typeof(PlayerRights).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            archive.method_9(fields.Length);

            foreach (FieldInfo field in fields)
            {
                archive.method_14(field.Name);
                archive.method_14(ConvertToString(field.GetValue(this), field.FieldType));
            }
        }

        public void SetPlayerRight(string fieldName, string fieldValue, bool overwrite)
        {
            var field = typeof(PlayerRights).GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (field == null) return;

            object? newValue = ConvertFromString(fieldValue, field.FieldType);
            if (newValue == null) return;

            if (overwrite)
            {
                field.SetValue(this, newValue);
                return;
            }

            if (field.GetValue(this) is IList targetList && newValue is IList sourceList)
            {
                foreach (var item in sourceList)
                {
                    if (!targetList.Contains(item))
                        targetList.Add(item);
                }
            }
            else
            {
                field.SetValue(this, newValue);
            }
        }

        private static object? ConvertFromString(string value, Type type)
        {
            if (type == typeof(bool))
                return GClass101.smethod_3(value);

            if (type == typeof(List<int>))
            {
                var result = new List<int>();
                foreach (var part in value.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    result.Add(GClass101.smethod_5(part));
                }
                return result;
            }

            return null;
        }

        private static string? ConvertToString(object value, Type type)
        {
            if (type == typeof(bool))
                return GClass101.smethod_13((bool)value);

            if (type == typeof(List<int>) && value is List<int> list)
                return string.Join(",", list);

            return null;
        }
    }
}
