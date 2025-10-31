using MediatR;
using Proposal.Domain;

namespace Proposal.Application.Contracts.Queries
{
    public sealed record GetProposalByIdQuery(Guid Id) : IRequest<ProposalModel?>;
}
