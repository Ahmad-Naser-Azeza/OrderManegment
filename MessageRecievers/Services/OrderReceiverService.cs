using Order.Domain.Dtos;

public class OrderReceiverService
{
    private readonly IRabbitMQReceiver<OrdersDto> _rabbitMQReceiver;

    public OrderReceiverService(IRabbitMQReceiver<OrdersDto> rabbitMQReceiver)
    {
        _rabbitMQReceiver = rabbitMQReceiver;
    }

    public void StartReceiving()
    {
        _rabbitMQReceiver.Receive("my_queue_azeza", async (order) =>
        {
            Console.WriteLine($"Received order: {order.Id}");
            // Process the order
            await Task.CompletedTask;
        });
    }
}