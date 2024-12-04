using CoreOps.MasterData.Application.Queries;
using CoreOps.MasterData.Application.RequestHandlers;
using CoreOps.Order.Tests;
using MediatR;
using Order.Domain.Enums;
using Order.Domain.Mapping;
using Order.Domain.Models;
using Order.Test.Factory;
using SharedKernel;

namespace CoreOps.MasterData.Tests.Controllers
{
    public class OrdersControllerTests : Initializer
    {        
        private readonly IMediator _mediator;
        
        private readonly Dispatcher _dispatcher;

        public OrdersControllerTests(OrdersControllerFactory factory)
        {            
            _mediator = GetMediator();            
            _dispatcher = GetDispatcher();
        }
        [Fact]
        public async Task Order_Save_Test()
        {
            var orderModel = new OrdersModel
            {
                CustomerName = Guid.NewGuid().ToString(),
                Price = new Random().Next(1, 101),
                ProductName = Guid.NewGuid().ToString(),
                Quantity = new Random().Next(1, 5),
                Status = OrderStatus.Pending,
            };
            var CustomerName = orderModel.CustomerName;            
            var result = await _mediator.Send(new CreateOrderRequest { Model = orderModel });
            var data = result.Data;

            var dto = (await _dispatcher.DispatchAsync(new GetOrderQuery { AsNoTracking = true, Id = data.Id })).ToDto();

            await DeleteOrderAfterTest(data.Id);
            Assert.True(
               data.IsNotNull()
                , "No Result Returned");
            Assert.True(
                dto.IsNotNull()
                , "Not Saved Successfully");
            Assert.True(
                data.Id != 0
                , "Not Saved Successfully"); 
            Assert.True(
                CustomerName == dto.CustomerName
                , "Not Saved Successfully");
        }
        [Fact]
        public async Task Order_Update_Test()
        {
            var orderModel = new OrdersModel
            {
                CustomerName = Guid.NewGuid().ToString(),
                Price = new Random().Next(1, 101),
                ProductName = Guid.NewGuid().ToString(),
                Quantity = new Random().Next(1, 5),
                Status = OrderStatus.Pending,
            };
            var CustomerName = orderModel.CustomerName;
            var result = await _mediator.Send(new CreateOrderRequest { Model = orderModel });
            var data = result.Data;
            
            data.CustomerName = CustomerName + "TestUpdate";
            var resultUpdate = await _mediator.Send(new UpdateOrderRequest {Id = data.Id, Model = data.ToModel() });
            var updatedData = resultUpdate.Data;

            await DeleteOrderAfterTest(data.Id);
            Assert.True(
               updatedData.IsNotNull()
                , "No Result Returned");
            Assert.True(
                resultUpdate.IsNotNull()
                , "Not Saved Successfully");
            Assert.True(
                updatedData.Id != 0
                , "Not Saved Successfully");
            Assert.True(
                updatedData.CustomerName == data.CustomerName
                , "Not Saved Successfully");

        }
        [Fact]
        public async Task Order_ChangeStatus_Test()
        {
            var orderModel = new OrdersModel
            {
                CustomerName = Guid.NewGuid().ToString(),
                Price = new Random().Next(1, 101),
                ProductName = Guid.NewGuid().ToString(),
                Quantity = new Random().Next(1, 5),
                Status = OrderStatus.Pending,
            };
            var CustomerName = orderModel.CustomerName;
            var result = await _mediator.Send(new CreateOrderRequest { Model = orderModel });
            var data = result.Data;            
           
            var resultUpdate = await _mediator.Send(new ChangeStatusOrderRequest { Model = new ChangeStatusOrdersModel { Id = data.Id , Status = OrderStatus.Shipped } });
            var updatedData = resultUpdate.Data;

            await DeleteOrderAfterTest(data.Id);
            Assert.True(
               updatedData.IsNotNull()
                , "No Result Returned");
            Assert.True(
                resultUpdate.IsNotNull()
                , "Not Saved Successfully");
            Assert.True(
                updatedData.Id != 0
                , "Not Saved Successfully");
            Assert.True(
                updatedData.Status == OrderStatus.Shipped
                , "Not Saved Successfully");


        }
        [Fact]
        public async Task Order_Delete_Test()
        {
            var orderModel = new OrdersModel
            {
                CustomerName = Guid.NewGuid().ToString(),
                Price = new Random().Next(1, 101),
                ProductName = Guid.NewGuid().ToString(),
                Quantity = new Random().Next(1, 5),
                Status = OrderStatus.Pending,
            };
            var CustomerName = orderModel.CustomerName;
            var result = await _mediator.Send(new CreateOrderRequest { Model = orderModel });
            var data = result.Data;

            var resultDelete = await _mediator.Send(new DeleteOrderRequest { Id  = data.Id});
           

            Assert.True(
                resultDelete.IsSuccess
                , "Not Deleted Successfully");
          


        }
        private async Task DeleteOrderAfterTest(long orderId)
        {
            var result = await _mediator.Send(new DeleteOrderRequest { Id = orderId});
        }
    }
}
