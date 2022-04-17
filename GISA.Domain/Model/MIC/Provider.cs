using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISA.Domain.Model.MIC
{
    [Table("tb_provider")]
    public class Provider : Person
    {
        public ICollection<Specialty> SpecialtyCollection { get; set; }

        [Column("qualification")]
        public EProviderQualification Qualification { get; set; }
    }
}
