using MediatR;
using Microsoft.AspNetCore.Mvc;
using Proposal.Application.Contracts.Commands;
using Proposal.Application.Contracts.Queries;

namespace Proposal.Api.Controllers
{
    [ApiController]
    [Route("api/proposals")]
    public sealed class ProposalsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProposalsController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRequest request, CancellationToken ct)
        {
            var id = await _mediator.Send(new CreateProposalCommand(request.CustomerId, request.Product, request.Premium), ct);
            return CreatedAtAction(nameof(Get), new { id }, new { id });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken ct)
        {
            var p = await _mediator.Send(new GetProposalByIdQuery(id), ct);
            
            return p is null
                ? NotFound()
                : Ok(new { p.Id, p.CustomerId, p.Product, p.Premium, Status = p.Status.ToString(), p.CreatedAt });
        }

        [HttpGet]
        public async Task<IReadOnlyList<object>> List([FromQuery] int page = 1, [FromQuery] int size = 20, CancellationToken ct = default)
        {
            var items = await _mediator.Send(new ListProposalsQuery(page, size), ct);
            return items.Select(p => (object)new { p.Id, p.CustomerId, p.Product, p.Premium, Status = p.Status.ToString(), p.CreatedAt })
                        .ToList().AsReadOnly();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] ChangeStatusRequest request, CancellationToken ct)
        {
            await _mediator.Send(new ChangeStatusCommand(id, request.Status), ct);
            return NoContent();
        }

        public sealed record CreateRequest(Guid CustomerId, string Product, decimal Premium);
        public sealed record ChangeStatusRequest(string Status);
    }
}
