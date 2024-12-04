using CoreOps.MasterData.Application.Queries;
using MediatorCoordinator.Contract;
using MediatR;
using Order.Application.Commands;
using Order.Domain.Enums;
using SharedKernel;

namespace CoreOps.MasterData.Application.RequestHandlers;

/// <summary>
/// Request class for deleting a Order entity by its ID. This request triggers the process to identify 
/// and delete the specified entity from the system.
/// </summary>
public class DeleteOrderRequest : IRequestContext<Result>
{
    public long Id { get; set; }
}

/// <summary>
/// Handles the DeleteOrderRequest by retrieving the Order entity, dispatching the delete command, 
/// and removing the entity from the cache. Returns the result of the deletion operation.
/// </summary>
public class DeleteOrderRequestHandler(Dispatcher dispatcher, IUnitOfWork unitOfWork) : IRequestHandler<DeleteOrderRequest, Result>
{
    public async Task<Result> Handle(DeleteOrderRequest request, CancellationToken cancellationToken)
    {
        unitOfWork.BeginTransaction();
        var order = await dispatcher.DispatchAsync(new GetOrderQuery { Id = request.Id }, cancellationToken);
        if (order == null)
            return Result.NotFound("Order not found");
        if(order.Status != OrderStatus.Pending)
            return Result.Failure($"Can't delete order {order.Status.ToString() } ");
        await dispatcher.DispatchAsync(new DeleteOrderCommand { Order = order }, cancellationToken);
        await unitOfWork.Commit(cancellationToken);
        return Result.Success("Successfully Deleted");
    }
}