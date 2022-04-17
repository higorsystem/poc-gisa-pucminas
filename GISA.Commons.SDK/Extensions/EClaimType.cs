using System.ComponentModel;

namespace GISA.Commons.SDK.Extensions
{
    public enum EClaimType
    {
        [Description("custom:person_id")]
        Person,

        [Description("custom:user_type")]
        UserType
    }

    public enum EClaimRoleType
    {
        [Description("Colaborador")]
        Provider,

        [Description("Associado")]
        Associate,

        [Description("Admin")]
        Admin
    }
}