using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Models.Orders.OrderState;

namespace VDA5050.NET.Public.Events;

public sealed record RobotOrderStateChangedEvent(
    RobotSerialNumber RobotSerialNumber,
    RobotOrderState OrderState) : IRobotEvent;