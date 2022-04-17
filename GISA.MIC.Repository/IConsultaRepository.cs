using GISA.Domain.Model;

using System.Threading.Tasks;

namespace SAF.Repository
{
    public interface IConsultaRepository
    {
        Task<Consulta> Incluir(Consulta consulta);

        Task<Consulta> RecuperarPorId(long consultaId);
    }
}
