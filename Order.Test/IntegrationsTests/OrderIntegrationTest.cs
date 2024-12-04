using CoreOps.Order.Tests;
using Newtonsoft.Json;
using Order.Domain.Dtos;
using Order.Domain.Enums;
using Order.Domain.Mapping;
using Order.Domain.Models;
using Order.Test.Factory;
using SharedKernel;
using System.Net.Http.Headers;
using System.Text;

public class GetOrdersTest : Initializer
{
    private readonly HttpClient _client;

    public GetOrdersTest(OrdersControllerFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllOrders_ReturnsSuccess()
    {
        var configuration = GetConfigurations();

        var token = CustomsExtensions.GenerateJwtToken("admin", configuration);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync("/Orders");
        
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();        
        Assert.Contains("true", content);
    }
    [Fact]
    public async Task CreateOrder_ValidModel_ReturnsSuccess()
    {
        var configuration = GetConfigurations();

        var token = CustomsExtensions.GenerateJwtToken("admin", configuration);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var orderModel = new OrdersModel
        {
            CustomerName = Guid.NewGuid().ToString(),
            Price = new Random().Next(1, 101),
            ProductName = Guid.NewGuid().ToString(),
            Quantity = new Random().Next(1, 5),
            Status = OrderStatus.Pending,
        };
        
        var content = JsonConvert.SerializeObject(orderModel);
        var response = await _client.PostAsync("/Orders", new StringContent(content, Encoding.UTF8, "application/json"));

        response.EnsureSuccessStatusCode();
        var createdOrderContent = await response.Content.ReadAsStringAsync();
        var createdOrder = (JsonConvert.DeserializeObject<Result<OrdersDto>>(createdOrderContent)).Data;

        var orderId = createdOrder.Id;
        await DeleteOrderAfterTest(orderId);
        Assert.Contains("true", createdOrderContent);
    }
    [Fact]
    public async Task UpdateOrder_ValidModel_ReturnsSuccess()
    {
        var configuration = GetConfigurations();

        var token = CustomsExtensions.GenerateJwtToken("admin", configuration);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var createOrderModel = new OrdersModel {
            CustomerName = Guid.NewGuid().ToString(),
            Price = new Random().Next(1, 101),
            ProductName = Guid.NewGuid().ToString(),
            Quantity = new Random().Next(1, 5),
            Status = OrderStatus.Pending,
        };

        var createContent = JsonConvert.SerializeObject(createOrderModel);
        var createResponse = await _client.PostAsync("/Orders", new StringContent(createContent, Encoding.UTF8, "application/json"));

        createResponse.EnsureSuccessStatusCode();

        var createdOrderContent = await createResponse.Content.ReadAsStringAsync();
        var createdOrder = (JsonConvert.DeserializeObject<Result<OrdersDto>>(createdOrderContent)).Data;
        
        var orderId = createdOrder.Id;
        createdOrder.CustomerName = "After Update";

        var updateContent = JsonConvert.SerializeObject(createdOrder.ToModel());        
        var updateResponse = await _client.PutAsync($"/Orders/{orderId}", new StringContent(updateContent, Encoding.UTF8, "application/json"));
                        
        updateResponse.EnsureSuccessStatusCode();
        var updatedOrderContent = await updateResponse.Content.ReadAsStringAsync();
        await DeleteOrderAfterTest(orderId);
        Assert.Contains("true", updatedOrderContent);
    }
    [Fact]
    public async Task GetOrder_ValidModel_ReturnsSuccess()
    {
        var configuration = GetConfigurations();

        var token = CustomsExtensions.GenerateJwtToken("admin", configuration);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var createOrderModel = new OrdersModel {
            CustomerName = Guid.NewGuid().ToString(),
            Price = new Random().Next(1, 101),
            ProductName = Guid.NewGuid().ToString(),
            Quantity = new Random().Next(1, 5),
            Status = OrderStatus.Pending,
        }; 

        var createContent = JsonConvert.SerializeObject(createOrderModel);
        var createResponse = await _client.PostAsync("/Orders", new StringContent(createContent, Encoding.UTF8, "application/json"));

        createResponse.EnsureSuccessStatusCode();

        var createdOrderContent = await createResponse.Content.ReadAsStringAsync();
        var createdOrder = (JsonConvert.DeserializeObject<Result<OrdersDto>>(createdOrderContent)).Data;
        
        var orderId = createdOrder.Id;
        
        var getResponse = await _client.GetAsync($"/Orders/{orderId}");
        
        getResponse.EnsureSuccessStatusCode();
        var getOrderContent = await getResponse.Content.ReadAsStringAsync();
        await DeleteOrderAfterTest(orderId);
        Assert.Contains("true", getOrderContent);
        Assert.Contains(orderId.ToString(), getOrderContent);
    }
    [Fact]
    public async Task GetStatusById_ValidModel_ReturnsSuccess()
    {
        var configuration = GetConfigurations();

        var token = CustomsExtensions.GenerateJwtToken("admin", configuration);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var createOrderModel = new OrdersModel
        {
            CustomerName = Guid.NewGuid().ToString(),
            Price = new Random().Next(1, 101),
            ProductName = Guid.NewGuid().ToString(),
            Quantity = new Random().Next(1, 5),
            Status = OrderStatus.Delivered,
        };

        var createContent = JsonConvert.SerializeObject(createOrderModel);
        var createResponse = await _client.PostAsync("/Orders", new StringContent(createContent, Encoding.UTF8, "application/json"));

        createResponse.EnsureSuccessStatusCode();

        var createdOrderContent = await createResponse.Content.ReadAsStringAsync();
        var createdOrder = (JsonConvert.DeserializeObject<Result<OrdersDto>>(createdOrderContent)).Data;
        
        var orderId = createdOrder.Id;
        
        var getStatusResponse = await _client.GetAsync($"/Orders/GetStatusById/{orderId}");
        
        getStatusResponse.EnsureSuccessStatusCode();
        var getStatusContent = await getStatusResponse.Content.ReadAsStringAsync();
        await DeleteOrderAfterTest(orderId);
        Assert.Contains("true", getStatusContent);
        Assert.Contains(OrderStatus.Delivered.ToString(), getStatusContent);
    }
    [Fact]
    public async Task ChangeStatusOrders_ValidModel_ReturnsSuccess()
    {
        var configuration = GetConfigurations();

        var token = CustomsExtensions.GenerateJwtToken("admin", configuration);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var createOrderModel = new OrdersModel
        {
            CustomerName = Guid.NewGuid().ToString(),
            Price = new Random().Next(1, 101),
            ProductName = Guid.NewGuid().ToString(),
            Quantity = new Random().Next(1, 5),
            Status = OrderStatus.Shipped,
        };

        var createContent = JsonConvert.SerializeObject(createOrderModel);
        var createResponse = await _client.PostAsync("/Orders", new StringContent(createContent, Encoding.UTF8, "application/json"));

        createResponse.EnsureSuccessStatusCode();

        var createdOrderContent = await createResponse.Content.ReadAsStringAsync();
        var createdOrder = (JsonConvert.DeserializeObject<Result<OrdersDto>>(createdOrderContent)).Data;
        
        var orderId = createdOrder.Id;
        
        var changeStatusModel = new ChangeStatusOrdersModel
        {
            Id = orderId,
            Status = OrderStatus.Delivered
        };
        var changeStatusContent = new StringContent(JsonConvert.SerializeObject(changeStatusModel), Encoding.UTF8, "application/json");
        var changeStatusResponse = await _client.PostAsync("/Orders/ChangeStatusOrders", changeStatusContent);
        
        changeStatusResponse.EnsureSuccessStatusCode();
        var changeStatusContentResponse = await changeStatusResponse.Content.ReadAsStringAsync();
        await DeleteOrderAfterTest(orderId);
        Assert.Contains("true", changeStatusContentResponse);        
        Assert.Contains("2", changeStatusContentResponse);
    }
    [Fact]
    public async Task CreateAndDeleteOrder_ValidModel_ReturnsSuccess()
    {
        var configuration = GetConfigurations();

        var token = CustomsExtensions.GenerateJwtToken("admin", configuration);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var createOrderModel = new OrdersModel
        {
            CustomerName = Guid.NewGuid().ToString(),
            Price = new Random().Next(1, 101),
            ProductName = Guid.NewGuid().ToString(),
            Quantity = new Random().Next(1, 5),
            Status = OrderStatus.Shipped,
        };

        var createContent = JsonConvert.SerializeObject(createOrderModel);
        var createResponse = await _client.PostAsync("/Orders", new StringContent(createContent, Encoding.UTF8, "application/json"));

        createResponse.EnsureSuccessStatusCode();

        var createdOrderContent = await createResponse.Content.ReadAsStringAsync();
        var createdOrder = (JsonConvert.DeserializeObject<Result<OrdersDto>>(createdOrderContent)).Data;
        
        var orderId = createdOrder.Id;
        
        var deleteResponse = await _client.DeleteAsync($"/Orders/{orderId}");        
        deleteResponse.EnsureSuccessStatusCode();
        var deleteContent = await deleteResponse.Content.ReadAsStringAsync();
        await DeleteOrderAfterTest(orderId);
        Assert.Contains("true", deleteContent);        
    }
    private async Task DeleteOrderAfterTest(long orderId)
    {
        var deleteResponse = await _client.DeleteAsync($"/Orders/{orderId}");
        deleteResponse.EnsureSuccessStatusCode();
        var deleteContent = await deleteResponse.Content.ReadAsStringAsync();
        Assert.Contains("true", deleteContent);
    }
}


