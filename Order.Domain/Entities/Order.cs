using Order.Domain.Enums;
using SharedKernel;

namespace Order.Domain.Entities;
public class Orders : BaseEntity
{    
    public string CustomerName { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public OrderStatus Status { get; set; }
}
