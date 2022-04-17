using System.Collections.Generic;
using System.Threading.Tasks;
using GISA.Domain.Model;
using GISA.Domain.Model.MIC;
using GISA.MIC.Application.Helper;

namespace GISA.MIC.Application.Service
{
    public interface ISpecialtyService
    {
        Task<Response<IList<Specialty>>> GetAllAsync();
        Task<Response<Specialty>> GetByIdAsync(long specialtyId);
    }
}