using MediatR;
using Proposal.Domain;

namespace Proposal.Application.Contracts.Queries
{
    public sealed record ListProposalsQuery(int Page, int Size) : IRequest<IReadOnlyList<ProposalModel>>;
}
