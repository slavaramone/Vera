using System.ComponentModel;

namespace Domain.Enums
{
    public enum UserRole
    {
        [Description("Администратор")]
        Admin = 0,

        [Description("Врач")]
        Medic = 1,

        [Description("Социальный работник")]
        SocialWorker = 2,

        [Description("Пациент")]
        Patient = 3,

        [Description("Родственник")]
        Relative = 4
    }
}
