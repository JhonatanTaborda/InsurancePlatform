using Proposal.Domain;
using Xunit;

namespace Proposal.Tests.Domain
{
    public class ProposalTests
    {
        [Fact]
        public void Approve_From_UnderReview_Transitions()
        {
            var p = new ProposalModel(Guid.NewGuid(), "Auto", 100);
            p.Approve();
            Assert.Equal(ProposalStatus.Approved, p.Status);
        }

        [Fact]
        public void Reject_From_UnderReview_Transitions()
        {
            var p = new ProposalModel(Guid.NewGuid(), "Life", 200);
            p.Reject();
            Assert.Equal(ProposalStatus.Rejected, p.Status);
        }
    }
}