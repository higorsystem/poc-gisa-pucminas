using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISA.Domain.Model
{
    //[Table("tb_address")]
    public class Address
    {
        public Address(string area, string complement, int number, string zipCode, string city, string state)
        {
            Area = area;
            Complement = complement;
            Number = number;
            ZipCode = zipCode;
            City = city;
            State = state;
        }

        [Column("area")]
        [Required]
        [StringLength(128, ErrorMessage = "Bairro inválido", MinimumLength = 8)]
        public string Area { get; set; }

        [Column("city")]
        [Required]
        [StringLength(128, ErrorMessage = "Cidade inválida", MinimumLength = 4)]
        public string City { get; set; }

        [Column("complement")]
        [StringLength(128, ErrorMessage = "Complemento inválido", MinimumLength = 0)]
        public string Complement { get; set; }

        [Column("number")] [Required] public int Number { get; set; }

        [Column("state")]
        [Required]
        [StringLength(16, ErrorMessage = "Estado inválido", MinimumLength = 8)]
        public string State { get; set; }

        [Column("street")]
        [Required]
        [StringLength(128, ErrorMessage = "Logradouro inválido", MinimumLength = 8)]
        public string Street { get; set; }

        [Column("zipcode")]
        [Required]
        [StringLength(8, ErrorMessage = "ZipCode inválido", MinimumLength = 8)]
        public string ZipCode { get; set; }
    }
}