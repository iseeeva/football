using System.ComponentModel;

namespace Football.Network
{
    [TypeConverter(typeof(EnumConverter))]
    public enum SessionType
    {
        Authentication = 1,
        Main,
        Patcher,
        Chat,
        Game,
        User,
        Admin
    }
}
