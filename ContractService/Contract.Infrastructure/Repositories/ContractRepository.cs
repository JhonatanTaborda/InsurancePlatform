using Contract.Application.Interfaces;
using Contract.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;

namespace Contract.Infrastructure.Repositories
{
    public sealed class ContractRepository : IContractRepository
    {
        private readonly ContractDbContext _db;
        public ContractRepository(ContractDbContext db) => _db = db;
        public async Task Add(Contract.Domain.ContractModel entity, CancellationToken ct) => await _db.AddAsync(entity, ct);
        public Task<bool> ExistsForProposal(Guid proposalId, CancellationToken ct) => _db.Contracts.AnyAsync(x => x.ProposalId == proposalId, ct);
    }
}
