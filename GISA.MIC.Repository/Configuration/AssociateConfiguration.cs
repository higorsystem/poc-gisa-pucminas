using GISA.Domain.Model;
using GISA.Domain.Model.MIC;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GISA.MIC.Repository.Configuration
{
    public class AssociateConfiguration : BaseConfiguration, IEntityTypeConfiguration<Associate>
    {
        public AssociateConfiguration() : base(EPersonType.Associado) { }

        public void Configure(EntityTypeBuilder<Associate> builder)
        {
            builder.OwnsOne(e => e.Email).HasData(Emails.ToArray());
            builder.OwnsOne(e => e.Address).HasData(Address.ToArray());
        }
    }
}
