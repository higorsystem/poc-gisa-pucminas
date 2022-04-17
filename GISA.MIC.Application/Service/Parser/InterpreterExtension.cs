using System.Collections.Generic;
using System.Linq;
using GISA.Domain.Model;
using GISA.Domain.Model.DTO;

namespace GISA.MIC.Application.Service.Parser
{
    public static class InterpreterExtension
    {
        public static ConsultDto ToConsultDto(this Consult bo)
        {
            return new ConsultDto
            {
                IssuedBy = bo.IssuedBy,
                AssociateId = bo.AssociateId,
                ProviderId = bo.ProviderId,
                ConsultDate = bo.ConsultDate,
                SpecialtyId = bo.SpecialtyId
            };
        }

        public static IList<ConsultDto> ToListConsultDto(this IList<Consult> bo)
        {
            return bo.Select(ToConsultDto).ToList();
        }

        public static Consult ToConsultBo(this ConsultDto dto, long personId = default)
        {
            return new Consult(dto, personId);
        }
    }
}