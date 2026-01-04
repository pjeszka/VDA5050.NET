using VDA5050.NET.Public.Models.Orders;

namespace VDA5050.NET.Internal.VdaDomain.RobotOrders.OrderRequesting;

public interface IRobotOrderSender
{
    Task<OrderId> SendOrder(RobotOrder robotOrder);
    Task SendOrderUpdate(RobotOrderUpdate robotOrderUpdate);
}