using GISA.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GISA.MIC.Repository.Configuration
{
    public class CovenantConfiguration : IEntityTypeConfiguration<Covenant>
    {
        public void Configure(EntityTypeBuilder<Covenant> builder)
        {
            builder.OwnsOne(e => e.Email);
            builder.OwnsOne(e => e.Address);
        }
    }
}
