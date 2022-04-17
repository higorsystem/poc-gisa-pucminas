using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GISA.Domain.Model.MIC;
using GISA.Domain.Repositories.MIC;
using Microsoft.EntityFrameworkCore;

namespace GISA.MIC.Repository
{
    public class ProviderRepository : IProviderRepository
    {
        private readonly MICDbContext _context;

        public ProviderRepository(MICDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task<Provider> GetByIdAsync(long providerId)
        {
            return _context.Providers.FirstOrDefaultAsync(a => a.Id == providerId);
        }

        public async Task<IList<Provider>> GetByStateAsync(string providerState)
        {
            return await _context.Providers.Where(p => p.Address.State == providerState).ToListAsync();
        }

        public async Task<IList<Provider>> GetProviderBySpecialtyIdAsync(long specialtyId)
        {
            return await _context.Providers.Where(p => p.SpecialtyCollection.Any(e => e.Id == specialtyId)).ToListAsync();
        }
    }
}