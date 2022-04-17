using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISA.Domain.Model.MIC
{
    [Table("tb_associate")]
    public class Associate : Person
    {
        /// <summary>
        ///     Representa o identificador do titular do plano de saúde.
        /// </summary>
        [Column("holder_id")]
        public long? HolderId { get; set; }

        [Column("card_number")]
        [StringLength(16, ErrorMessage = "Número de carteirinha inválido", MinimumLength = 16)]
        public string CardNumber { get; set; }

        public List<MonthlyPayment> MonthlyPayments { get; set; }

        [Column("inserted_at")]
        [Required]
        public DateTime JoiningDate { get; set; }

        [Column("plan_status")]
        [Required]
        [DataType(DataType.Text)]
        public EPlanStatus PlanStatus { get; set; }

        [Required]
        public HealthInsurance HealthInsurance { get; set; }

        [Column("plan_type")]
        [Required]
        public EContractType PlanType { get; set; }

        [Column("last_location")]
        public string LastLocation { get; set; }
    }
}
