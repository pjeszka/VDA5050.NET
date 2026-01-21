using VDA5050.NET.Public.Models.Orders;

namespace VDA5050.NET.Internal.VdaDomain.RobotOrders.OrderRequesting;

public sealed record OrderRequestState(OrderId OrderId, OrderUpdateId OrderUpdateId, OrderRequestStatus Status, string? Message = null);