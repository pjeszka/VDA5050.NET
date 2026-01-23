using System.Collections.Concurrent;
using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Models.Orders;

namespace VDA5050.NET.Internal.VdaDomain.RobotOrders.OrderRequesting;

public sealed class RobotOrderRequestStateRepository : IRobotOrderRequestStateRepository
{
    private readonly ConcurrentDictionary<OrderRequestId, OrderRequestState> _ordersRequestStates = new();
    public ICollection<OrderRequestState> GetAll()
    {
        return _ordersRequestStates.Values.ToList();
    }

    public ICollection<OrderRequestState> GetAllForRobot(RobotSerialNumber robotSerialNumber)
    {
        return _ordersRequestStates.Values.Where(x => x.RobotSerialNumber == robotSerialNumber).ToList();
    }

    public void AddOrderRequest(OrderRequestState robotOrderState)
    {
        if (_ordersRequestStates.TryAdd(robotOrderState.Id, robotOrderState) is false)
        {
            robotOrderState.Id.IncrementCounter();
            _ordersRequestStates.Remove(robotOrderState.Id, out _);
            _ordersRequestStates.AddOrUpdate(robotOrderState.Id, robotOrderState, (_, _) => robotOrderState);
        }
    }

    public void UpdateOrderRequestStatus(
        OrderId orderId,
        OrderUpdateId orderUpdateId,
        OrderRequestStatus status,
        string? message = null,
        DateTime? sentAt = null)
    {
        _ordersRequestStates
            .FirstOrDefault(x => x.Value.Id.OrderId == orderId && x.Value.Id.OrderUpdateId == orderUpdateId)
            .Value
            .Update(status, message, sentAt);
        Clear();
    }

    public void Clear()
    {
        foreach (var kvp in _ordersRequestStates)
        {
            if (kvp.Value.HasEnded)
            {
                _ordersRequestStates.TryRemove(kvp.Key, out _);
            }
        }
    }
}