using MediatorCoordinator.Contract;
using MediatR;
using Order.Domain.Dtos;
using Order.Domain.Models;
using SharedKernel;
using Order.Domain.Mapping;
using Order.Application.Commands;
using Microsoft.AspNetCore.Mvc;
using CoreOps.FleetManagment.Application.Validators;
using FluentValidation;

namespace CoreOps.MasterData.Application.RequestHandlers;

/// <summary>
/// Command request for creating a new Orders. This request carries the model data needed to create 
/// a new entity and processes it through the mediator.
/// </summary>
public class CreateOrderRequest : IRequestContext<Result<OrdersDto>>
{
    public required OrdersModel Model { get; set; }
}

/// <summary>
/// Handles the logic for processing the CreateOrderRequest.
/// </summary>
public class CreateOrderRequestHandler(Dispatcher dispatcher, IUnitOfWork unitOfWork) : IRequestHandler<CreateOrderRequest, Result<OrdersDto>>
{
    public async Task<Result<OrdersDto>> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
    {

        var validator = new OrderValidator(dispatcher);
        var resultValidator = await validator.ValidateAsync(request.Model);

        if (!resultValidator.IsValid)
            return Result.Failure(resultValidator.Errors.Select(e => e.ErrorMessage));

        unitOfWork.BeginTransaction();
        var orderEntity = request.Model.ToEntity();
        await dispatcher.DispatchAsync(new AddOrderCommand { Order = orderEntity });
        await unitOfWork.Commit(cancellationToken);

        return Result.Success(orderEntity.ToDto());
    }
}