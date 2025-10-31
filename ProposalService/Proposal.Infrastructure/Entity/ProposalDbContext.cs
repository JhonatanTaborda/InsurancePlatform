using Microsoft.EntityFrameworkCore;

namespace Proposal.Infrastructure.Entity
{
    public sealed class ProposalDbContext : DbContext
    {
        public DbSet<Proposal.Domain.ProposalModel> Proposals => Set<Proposal.Domain.ProposalModel>();
        public ProposalDbContext(DbContextOptions<ProposalDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProposalDbContext).Assembly);
        }
    }
}
