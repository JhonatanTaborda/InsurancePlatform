namespace Contract.Application.Interfaces
{
    public interface IApprovedProposalReadModel
    {
        Task<bool> IsApproved(Guid proposalId, CancellationToken ct);
    }
}
