using Contract.Application.Contracts.Commands;
using Contract.Application.Interfaces;
using MassTransit;
using MediatR;
using static Shared.Contract.ProposalEvents;

namespace Contract.Application.Handlers
{
    public sealed class HireProposalHandler : IRequestHandler<HireProposalCommand, Guid>
    {
        private readonly IContractRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApprovedProposalReadModel _readModel;
        private readonly IPublishEndpoint _bus;

        public HireProposalHandler(IContractRepository repository, IUnitOfWork unitOfWork, IApprovedProposalReadModel readModel, IPublishEndpoint bus)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _readModel = readModel;
            _bus = bus;
        }

        public async Task<Guid> Handle(HireProposalCommand request, CancellationToken ct)
        {
            if (!await _readModel.IsApproved(request.ProposalId, ct)) 
                throw new InvalidOperationException("Proposal not approved");
            
            if (await _repository.ExistsForProposal(request.ProposalId, ct)) 
                throw new InvalidOperationException("Contract already exists");

            var c = new Domain.ContractModel(request.ProposalId);

            await _repository.Add(c, ct);
            await _unitOfWork.Commit(ct);
            await _bus.Publish(new ContractCompleted(c.Id, c.ProposalId, c.ContractedAt), ct);
            
            return c.Id;
        }
    }
}
