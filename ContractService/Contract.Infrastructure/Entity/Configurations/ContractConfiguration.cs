using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contract.Infrastructure.Entity.Configurations
{
    public sealed class ContractConfiguration : IEntityTypeConfiguration<Contract.Domain.ContractModel>
    {
        public void Configure(EntityTypeBuilder<Contract.Domain.ContractModel> b)
        {
            b.ToTable("Contracts");
            b.HasKey(x => x.Id);
            b.Property(x => x.ContractedAt).IsRequired();
            b.Property(x => x.ProposalId).IsRequired();
            b.HasIndex(x => x.ProposalId).IsUnique();
        }
    }
}
