namespace Proposal.Application.Interfaces
{
    public interface IProposalRepository
    {
        Task Add(Proposal.Domain.ProposalModel entity, CancellationToken ct);
        Task<Proposal.Domain.ProposalModel?> Get(Guid id, CancellationToken ct);
        Task<IReadOnlyList<Proposal.Domain.ProposalModel>> List(int skip, int take, CancellationToken ct);
    }
}
