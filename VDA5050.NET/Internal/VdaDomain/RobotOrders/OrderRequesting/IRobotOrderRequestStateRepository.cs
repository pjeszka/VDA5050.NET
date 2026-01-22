using VDA5050.NET.Public.Models.Orders;

namespace VDA5050.NET.Internal.VdaDomain.RobotOrders.OrderRequesting;

public interface IRobotOrderRequestStateRepository
{
    ICollection<OrderRequestState> GetAll();
    void AddOrderRequest(OrderRequestState robotOrderState);
    void UpdateOrderRequestStatus(OrderId orderId, OrderUpdateId orderUpdateId, OrderRequestStatus status);
    void Clear();
}