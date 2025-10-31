using MassTransit;
using MediatR;
using Proposal.Application.Contracts.Commands;
using Proposal.Application.Interfaces;
using Proposal.Domain;
using static Shared.Contract.ProposalEvents;

namespace Proposal.Application.Handlers
{
    public sealed class ChangeStatusHandler : IRequestHandler<ChangeStatusCommand, Unit>
    {
        private readonly IProposalRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublishEndpoint _bus;

        public ChangeStatusHandler(IProposalRepository repository, IUnitOfWork unitOfWork, IPublishEndpoint bus)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _bus = bus;
        }

        public async Task<Unit> Handle(ChangeStatusCommand request, CancellationToken ct)
        {
            ProposalModel p = await _repository.Get(request.ProposalId, ct) ?? throw new KeyNotFoundException();

            if (request.Status.Equals("Approved", StringComparison.OrdinalIgnoreCase)) 
                p.Approve();
            else if (request.Status.Equals("Rejected", StringComparison.OrdinalIgnoreCase)) 
                p.Reject();
            else 
                throw new ArgumentException("Invalid status");

            await _unitOfWork.Commit(ct);
            await _bus.Publish(new ProposalStatusChanged(p.Id, p.Status.ToString(), DateTime.UtcNow), ct);
            
            if (p.Status == ProposalStatus.Approved)
                await _bus.Publish(new ProposalApproved(p.Id, DateTime.UtcNow), ct);

            return Unit.Value;
        }
    }
}
