using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISA.Domain.Model
{
    [Table("tb_health_insurance")]
    public class HealthInsurance : Register
    {
        public HealthInsurance()
        {
        }

        public HealthInsurance(long id, string ansNumber, ECategoryPlan categoryPlan, long issuedBy, DateTime issuedAt, string commercialName, bool dentalPlan)
        {
            Id = id;
            AnsNumber = ansNumber;
            CategoryPlan = categoryPlan;
            IssuedBy = issuedBy;
            IssuedAt = issuedAt;
            CommercialName = commercialName;
            DentalPlan = dentalPlan;
        }

        /// <summary>
        ///     Número de registro do HealthInsurance junto à Agência Nacional de Saúde Suplementar
        /// </summary>
        [Column("ans_number")]
        [Required]
        [StringLength(8, ErrorMessage = "Número ANS inválido", MinimumLength = 6)]
        public string AnsNumber { get; set; }

        [Column("category_plan")] public ECategoryPlan CategoryPlan { get; set; }

        [Column("commercial_name")]
        [Required]
        [StringLength(128, ErrorMessage = "Name comercial inválido", MinimumLength = 16)]
        public string CommercialName { get; set; }

        /// <summary>
        ///     Contempla HealthInsurance Odontológico?
        /// </summary>
        [Column("dental_plan")]
        [Required]
        public bool DentalPlan { get; set; }
    }
}