using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GISA.Domain.Model;
using GISA.Domain.Repositories.MIC;
using Microsoft.EntityFrameworkCore;

namespace GISA.MIC.Repository
{
    public class SpecialtyRepository : ISpecialtyRepository
    {
        private readonly MICDbContext _context;

        public SpecialtyRepository(MICDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IList<Specialty>> GetAllAsync()
        {
            return await _context.Specialties.ToListAsync();
        }

        public async Task<Specialty> GetByIdAsync(long specialtyId)
        {
            return await _context.Specialties.FirstOrDefaultAsync(e => e.Id == specialtyId);
        }
    }
}