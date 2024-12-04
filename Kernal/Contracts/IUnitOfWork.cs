namespace SharedKernel;

public interface IUnitOfWork
{
    Task SaveChanges(CancellationToken cancellationToken = default);
    void BeginTransaction();
    Task<int> Commit(CancellationToken cancellationToken = default);
    Task Rollback(CancellationToken cancellationToken = default);
}

