using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Proposal.Api.Controllers;
using Proposal.Application.Contracts.Commands;
using Xunit;

namespace Proposal.Tests.Api
{
    public class ProposalsControllerTests
    {
        [Fact]
        public async Task Create_Returns_Created()
        {
            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Send(It.IsAny<CreateProposalCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Guid.NewGuid());

            var controller = new ProposalsController(mediator.Object);
            var result = await controller.Create(new ProposalsController.CreateRequest(Guid.NewGuid(), "Auto", 100), CancellationToken.None) as CreatedAtActionResult;

            Assert.NotNull(result);
            Assert.Equal("Get", result!.ActionName);
        }
    }
}
