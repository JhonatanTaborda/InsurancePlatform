using MassTransit;
using Moq;
using Proposal.Application.Contracts.Commands;
using Proposal.Application.Contracts.Queries;
using Proposal.Application.Handlers;
using Proposal.Application.Interfaces;
using Proposal.Domain;
using Xunit;

namespace Proposal.Tests.Application
{
    public class ListProposalsHandlersTests
    {
        [Fact]
        public async Task List_Returns_Paginated()
        {
            var items = new List<Proposal.Domain.ProposalModel>
        {
            new(Guid.NewGuid(), "Auto", 100),
            new(Guid.NewGuid(), "Home", 200)
        };

            var repo = new Mock<IProposalRepository>();
            repo.Setup(x => x.List(0, 2, It.IsAny<CancellationToken>())).ReturnsAsync(items);

            var handler = new ListProposalsHandler(repo.Object);
            var result = await handler.Handle(new ListProposalsQuery(1, 2), CancellationToken.None);

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task Get_By_Id_Returns_Item()
        {
            var p = new Proposal.Domain.ProposalModel(Guid.NewGuid(), "Auto", 100);
            var repo = new Mock<IProposalRepository>();
            repo.Setup(x => x.Get(p.Id, It.IsAny<CancellationToken>())).ReturnsAsync(p);

            var handler = new GetProposalByIdHandler(repo.Object);
            var result = await handler.Handle(new GetProposalByIdQuery(p.Id), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(p.Id, result!.Id);
        }
    }
}
