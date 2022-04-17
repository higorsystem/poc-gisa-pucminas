using System.Threading.Tasks;
using GISA.Domain.Model.MIC;

namespace GISA.Domain.Repositories.MIC
{
    public interface IAssociateRepository
    {
        Task<Associate> SaveAsync(Associate associate);

        Task<Associate> GetByIdAsync(long associateId);

        Task<Associate> GetByCpfAsync(string associateCpf);

        Task<Associate> GetByCardNumberAsync(string cardNumber);

        Task<Associate> UpdateAsync(Associate associate);

        Task DeleteAsync(Associate associate);
    }
}
