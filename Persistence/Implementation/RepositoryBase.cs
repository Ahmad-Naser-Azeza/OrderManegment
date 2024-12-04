using Kernel.Contract;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Linq.Expressions;

namespace Persistence;

public class RepositoryBase<T> : IRepositoryBase<T>
       where T : BaseEntity
{
    private readonly DbSet<T> _dbSet;
    private readonly IHttpContext _httpContext;
    private readonly IDomainEvents _domainEvents;

    public DbContext Context { get; private set; }

    public RepositoryBase(DbContext context, IHttpContext httpContext, IDomainEvents domainEvents)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = Context.Set<T>();
        _httpContext = httpContext;
        _domainEvents = domainEvents;
    }

    #region Add
    public async Task AddOrUpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (entity.Id.Equals(default))
            await AddAsync(entity, cancellationToken);
        else
            await UpdateAsync(entity, cancellationToken);
    }

    public async Task AddOrUpdateAsync(T entity, string Changes, CancellationToken cancellationToken = default)
    {
        if (entity.Id.Equals(default))
            await AddAsync(entity, cancellationToken);
        else
            await UpdateAsync(entity, Changes, cancellationToken);
    }
    public async Task AddOrUpdateAsync(T entity, bool isStatusChanged, CancellationToken cancellationToken = default)
    {
        if (entity.Id.Equals(default))
            await AddAsync(entity, cancellationToken);
        else
        {
            await UpdateAsync(entity, cancellationToken);
        }
    }


    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTimeOffset.UtcNow;
        if (!string.IsNullOrWhiteSpace(_httpContext.IntranetUser?.UserName)) entity.CreatedBy = _httpContext.IntranetUser?.UserName;

        await _dbSet.AddAsync(entity);
        await _domainEvents.DispatchAsync(new EntityCreatedEvent<T>(entity, DateTime.UtcNow), cancellationToken);

        return entity;
    }
    public T Add(T entity) => AddAsync(entity).ConfigureAwait(false).GetAwaiter().GetResult();
    public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entity, CancellationToken cancellationToken = default)
    {
        entity.ForEach(entity => entity.CreatedAt = DateTimeOffset.UtcNow);
        await _dbSet.AddRangeAsync(entity);
        return entity;
    }
    public IEnumerable<T> AddRange(IEnumerable<T> entity) => AddRangeAsync(entity).ConfigureAwait(false).GetAwaiter().GetResult();
    #endregion

    #region Update
    public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(_httpContext.IntranetUser?.UserName)) entity.UpdatedBy = _httpContext.IntranetUser?.UserName;
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        _dbSet.Entry(entity).State = EntityState.Modified;
        _dbSet.Update(entity);

        await _domainEvents.DispatchAsync(new EntityUpdatedEvent<T>(entity, DateTime.UtcNow), cancellationToken);
        await Task.CompletedTask;

        return entity;
    }

    public async Task<T> UpdateAsync(T entity, string Changes, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(_httpContext.IntranetUser?.UserName)) entity.UpdatedBy = _httpContext.IntranetUser?.UserName;
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        _dbSet.Entry(entity).State = EntityState.Modified;
        _dbSet.Update(entity);

        await _domainEvents.DispatchAsync(new EntityUpdatedWithChngesEvent<T>(entity, Changes, DateTime.UtcNow), cancellationToken);
        await Task.CompletedTask;

        return entity;
    } 
    public void Update(T entity) => UpdateAsync(entity).ConfigureAwait(false).GetAwaiter().GetResult();
    public Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            if (!string.IsNullOrWhiteSpace(_httpContext.IntranetUser?.UserName)) entity.UpdatedBy = _httpContext.IntranetUser?.UserName;
            entity.UpdatedAt = DateTimeOffset.UtcNow;
            _dbSet.Entry(entity).State = EntityState.Modified;
        }

        _dbSet.UpdateRange(entities);

        return Task.CompletedTask;
    }
    public void UpdateRange(IEnumerable<T> entities) => UpdateRangeAsync(entities).ConfigureAwait(false).GetAwaiter().GetResult();
    #endregion

    #region Delete
    public async Task<bool> DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Entry(entity).State = EntityState.Deleted;
        var result = _dbSet.Remove(entity);
        await _domainEvents.DispatchAsync(new EntityDeletedEvent<T>(entity, DateTime.UtcNow), cancellationToken);
        return await Task.FromResult(result.State == EntityState.Deleted);
    }
    public bool Delete(T entity) => DeleteAsync(entity, default).ConfigureAwait(false).GetAwaiter().GetResult();
    public Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        foreach (var item in entities)
            _dbSet.Entry(item).State = EntityState.Deleted;

        _dbSet.RemoveRange(entities);
        return Task.CompletedTask;
    }
    public void DeleteRange(IEnumerable<T> entities) => DeleteRangeAsync(entities).ConfigureAwait(false).GetAwaiter().GetResult();

    public async Task<T> SoftDelete(T entity, CancellationToken cancellationToken = default)
    {
        entity.StatusId = (short)EntityStatus.Deleted;
        _dbSet.Entry(entity).State = EntityState.Deleted;
        _dbSet.Update(entity);
        await _domainEvents.DispatchAsync(new EntityUpdatedEvent<T>(entity, DateTime.UtcNow), cancellationToken);
        await Task.CompletedTask;
        return entity;
    }
    public Task SoftDeleteRange(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
            entity.StatusId = (short)EntityStatus.Deleted;
        return Task.CompletedTask;
    }
    #endregion

    #region Get
    public IQueryable<T> GetAll() => Context.Set<T>();
    #endregion

    #region Select
    public async Task<List<T>> ToListAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default) => await query.ToListAsync();
    public async Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default) => await query.FirstOrDefaultAsync();
    #endregion

    #region Count
    public async Task<int> CountAsync(CancellationToken cancellationToken = default) => await _dbSet.CountAsync();
    public int Count() => CountAsync().ConfigureAwait(false).GetAwaiter().GetResult();
    #endregion

    #region Any
    public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default) => await _dbSet.AnyAsync(expression);
    public bool Any(Expression<Func<T, bool>> expression) => AnyAsync(expression).ConfigureAwait(false).GetAwaiter().GetResult();
    #endregion

    #region Excute Functions
    public IQueryable<T> ExecuteFunction(string functionName)
    {
        return _dbSet.FromSql($"{functionName}").AsNoTracking();
    }

    public IQueryable<T> ExecuteFunction(string functionName, string[] parameters)
    {
        var formattedQuery = string.Format(functionName, parameters);
        return _dbSet.FromSql($"{functionName}").AsNoTracking();
    }
    #endregion

    

}

