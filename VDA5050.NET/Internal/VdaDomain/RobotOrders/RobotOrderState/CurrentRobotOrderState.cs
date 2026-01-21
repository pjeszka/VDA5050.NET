using VDA5050.NET.Public.Models.Orders;

namespace VDA5050.NET.Internal.VdaDomain.RobotOrders.RobotOrderState;

public sealed record CurrentRobotOrderState
{
    public OrderId Id { get; }
    public uint OrderUpdateId  { get; } 
    public OrderStatus OrderStatus { get; }
    
    public OrderDetails OrderDetails { get; private set; }

    private CurrentRobotOrderState(OrderId orderId, uint orderUpdateId, OrderStatus orderStatus)
    {
        Id = orderId;
        OrderUpdateId = orderUpdateId;
        OrderStatus = orderStatus;
    }

    public static CurrentRobotOrderState Create(OrderId orderId)
    {
        return new CurrentRobotOrderState(orderId, 0, OrderStatus.Requested);
    }
}