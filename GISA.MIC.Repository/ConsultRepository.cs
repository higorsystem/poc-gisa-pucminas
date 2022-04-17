using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GISA.Domain.Model;
using GISA.Domain.Repositories.MIC;
using Microsoft.EntityFrameworkCore;

namespace GISA.MIC.Repository
{
    public class ConsultRepository : IConsultRepository
    {
        private readonly MICDbContext _context;

        public ConsultRepository(MICDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IList<Consult>> CheckStatusByProviderAsync(string consultStatus, long providerId)
        {
            return await _context.Consults.Where(a =>
                a.Status == Enum.Parse<EConsultStatus>(consultStatus) &&
                a.ProviderId == providerId).ToListAsync();
        }

        public async Task<Consult> GetByIdAsync(long consultId)
        {
            return await _context.Consults.FirstOrDefaultAsync(a => a.Id == consultId);
        }

        public async Task<Consult> SaveAsync(Consult consult)
        {
            consult.IssuedAt = DateTime.UtcNow;
            consult.Status = EConsultStatus.Criada;

            var result = (await _context.Consults.AddAsync(consult)).Entity;

            await _context.SaveChangesAsync();

            return result;
        }
    }
}