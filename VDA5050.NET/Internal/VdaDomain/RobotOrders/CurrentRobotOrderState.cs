using VDA5050.NET.Public.Models.Orders;

namespace VDA5050.NET.Internal.VdaDomain.RobotOrders;

public sealed record CurrentRobotOrderState(int HeaderId, OrderId OrderId, uint OrderUpdateId);