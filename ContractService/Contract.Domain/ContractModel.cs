namespace Contract.Domain
{
    public sealed class ContractModel
    {
        public Guid Id { get; private set; }
        public Guid ProposalId { get; private set; }
        public DateTime ContractedAt { get; private set; }

        private ContractModel() { }

        public ContractModel(Guid proposalId)
        {
            Id = Guid.NewGuid();
            ProposalId = proposalId;
            ContractedAt = DateTime.UtcNow;
        }
    }
}
