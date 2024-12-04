using CoreOps.MasterData.Application.Queries;
using MediatorCoordinator.Contract;
using MediatR;
using Order.Application.Commands;
using Order.Domain.Dtos;
using Order.Domain.Mapping;
using Order.Domain.Models;
using SharedKernel;

namespace CoreOps.MasterData.Application.RequestHandlers;

/// <summary>
/// Request class for updating an existing Order entity. This request includes the ID of the entity 
/// to be change status.
/// through the mediator.
/// </summary>
public class ChangeStatusOrderRequest : IRequestContext<Result<OrdersDto>>
{    
    public required ChangeStatusOrdersModel Model { get; set; }
}

/// <summary>
/// Handles the ChangeStatusOrderRequest by retrieving the existing entity, updating its properties 
/// with the values from the provided model, and persisting the changes.
/// invalidation for the updated entity.
/// </summary>
public class ChangeStatusOrderRequestHandler(Dispatcher dispatcher, IUnitOfWork unitOfWork) : IRequestHandler<ChangeStatusOrderRequest, Result<OrdersDto>>
{
    public async Task<Result<OrdersDto>> Handle(ChangeStatusOrderRequest request, CancellationToken cancellationToken)
    {
        var order = await dispatcher.DispatchAsync(new GetOrderQuery { Id = request.Model.Id });
        if (order == null)
        {
            return Result.NotFound("Order not found");
        }
        if (OrderExtensions.CheckStatusConsecuense(request.Model.Status, order))
            return Result.NotFound($"Can't change status from {order.Status.ToString()} to {request.Model.Status.ToString()}");
        unitOfWork.BeginTransaction();
       
        order.Status = request.Model.Status;
       

        await dispatcher.DispatchAsync(new UpdateOrderCommand { Order = order });
        await unitOfWork.Commit(cancellationToken);
        return Result.Success(order.ToDto()!);
    }
}