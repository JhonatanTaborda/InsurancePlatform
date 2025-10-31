using MassTransit;
using Moq;
using Proposal.Application.Contracts.Commands;
using Proposal.Application.Handlers;
using Proposal.Application.Interfaces;
using Xunit;
using static Shared.Contract.ProposalEvents;

namespace Proposal.Tests.Application
{
    public class CreateProposalHandlerTests
    {
        [Fact]
        public async Task Creates_And_Publishes_Event()
        {
            var repo = new Mock<IProposalRepository>();
            var uow = new Mock<IUnitOfWork>();
            var bus = new Mock<IPublishEndpoint>();

            var handler = new CreateProposalHandler(repo.Object, uow.Object, bus.Object);

            var customerId = Guid.NewGuid();
            var cmd = new CreateProposalCommand(customerId, "Auto", 120.50m);

            var newId = await handler.Handle(cmd, CancellationToken.None);

            Assert.NotEqual(Guid.Empty, newId);

            repo.Verify(x => x.Add(It.IsAny<Proposal.Domain.ProposalModel>(), It.IsAny<CancellationToken>()), Times.Once);
            uow.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);

            bus.Verify(x => x.Publish(
                It.Is<ProposalCreated>(e =>
                    e.CustomerId == customerId &&
                    e.Product == "Auto" &&
                    e.Premium == 120.50m &&
                    e.ProposalId != Guid.Empty &&
                    e.CreatedAt != default),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
