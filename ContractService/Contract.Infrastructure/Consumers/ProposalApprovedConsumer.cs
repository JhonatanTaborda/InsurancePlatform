using Contract.Infrastructure.Entity;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Contract;
using static Shared.Contract.ProposalEvents;

namespace Contract.Infrastructure.Consumers
{
    public sealed class ProposalApprovedConsumer : IConsumer<ProposalApproved>
    {
        private readonly ContractDbContext _db;
        public ProposalApprovedConsumer(ContractDbContext db) => _db = db;


        public async Task Consume(ConsumeContext<ProposalApproved> context)
        {
            var msg = context.Message;
            var exists = await _db.ApprovedProposals.AnyAsync(x => x.ProposalId == msg.ProposalId);

            if (exists) 
                return;

            _db.ApprovedProposals.Add(new ApprovedProposalReadModel { ProposalId = msg.ProposalId, ApprovedAt = msg.ApprovedAt });
            await _db.SaveChangesAsync();
        }
    }
}
