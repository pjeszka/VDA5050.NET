using VDA5050.NET.Public.Models.Orders;

namespace VDA5050.NET.Internal.VdaDomain.RobotOrders.OrderRequesting;

public sealed record OrderRequestId
{
    public OrderRequestId(OrderId orderId, OrderUpdateId orderUpdateId)
    {
        OrderId = orderId;
        OrderUpdateId = orderUpdateId;
        Counter = 0;
    }

    public OrderId OrderId { get; }
    public OrderUpdateId OrderUpdateId { get; }
    public int Counter { get; private set;}

    public void IncrementCounter()
    {
        Counter++;
    }
}