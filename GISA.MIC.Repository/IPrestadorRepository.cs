using GISA.Domain.Model;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAF.Repository
{
    public interface IPrestadorRepository
    {
        Task<Prestador> RecuperarPorId(long prestadorId);

        IList<Prestador> RecuperarPorEspecialidade(long especialidadeId);

        IList<Prestador> RecuperarPorEstado(string estadoPrestador);
    }
}
