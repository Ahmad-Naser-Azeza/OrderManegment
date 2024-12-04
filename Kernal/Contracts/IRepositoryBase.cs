using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SharedKernel;

public interface IRepositoryBase<T>
{
    DbContext Context { get; }
    Task AddOrUpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task AddOrUpdateAsync(T entity, string Changes, CancellationToken cancellationToken = default);
    Task AddOrUpdateAsync(T entity, bool isStatusChanged, CancellationToken cancellationToken = default);


    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    T Add(T entity);
    
    Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<T> UpdateAsync(T entity, string Changes, CancellationToken cancellationToken = default);    
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);
    Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(T entity, CancellationToken cancellationToken = default);
    bool Delete(T entity);
    Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    void DeleteRange(IEnumerable<T> entities);
    Task<T> SoftDelete(T entity, CancellationToken cancellationToken = default);
    Task SoftDeleteRange(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    IQueryable<T> GetAll();

    Task<List<T>> ToListAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default);
    Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default);

    Task<int> CountAsync(CancellationToken cancellationToken = default);
    int Count();

    Task<bool> AnyAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
    bool Any(Expression<Func<T, bool>> expression);
}