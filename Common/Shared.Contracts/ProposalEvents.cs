namespace Shared.Contract
{
    public class ProposalEvents
    {
        public record ProposalCreated(Guid ProposalId, string Product, decimal Premium, Guid CustomerId, DateTime CreatedAt);
        public record ProposalStatusChanged(Guid ProposalId, string Status, DateTime ChangedAt);
        public record ProposalApproved(Guid ProposalId, DateTime ApprovedAt);
        public record ContractCompleted(Guid ContractId, Guid ProposalId, DateTime ContractedAt);
    }
}
