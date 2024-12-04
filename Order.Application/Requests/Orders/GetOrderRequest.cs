using CoreOps.MasterData.Application.Queries;
using MediatR;
using Order.Domain.Dtos;
using Order.Domain.Mapping;
using SharedKernel;

namespace CoreOps.MasterData.Application.RequestHandlers;

/// <summary>
/// Request class for retrieving a Order various filtering criteria,such as ID.
/// This request triggers the retrieval process through the mediator
/// and is used to fetch the entity's data.
/// </summary>
public class GetOrderRequest : IRequest<Result<OrdersDto>>
{
    public long Id { get; set; }
}

/// <summary>
/// The result is cached for future requests.
/// </summary>
public class GetOrderRequestHandler(Dispatcher dispatcher) : IRequestHandler<GetOrderRequest, Result<OrdersDto>>
{
    public async Task<Result<OrdersDto>> Handle(GetOrderRequest request, CancellationToken cancellationToken)
    {        
        var order = await dispatcher.DispatchAsync(new GetOrderQuery
        {
            Id = request.Id,
            AsNoTracking = true
        });        
        if (order is null)
            return Result.NotFound("Order Not Found");
        
        return Result.Success(order.ToDto());
    }
}