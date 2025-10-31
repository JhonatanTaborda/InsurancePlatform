namespace Contract.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task Commit(CancellationToken ct);
    }
}
