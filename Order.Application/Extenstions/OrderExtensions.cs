using Order.Domain.Entities;
using Order.Domain.Enums;

public static class OrderExtensions
{
    public static bool CheckStatusConsecuense(OrderStatus newStatus, Orders? order)
    {
        return (newStatus != order.Status && (newStatus < order.Status || ((short)newStatus - (short)order.Status) > 1));

    }
}