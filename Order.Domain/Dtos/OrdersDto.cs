using Order.Domain.Enums;

namespace Order.Domain.Dtos;
public class OrdersDto
{
    public long Id { get; set; }
    public string CustomerName { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public OrderStatus Status { get; set; }
}
