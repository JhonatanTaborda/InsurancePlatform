using MediatR;
using Microsoft.AspNetCore.Mvc;
using Contract.Application.Contracts.Commands;

namespace Contract.Api.Controllers
{
    [ApiController]
    [Route("api/contracts")]
    public sealed class ContractsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ContractsController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Hire([FromBody] Request request, CancellationToken ct)
        {
            var id = await _mediator.Send(new HireProposalCommand(request.ProposalId), ct);
            return CreatedAtAction(nameof(Get), new { id }, new { id });
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id) => Ok(new { id });

        public sealed record Request(Guid ProposalId);
    }
}
