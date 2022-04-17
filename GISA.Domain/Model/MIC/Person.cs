using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISA.Domain.Model.MIC
{
    public abstract class Person : Register
    {
        [ForeignKey("address_id")]
        public Address Address { get; set; }

        [Column("birth_date")]
        public DateTime BirthDate { get; set; }

        [Column("cpf")]
        [Required]
        [StringLength(11, ErrorMessage = "CPF inválido", MinimumLength = 11)]
        public string CPF { get; set; }

        public Email Email { get; set; }

        [Column("genre")]
        public EGenderPerson Genre { get; set; }

        [Column("name")]
        [Required]
        [StringLength(128, ErrorMessage = "Name inválido", MinimumLength = 32)]
        public string Name { get; set; }

        [Column("person_type")]
        public EPersonType PersonType { get; set; }

        [Column("rg")]
        [Required]
        [StringLength(11, ErrorMessage = "RG inválido", MinimumLength = 5)]
        public string RG { get; set; }

        [Column("social_name")]
        public string SocialName { get; set; }
    }
}