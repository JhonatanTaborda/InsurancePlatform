namespace Proposal.Domain
{
    public sealed class ProposalModel
    {
        public Guid Id { get; private set; }
        public Guid CustomerId { get; private set; }
        public string Product { get; private set; }
        public decimal Premium { get; private set; }
        public ProposalStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private ProposalModel() { }

        public ProposalModel(Guid customerId, string product, decimal premium)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
            Product = product;
            Premium = premium;
            Status = ProposalStatus.UnderReview;
            CreatedAt = DateTime.UtcNow;
        }

        public void Approve()
        {
            if (Status != ProposalStatus.UnderReview) 
                throw new InvalidOperationException("Invalid status");
            
            Status = ProposalStatus.Approved;
        }

        public void Reject()
        {
            if (Status != ProposalStatus.UnderReview) 
                throw new InvalidOperationException("Invalid status");
            
            Status = ProposalStatus.Rejected;
        }
    }
}
