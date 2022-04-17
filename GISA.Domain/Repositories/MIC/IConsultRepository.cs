using System.Collections.Generic;
using System.Threading.Tasks;
using GISA.Domain.Model;

namespace GISA.Domain.Repositories.MIC
{
    public interface IConsultRepository
    {
        Task<Consult> SaveAsync(Consult consult);

        Task<Consult> GetByIdAsync(long consultId);

        Task<IList<Consult>> CheckStatusByProviderAsync(string consultStatus, long providerId);
    }
}
