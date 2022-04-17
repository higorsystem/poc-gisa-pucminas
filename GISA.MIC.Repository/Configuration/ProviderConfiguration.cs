using GISA.Domain.Model;
using GISA.Domain.Model.MIC;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GISA.MIC.Repository.Configuration
{
    public class ProviderConfiguration : BaseConfiguration, IEntityTypeConfiguration<Provider>
    {
        public ProviderConfiguration() : base(EPersonType.Colaborador) { }

        public void Configure(EntityTypeBuilder<Provider> builder)
        {
            builder.OwnsOne(e => e.Email).HasData(Emails.ToArray());
            builder.OwnsOne(e => e.Address).HasData(Address.ToArray());
        }
    }
}
