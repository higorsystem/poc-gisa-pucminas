using GISA.Domain.Model.DTO;

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISA.Domain.Model
{
    [Table("tb_consult")]
    public class Consult : Register
    {
        public Consult()
        { }

        public Consult(ConsultDto consult, long personId = default)
        {
            AssociateId = consult.AssociateId;
            SpecialtyId = consult.SpecialtyId;
            ProviderId = consult.ProviderId;

            ConsultDate = consult.ConsultDate;
            IssuedBy = personId is default(long) ? AssociateId : personId;
        }

        [Column("associate_id")]
        [ForeignKey("associate_id")]
        public long AssociateId { get; set; }

        [Column("consult_date")]
        public DateTime ConsultDate { get; set; }

        [Column("provider_id")]
        [ForeignKey("provider_id")]
        public long ProviderId { get; set; }

        [Column("specialty_id")]
        [ForeignKey("specialty_id")]
        public long SpecialtyId { get; set; }

        [Column("status")]
        public EConsultStatus Status { get; set; }
    }
}