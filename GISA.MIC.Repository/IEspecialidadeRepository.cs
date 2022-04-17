using GISA.Domain.Model;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAF.Repository
{
    public interface IEspecialidadeRepository
    {
        Task<Especialidade> RecuperarPorId(long especialidadeId);

        IList<Especialidade> RecuperarTodas();
    }
}
