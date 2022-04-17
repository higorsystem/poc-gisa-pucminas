using System.ComponentModel;

namespace GISA.Domain.Model
{
    public enum EGenderPerson
    {
        Feminino,
        Masculino
    }

    public enum EPersonType
    {
        [Description("Associado")]
        Associado = 1,

        [Description("Colaborador")]
        Colaborador = 2,
    }

    public enum EContractType
    {
        Empresarial,
        Individual
    }

    public enum EPlanStatus
    {
        Inativo = 0,
        Ativo = 1,
        Suspenso = 2
    }

    public enum ECategoryPlan
    {
        Apartamento,
        Enfermaria,
        VIP
    }

    public enum ECovenantType
    {
        Clínica,
        Laboratório,
        Hospital
    }

    public enum EProviderQualification
    {
        Doutorado,
        Especialista,
        Mestrado,
        PósDoutorado,
        PosGraduacao,
        Residência
    }

    public enum EProcedureSector
    {
        Punção,
        UltraSom,
        Ressonância,
        Tomografia,
        Densitometria,
        Doppler,
        Mamografia
    }

    public enum EConsultStatus
    {
        Criada,
        Agendada,
        Rejeitada,
        Realizada
    }
}