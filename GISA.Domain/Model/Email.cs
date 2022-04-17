using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISA.Domain.Model
{
    public class Email
    {
        public Email(string emailAddress)
        {
            EmailAddress = emailAddress;
        }

        [Column("email_address")]
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string EmailAddress { get; set; }
    }
}