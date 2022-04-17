using System.Collections.Generic;
using System.Threading.Tasks;
using GISA.Domain.Model.MIC;
using GISA.MIC.Application.Helper;

namespace GISA.MIC.Application.Service
{
    public interface IProviderService
    {
        Task<Response<Provider>> GetByIdAsync(long providerId);
        Task<Response<IList<Provider>>> GetByStateAsync(string state);
        Task<Response<IList<Provider>>> GetProviderBySpecialtyIdAsync(long specialtyId);
    }
}