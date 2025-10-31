using Microsoft.EntityFrameworkCore;
using Proposal.Application.Interfaces;
using Proposal.Infrastructure.Entity;

namespace Proposal.Infrastructure.Repositories
{
    public sealed class ProposalRepository : IProposalRepository
    {
        private readonly ProposalDbContext _db;
        public ProposalRepository(ProposalDbContext db) => _db = db;

        public async Task Add(Proposal.Domain.ProposalModel entity, CancellationToken ct)
        => await _db.AddAsync(entity, ct);

        public Task<Proposal.Domain.ProposalModel?> Get(Guid id, CancellationToken ct)
        => _db.Proposals.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);

        public async Task<IReadOnlyList<Proposal.Domain.ProposalModel>> List(int skip, int take, CancellationToken ct)
        => await _db.Proposals.AsNoTracking().OrderByDescending(x => x.CreatedAt).Skip(skip).Take(take).ToListAsync(ct);
    }
}
