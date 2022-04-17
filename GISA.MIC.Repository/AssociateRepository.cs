using System;
using System.Threading.Tasks;
using GISA.Domain.Model.MIC;
using GISA.Domain.Repositories.MIC;
using Microsoft.EntityFrameworkCore;

namespace GISA.MIC.Repository
{
    public class AssociateRepository : IAssociateRepository
    {
        private readonly MICDbContext _context;

        public AssociateRepository(MICDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Associate> SaveAsync(Associate associate)
        {
            return (await _context.Associates.AddAsync(associate)).Entity;
        }

        public async Task<Associate> UpdateAsync(Associate associate)
        {
            var associateUpdated = default(Associate);

            await Task.Run(() => { associateUpdated = _context.Associates.Update(associate).Entity; });

            return await Task.FromResult(associateUpdated);
        }

        public async Task<Associate> GetByIdAsync(long associateId)
        {
            return await _context.Associates.FirstOrDefaultAsync(a => a.Id == associateId);
        }

        public async Task<Associate> GetByCpfAsync(string associateCpf)
        {
            return await _context.Associates.FirstOrDefaultAsync(a => a.CPF == associateCpf);
        }

        public async Task<Associate> GetByCardNumberAsync(string cardNumber)
        {
            return await _context.Associates.FirstOrDefaultAsync(a => a.CardNumber == cardNumber);
        }

        public async Task DeleteAsync(Associate associate)
        {
            await Task.Run(() => { _context.Remove(associate); });
        }
    }
}