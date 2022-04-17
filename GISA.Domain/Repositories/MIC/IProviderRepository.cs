using System.Collections.Generic;
using System.Threading.Tasks;
using GISA.Domain.Model.MIC;

namespace GISA.Domain.Repositories.MIC
{
    public interface IProviderRepository
    {
        Task<Provider> GetByIdAsync(long providerId);

        Task<IList<Provider>> GetByStateAsync(string providerState);

        Task<IList<Provider>> GetProviderBySpecialtyIdAsync(long specialtyId);
    }
}