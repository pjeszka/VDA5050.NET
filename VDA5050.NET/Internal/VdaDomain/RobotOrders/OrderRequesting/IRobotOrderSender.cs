using VDA5050.NET.Public.Models.Orders;

namespace VDA5050.NET.Internal.VdaDomain.RobotOrders.OrderRequesting;

public interface IRobotOrderSender
{
    Task<OrderId> SendOrder(RobotOrderRequest robotOrderRequest);
    Task<OrderUpdateId> SendOrderUpdate(RobotOrderUpdateRequest robotOrderUpdateRequest);
}