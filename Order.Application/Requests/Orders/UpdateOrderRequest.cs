using CoreOps.FleetManagment.Application.Validators;
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
/// to be updated and the model containing the new values. It is used to trigger the update process 
/// through the mediator.
/// </summary>
public class UpdateOrderRequest : IRequestContext<Result<OrdersDto>>
{
    public required long Id { get; set; }
    public required OrdersModel Model { get; set; }
}

/// <summary>
/// Handles the UpdateOrderRequest by retrieving the existing entity, updating its properties 
/// with the values from the provided model, and persisting the changes.
/// invalidation for the updated entity.
/// </summary>
public class UpdateOrderRequestHandler(Dispatcher dispatcher, IUnitOfWork unitOfWork) : IRequestHandler<UpdateOrderRequest, Result<OrdersDto>>
{
    public async Task<Result<OrdersDto>> Handle(UpdateOrderRequest request, CancellationToken cancellationToken)
    {
        
        var validator = new OrderValidator(dispatcher);
        var resultValidator = await validator.ValidateAsync(request.Model);

        if (!resultValidator.IsValid)
            return Result.Failure(resultValidator.Errors.Select(e => e.ErrorMessage));

        var order = await dispatcher.DispatchAsync(new GetOrderQuery { Id = request.Id});

        if (order == null)
        {
            return Result.NotFound("Order not found");
        }

        if (OrderExtensions.CheckStatusConsecuense(request.Model.Status, order))
            return Result.NotFound($"Can't change status from {order.Status.ToString()} to {request.Model.Status.ToString()}");


        unitOfWork.BeginTransaction();        
        order.CustomerName = request.Model.CustomerName;
        order.Quantity = request.Model.Quantity;
        order.ProductName = request.Model.ProductName;
        order.Price = request.Model.Price;
        order.Status = request.Model.Status;

 await dispatcher.DispatchAsync(new UpdateOrderCommand { Order = order });
        await unitOfWork.Commit(cancellationToken);
        return Result.Success(order.ToDto()!);
    }
  

}