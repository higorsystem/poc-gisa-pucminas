using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISA.Domain.Model
{
    [Table("tb_procedure")]
    public class Procedure : Register
    {
        public Procedure()
        {
        }

        public Procedure(long id, string tussCode, string name, EProcedureSector sector, long issuedBy,
            DateTime issuedAt)
        {
            Id = id;
            TUSSCode = tussCode;
            Name = name;
            Sector = sector;

            IssuedBy = issuedBy;
            IssuedAt = issuedAt;
        }

        [Column("name")]
        [Required]
        [StringLength(64, ErrorMessage = "Procedimento inválido", MinimumLength = 16)]
        public string Name { get; set; }

        [Column("sector")] [Required] public EProcedureSector Sector { get; set; }

        /// <summary>
        ///     Código do procedimento padronizado pela ANS
        ///     http://www.ans.gov.br/images/stories/Legislacao/in/anexo_in34_dides.pdf
        /// </summary>
        [Column("tuss_code")]
        [Required]
        [StringLength(16, ErrorMessage = "Código TUSS inválido", MinimumLength = 8)]
        public string TUSSCode { get; set; }
    }
}