using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contract.Infrastructure.Entity.Configurations
{
    public sealed class ApprovedProposalConfiguration : IEntityTypeConfiguration<ApprovedProposalReadModel>
    {
        public void Configure(EntityTypeBuilder<ApprovedProposalReadModel> b)
        {
            b.ToTable("ApprovedProposals");
            b.HasKey(x => x.ProposalId);
            b.Property(x => x.ApprovedAt).IsRequired();
        }
    }
}
