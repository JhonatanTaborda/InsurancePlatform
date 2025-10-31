using MediatR;

namespace Proposal.Application.Contracts.Commands
{
    public sealed record ChangeStatusCommand(Guid ProposalId, string Status) : IRequest<Unit>;
}
