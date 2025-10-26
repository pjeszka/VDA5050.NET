using VDA5050.NET.Public.Messages.Order;

namespace VDA5050.NET.Public.Models.Orders;

public sealed record RobotOrderUpdate(RobotSerialNumber RobotSerialNumber, Order Order);