using VDA5050.NET.Internal.VdaDomain.RobotOrders;
using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Models.Orders;

namespace VDA5050.NET.Public.Events;

public record RobotOrderRequestStateChanged(
    RobotSerialNumber RobotSerialNumber,
    OrderId OrderId,
    OrderUpdateId OrderUpdateId,
    OrderRequestStatus Status,
    string? Message = null);