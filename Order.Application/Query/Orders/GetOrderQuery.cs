using Microsoft.EntityFrameworkCore;
using Order.Domain.Entities;
using SharedKernel;

namespace CoreOps.MasterData.Application.Queries;

/// <summary>
/// Query class for retrieving a single Orders entity based on various filtering criteria, 
/// such as ID. It supports an option to retrieve the entity with or without tracking for more 
/// efficient database operations.
/// </summary>
public class GetOrderQuery : IQuery<Orders?>
{
    public long Id { get; set; }      
    public bool AsNoTracking { get; set; }
}

/// <summary>
/// Handles the GetOrdersQuery by querying the repository for a single Orders entity 
/// based on the provided filtering criteria (e.g., ID). It allows for future extensibility to include 
/// additional filters. Returns null if no entity matches the criteria, ensuring that only non-deleted 
/// entities are retrieved.
/// </summary>
public class GetOrderQueryHandler(IRepositoryBase<Orders> repository) : IQueryHandler<GetOrderQuery, Orders?>
{
    public async Task<Orders?> Handle(GetOrderQuery query, CancellationToken cancellationToken = default)
    {
        var db = repository.GetAll();           

        if (query.AsNoTracking)
            db = db.AsNoTracking();

        if (query.Id > 0)
            db = db.Where(x => x.Id == query.Id);

        return await repository.FirstOrDefaultAsync(db, cancellationToken);
        
    }
}