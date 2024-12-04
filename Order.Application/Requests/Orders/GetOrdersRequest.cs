using CoreOps.MasterData.Application.Queries;
using MediatR;
using Order.Domain.Dtos;
using Order.Domain.Mapping;
using SharedKernel;

namespace CoreOps.MasterData.Application.RequestHandlers;

/// <summary>
/// Request class for retrieving a list of Orders. It is used to trigger the retrieval process 
/// through the mediator.
/// </summary>
public class GetOrdersRequest : IRequest<Result<IEnumerable<OrdersDto>>> { }

/// <summary>
/// Handles the GetOrdersRequest by first attempting to retrieve the data from the cache. 
/// </summary>
public class GetOrdersRequestHandler(Dispatcher _dispatcher) : IRequestHandler<GetOrdersRequest, Result<IEnumerable<OrdersDto>>>
{
    public async Task<Result<IEnumerable<OrdersDto>>> Handle(GetOrdersRequest request, CancellationToken cancellationToken)
    {
        var orders = await _dispatcher.DispatchAsync(new GetOrdersQuery(), cancellationToken);        

        if (orders.Count > 0)
        {
            return Result.Success(orders.ToDtos());
        }
        return Result.NotFound("No Orders Found");
    }
}