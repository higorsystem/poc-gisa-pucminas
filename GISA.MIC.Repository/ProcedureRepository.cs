using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GISA.Domain.Model;
using GISA.Domain.Repositories.MIC;
using Microsoft.EntityFrameworkCore;

namespace GISA.MIC.Repository
{
    public class ProcedureRepository : IProcedureRepository
    {
        private readonly MICDbContext _context;

        public ProcedureRepository(MICDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IList<Procedure>> GetAllProcedureAsync()
        {
            return await _context.Procedures.ToListAsync();
        }
    }
}
