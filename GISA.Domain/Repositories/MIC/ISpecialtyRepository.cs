using System.Collections.Generic;
using System.Threading.Tasks;
using GISA.Domain.Model;

namespace GISA.Domain.Repositories.MIC
{
    public interface ISpecialtyRepository
    {
        Task<Specialty> GetByIdAsync(long specialtyId);

        Task<IList<Specialty>> GetAllAsync();
    }
}
