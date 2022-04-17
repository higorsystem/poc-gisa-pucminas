using System.Collections.Generic;
using System.Threading.Tasks;
using GISA.Domain.Model;

namespace GISA.Domain.Repositories.MIC
{
    public interface IProcedureRepository
    {
        Task<IList<Procedure>> GetAllProcedureAsync();
    }
}
