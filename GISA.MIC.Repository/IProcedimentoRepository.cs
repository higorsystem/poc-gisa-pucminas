using GISA.Domain.Model;

using System.Collections.Generic;

namespace SAF.Repository
{
    public interface IProcedimentoRepository
    {
        IList<Procedimento> RecuperarTodos();
    }
}
