using Order.Domain.Enums;

namespace Order.Domain.Models;
public class ChangeStatusOrdersModel
{
    public long Id { get; set; }
    public OrderStatus Status { get; set; }    
}
