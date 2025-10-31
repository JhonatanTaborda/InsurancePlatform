using MediatR;
using Proposal.Application.Contracts.Queries;
using Proposal.Application.Interfaces;

namespace Proposal.Application.Handlers
{
    public sealed class ListProposalsHandler : IRequestHandler<ListProposalsQuery, IReadOnlyList<Domain.ProposalModel>>
    {
        private readonly IProposalRepository _repository;
        public ListProposalsHandler(IProposalRepository repository) => _repository = repository;

        public Task<IReadOnlyList<Domain.ProposalModel>> Handle(ListProposalsQuery request, CancellationToken ct)
            => _repository.List(Math.Max(0, (request.Page - 1) * request.Size), request.Size, ct);
    }

    public sealed class GetProposalByIdHandler : IRequestHandler<GetProposalByIdQuery, Domain.ProposalModel?>
    {
        private readonly IProposalRepository _repository;
        public GetProposalByIdHandler(IProposalRepository repository) => _repository = repository;

        public Task<Domain.ProposalModel?> Handle(GetProposalByIdQuery request, CancellationToken ct)
            => _repository.Get(request.Id, ct);
    }
}
