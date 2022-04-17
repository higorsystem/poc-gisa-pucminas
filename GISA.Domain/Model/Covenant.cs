using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISA.Domain.Model
{
    [Table("tb_covenant")]
    public class Covenant : Register
    {
        [Column("address")]
        public Address Address { get; set; }

        [Column("email_address")]
        public Email Email { get; set; }

        [Column("member_type")]
        public ECovenantType MemberType { get; set; }

        [Column("name")]
        [Required]
        [StringLength(128, ErrorMessage = "Name inválido", MinimumLength = 32)]
        public string Name { get; set; }
    }
}