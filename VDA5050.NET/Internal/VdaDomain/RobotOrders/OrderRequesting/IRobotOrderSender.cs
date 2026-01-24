using VDA5050.NET.Internal.VdaDomain.Robots;
using VDA5050.NET.Public.Models.Orders;
using VDA5050.NET.Public.Models.Orders.OrderRequesting;

namespace VDA5050.NET.Internal.VdaDomain.RobotOrders.OrderRequesting;

internal interface IRobotOrderSender
{
    Task SendOrder(OperationalRobot operationalRobot, RobotOrderRequest robotOrderRequest, OrderId orderId);
    Task SendOrderUpdate(OperationalRobot operationalRobot, RobotOrderUpdateRequest robotOrderUpdateRequest, OrderUpdateId orderUpdateId);
}