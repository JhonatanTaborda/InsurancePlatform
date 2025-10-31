using MassTransit;
using MediatR;
using Proposal.Application.Contracts.Commands;
using Proposal.Application.Interfaces;
using Shared.Contract;
using static Shared.Contract.ProposalEvents;

namespace Proposal.Application.Handlers
{
    public sealed class CreateProposalHandler : IRequestHandler<CreateProposalCommand, Guid>
    {
        private readonly IProposalRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublishEndpoint _bus;

        public CreateProposalHandler(IProposalRepository repository, IUnitOfWork unitOfWork, IPublishEndpoint bus)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _bus = bus;
        }

        public async Task<Guid> Handle(CreateProposalCommand request, CancellationToken ct)
        {
            var proposal = new Domain.ProposalModel(request.CustomerId, request.Product, request.Premium);
            
            await _repository.Add(proposal, ct);
            await _unitOfWork.Commit(ct);
            await _bus.Publish(new ProposalCreated(proposal.Id, proposal.Product, proposal.Premium, proposal.CustomerId, proposal.CreatedAt), ct);
            
            return proposal.Id;
        }
    }
}
