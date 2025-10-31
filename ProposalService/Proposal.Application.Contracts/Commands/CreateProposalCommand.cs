using MediatR;

namespace Proposal.Application.Contracts.Commands
{
    public sealed record CreateProposalCommand(Guid CustomerId, string Product, decimal Premium) : IRequest<Guid>;
}
