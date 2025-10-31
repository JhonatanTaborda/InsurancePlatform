using Microsoft.EntityFrameworkCore;
using Contract.Domain;

namespace Contract.Infrastructure.Entity
{
    public sealed class ContractDbContext : DbContext
    {
        public ContractDbContext(DbContextOptions<ContractDbContext> options) : base(options) { }

        public DbSet<Contract.Domain.ContractModel> Contracts => Set<Contract.Domain.ContractModel>();
        public DbSet<ApprovedProposalReadModel> ApprovedProposals => Set<ApprovedProposalReadModel>();            
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContractDbContext).Assembly);
        }
    }

    public sealed class ApprovedProposalReadModel
    {
        public Guid ProposalId { get; set; }
        public DateTime ApprovedAt { get; set; }
    }
}
