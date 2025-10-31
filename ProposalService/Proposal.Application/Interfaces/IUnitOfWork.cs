namespace Proposal.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task Commit(CancellationToken ct);
    }
}
