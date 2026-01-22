using VDA5050.NET.Internal.VdaDomain.Robots;
using VDA5050.NET.Public.Models.Orders;

namespace VDA5050.NET.Internal.VdaDomain.RobotOrders.OrderRequesting;

internal interface IRobotOrderSender
{
    Task<OrderId> SendOrder(OperationalRobot operationalRobot, RobotOrderRequest robotOrderRequest);
    Task<OrderUpdateId> SendOrderUpdate(OperationalRobot operationalRobot, RobotOrderUpdateRequest robotOrderUpdateRequest);
}