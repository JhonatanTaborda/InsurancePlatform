using MediatR;

namespace Contract.Application.Contracts.Commands
{
    public sealed record HireProposalCommand(Guid ProposalId) : IRequest<Guid>;
}
