using System.Collections.Generic;
using System.Threading.Tasks;
using GISA.Domain.Model.DTO;
using GISA.MIC.Application.Helper;

namespace GISA.MIC.Application.Service
{
    public interface IConsultService
    {
        Task<Response<ConsultDto>> SaveConsultScheduleAsync(ConsultDto request, long personId = default);
        Task<Response<IList<ConsultDto>>> CheckStatusByProviderAsync(string checkStatus, long personId);
    }
}