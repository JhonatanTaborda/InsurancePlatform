using Contract.Api.Controllers;
using Contract.Application.Contracts.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Contract.Tests.Api
{
    public class ContractsControllerTests
    {
        [Fact]
        public async Task Hire_Returns_Created()
        {
            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Send(It.IsAny<HireProposalCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Guid.NewGuid());

            var controller = new ContractsController(mediator.Object);
            var result = await controller.Hire(new ContractsController.Request(Guid.NewGuid()), CancellationToken.None) as CreatedAtActionResult;

            Assert.NotNull(result);
            Assert.Equal("Get", result!.ActionName);
        }
    }
}
