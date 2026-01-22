using System.Collections.Concurrent;
using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Models.Orders;

namespace VDA5050.NET.Internal.VdaDomain.RobotOrders.OrderRequesting;

public sealed class RobotOrderRequestStateRepository : IRobotOrderRequestStateRepository
{
    private readonly ConcurrentDictionary<RobotSerialNumber, OrderRequestState> _orders = new();
    public ICollection<OrderRequestState> GetAll()
    {
        return _orders.Values;
    }

    public void AddOrderRequest(OrderRequestState robotOrderState)
    {
        throw new NotImplementedException();
    }

    public void UpdateOrderRequestStatus(OrderId orderId, OrderUpdateId orderUpdateId, OrderRequestStatus status)
    {
        throw new NotImplementedException();
    }
}