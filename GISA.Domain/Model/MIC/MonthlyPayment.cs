using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISA.Domain.Model.MIC
{
    [Table("tb_monthly_payment")]
    public class MonthlyPayment : Register
    {
        [Column("monthly_reference")]
        [Required]
        public short MonthtlyReference { get; set; }

        [Column("due_date")]
        [Required]
        public DateTime DueDate { get; set; }

        [Column("payment_amount")]
        [Required]
        [DataType(DataType.Currency)]
        public decimal PaymentAmount { get; set; }
    }
}
