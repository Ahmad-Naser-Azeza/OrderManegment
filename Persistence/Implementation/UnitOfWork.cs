using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SharedKernel;

namespace Persistence;

public class UnitOfWork(DbContext dbContext) : IUnitOfWork, IDisposable
{
    private IDbContextTransaction? _transaction;
    public void BeginTransaction() => _transaction = dbContext.Database.BeginTransaction();
    public async Task SaveChanges(CancellationToken cancellationToken) => await dbContext.SaveChangesAsync(cancellationToken);
    public async Task<int> Commit(CancellationToken cancellationToken)
    {
        try
        {
            UnitOfWorkInterceptor.SkipInterceptor();

            var ser = await dbContext.SaveChangesAsync(cancellationToken);
            await _transaction!.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();

            return ser;
        }
        catch (DbUpdateException ex)
        {
            await _transaction!.RollbackAsync(cancellationToken);
            throw ex;
        }
        finally
        {
            UnitOfWorkInterceptor.ResumeInterceptor();
        }
    }

    public async Task Rollback(CancellationToken cancellationToken) => await _transaction!.RollbackAsync(cancellationToken);

    public void Dispose() => dbContext.Dispose();
}