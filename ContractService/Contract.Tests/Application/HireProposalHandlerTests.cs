using Contract.Application.Contracts.Commands;
using Contract.Application.Handlers;
using Contract.Application.Interfaces;
using MassTransit;
using Moq;
using Xunit;
using static Shared.Contract.ProposalEvents;

namespace Contract.Tests.Application
{
    public class HireProposalHandlerTests
    {
        [Fact]
        public async Task Hires_When_Approved_And_Not_Exists()
        {
            var repo = new Mock<IContractRepository>();
            repo.Setup(x => x.ExistsForProposal(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var uow = new Mock<IUnitOfWork>();

            var read = new Mock<IApprovedProposalReadModel>();
            var proposalId = Guid.NewGuid();
            read.Setup(x => x.IsApproved(proposalId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var bus = new Mock<IPublishEndpoint>();

            var handler = new HireProposalHandler(repo.Object, uow.Object, read.Object, bus.Object);

            var id = await handler.Handle(new HireProposalCommand(proposalId), CancellationToken.None);

            Assert.NotEqual(Guid.Empty, id);

            repo.Verify(x => x.Add(It.IsAny<Contract.Domain.ContractModel>(), It.IsAny<CancellationToken>()), Times.Once);
            uow.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);

            bus.Verify(x => x.Publish(
                It.Is<ContractCompleted>(e =>
                    e.ProposalId == proposalId &&
                    e.ContractId != Guid.Empty &&
                    e.ContractedAt != default),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Throws_When_Not_Approved()
        {
            var repo = new Mock<IContractRepository>();
            var uow = new Mock<IUnitOfWork>();
            var read = new Mock<IApprovedProposalReadModel>();
            read.Setup(x => x.IsApproved(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
            var bus = new Mock<IPublishEndpoint>();

            var handler = new HireProposalHandler(repo.Object, uow.Object, read.Object, bus.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(new HireProposalCommand(Guid.NewGuid()), CancellationToken.None));
        }

        [Fact]
        public async Task Throws_When_Already_Exists()
        {
            var repo = new Mock<IContractRepository>();
            repo.Setup(x => x.ExistsForProposal(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var uow = new Mock<IUnitOfWork>();
            var read = new Mock<IApprovedProposalReadModel>();
            read.Setup(x => x.IsApproved(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            var bus = new Mock<IPublishEndpoint>();

            var handler = new HireProposalHandler(repo.Object, uow.Object, read.Object, bus.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(new HireProposalCommand(Guid.NewGuid()), CancellationToken.None));
        }
    }
}
