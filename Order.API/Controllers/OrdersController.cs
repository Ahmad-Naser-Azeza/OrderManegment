using CoreOps.MasterData.Application.RequestHandlers;
using Kernel.Enum;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.Domain.Models;
using SharedKernel;

namespace CoreOps.MasterData.API.Controllers;

/// <summary>
/// API controller responsible for managing the operations related to Orders. 
/// This controller provides endpoints for creating, updating, retrieving, and deleting Orders entities.
/// </summary>
[Route("[controller]")]
[ApiController]
public class OrdersController(IMediator _mediator) : ControllerBase
{
    /// <summary>
    /// Retrieves all Orders
    /// </summary>
    [HttpGet]    
    [AuthorizePermission(UserAccessPermission.ReadOrders)]    
    public async Task<ActionResult> Get()
    {
        var result = await _mediator.Send(new GetOrdersRequest() { });
        return result;
    }

    /// <summary>
    /// Creates a new Orders entity.
    /// </summary>
    [HttpPost]
    [AuthorizePermission(UserAccessPermission.CreateOrders)]
    public async Task<ActionResult> CreateOrders([FromBody] OrdersModel model) => await _mediator.Send(new CreateOrderRequest { Model = model });

    /// <summary>
    /// Updates an existing Orders entity based on the provided ID.
    /// </summary>     
    [HttpPut("{id}")]
    [AuthorizePermission(UserAccessPermission.UpdateOrders)]
    public async Task<ActionResult> UpdateOrders(int id, [FromBody] OrdersModel model) => await _mediator.Send(new UpdateOrderRequest { Id = id, Model = model });

    /// <summary>
    /// Retrieves a specific Orders entity by its ID.
    /// </summary>
    [HttpGet("{id}")]
    [AuthorizePermission(UserAccessPermission.ReadOrders)]
    public async Task<ActionResult> Get(int id)
    {
        var result = await _mediator.Send(new GetOrderRequest() { Id = id });
        return result == null ? Result.NotFound() : Result.Success(result);
    }
    [HttpGet("GetStatusById/{id}")]
    [AuthorizePermission(UserAccessPermission.ReadOrders)]
    public async Task<ActionResult> GetStatusById(int id)
    {
        var result = await _mediator.Send(new GetOrderRequest() { Id = id });
        return result == null ? Result.NotFound() : Result.Success(result.Data.Status.ToString());
    }
    /// <summary>
    /// Deletes a Orders entity by its ID.
    /// </summary>
    [HttpDelete("{id}")]
    [AuthorizePermission(UserAccessPermission.DeleteOrders)]
    public async Task<ActionResult> Delete(int id) => await _mediator.Send(new DeleteOrderRequest { Id = id });

    [HttpPost("ChangeStatusOrders")]
    [AuthorizePermission(UserAccessPermission.UpdateOrders)]
    public async Task<ActionResult> ChangeStatusOrders([FromBody] ChangeStatusOrdersModel model) => await _mediator.Send(new ChangeStatusOrderRequest { Model = model });

}
