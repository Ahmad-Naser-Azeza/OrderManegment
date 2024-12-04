using Microsoft.EntityFrameworkCore;
using Order.Domain.Entities;
using SharedKernel;

namespace CoreOps.MasterData.Application.Queries;

/// <summary>
/// Query class for retrieving a list of Orders. It allows for an option to retrieve the data 
/// with or without entity tracking for performance optimization.
/// </summary>
public class GetOrdersQuery : IQuery<List<Orders>>
{
    public bool AsNoTracking { get; set; }
}

/// <summary>
/// Handles the GetOrdersQuery by querying the repository for active Orders entities.
/// It supports retrieval with or without tracking based on the query's parameters.
/// </summary>
public class GetOrdersQueryHandler(IRepositoryBase<Orders> repository) : IQueryHandler<GetOrdersQuery, List<Orders>>
{
    public async Task<List<Orders>> Handle(GetOrdersQuery query, CancellationToken cancellationToken = default)
    {
        var db = repository.GetAll();
        
        if (query.AsNoTracking)
            db = db.AsNoTracking();        
        return await repository.ToListAsync(db, cancellationToken); 
    }
}
