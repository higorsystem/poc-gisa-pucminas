using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GISA.Domain.Model.MIC;

namespace GISA.Domain.Model
{
    [Table("tb_specialty")]
    public class Specialty : Register
    {
        public Specialty()
        {
        }

        public Specialty(long id, string name, long issuedBy, DateTime issuedAt)
        {
            Id = id;
            Name = name;
            IssuedBy = issuedBy;
            IssuedAt = issuedAt;
        }

        [Column("name")]
        [Required]
        [StringLength(64, ErrorMessage = "Especialidade inválida", MinimumLength = 8)]
        public string Name { get; set; }

        public ICollection<Provider> Providers { get; set; }
    }
}