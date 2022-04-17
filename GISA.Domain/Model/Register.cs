using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISA.Domain.Model
{
    public abstract class Register
    {
        public long Id { get; set; }

        [Column("issued_by")]
        [Required]
        public long IssuedBy { get; set; }

        [Column("issued_at")]
        [Required]
        public DateTime IssuedAt { get; set; }

        [Column("updated_by")]
        public long? UpdatedBy { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
