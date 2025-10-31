using MassTransit;
using MediatR;
using Moq;
using Proposal.Application.Contracts.Commands;
using Proposal.Application.Handlers;
using Proposal.Application.Interfaces;
using Proposal.Domain;
using Xunit;
using static Shared.Contract.ProposalEvents;

namespace Proposal.Tests.Application
{
    public class ChangeStatusHandlerTests
    {
        [Fact]
        public async Task Approve_Publishes_Approved_And_StatusChanged()
        {
            var proposal = new Proposal.Domain.ProposalModel(Guid.NewGuid(), "Auto", 100);
            var repo = new Mock<IProposalRepository>();
            repo.Setup(x => x.Get(proposal.Id, It.IsAny<CancellationToken>())).ReturnsAsync(proposal);

            var uow = new Mock<IUnitOfWork>();
            var bus = new Mock<IPublishEndpoint>();

            var handler = new ChangeStatusHandler(repo.Object, uow.Object, bus.Object);
            var cmd = new ChangeStatusCommand(proposal.Id, "Approved");

            var result = await handler.Handle(cmd, CancellationToken.None);

            Assert.Equal(Unit.Value, result);
            Assert.Equal(ProposalStatus.Approved, proposal.Status);

            uow.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);

            bus.Verify(x => x.Publish(It.IsAny<ProposalStatusChanged>(), It.IsAny<CancellationToken>()), Times.Once);
            bus.Verify(x => x.Publish(It.IsAny<ProposalApproved>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Reject_Publishes_Only_StatusChanged()
        {
            var proposal = new Proposal.Domain.ProposalModel(Guid.NewGuid(), "Life", 200);
            var repo = new Mock<IProposalRepository>();
            repo.Setup(x => x.Get(proposal.Id, It.IsAny<CancellationToken>())).ReturnsAsync(proposal);

            var uow = new Mock<IUnitOfWork>();
            var bus = new Mock<IPublishEndpoint>();

            var handler = new ChangeStatusHandler(repo.Object, uow.Object, bus.Object);
            var cmd = new ChangeStatusCommand(proposal.Id, "Rejected");

            var result = await handler.Handle(cmd, CancellationToken.None);

            Assert.Equal(Unit.Value, result);
            Assert.Equal(ProposalStatus.Rejected, proposal.Status);
            uow.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);

            bus.Verify(x => x.Publish(It.IsAny<ProposalStatusChanged>(), It.IsAny<CancellationToken>()), Times.Once);
            bus.Verify(x => x.Publish(It.IsAny<ProposalApproved>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
