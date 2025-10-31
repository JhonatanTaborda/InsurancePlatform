using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Proposal.Infrastructure.Entity.Configurations
{
    public sealed class ProposalConfiguration : IEntityTypeConfiguration<Proposal.Domain.ProposalModel>
    {
        public void Configure(EntityTypeBuilder<Proposal.Domain.ProposalModel> b)
        {
            b.ToTable("Proposals");
            b.HasKey(x => x.Id);
            b.Property(x => x.Product).HasMaxLength(120).IsRequired();
            b.Property(x => x.Premium).HasColumnType("decimal(18,2)");
            b.Property(x => x.Status).HasConversion<int>();
            b.Property(x => x.CreatedAt);
            b.Property(x => x.CustomerId).IsRequired();
            b.HasIndex(x => x.Status);
        }
    }
}
