using System;

namespace GISA.Domain.Model.DTO
{
    public class ConsultDto
    {
        public long AssociateId { get; set; }

        public long SpecialtyId { get; set; }

        public long ProviderId { get; set; }

        public DateTime ConsultDate { get; set; }

        public long IssuedBy { get; set; }
    }
}
