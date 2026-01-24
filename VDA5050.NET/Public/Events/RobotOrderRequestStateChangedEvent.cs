using VDA5050.NET.Public.Enums.Domain;
using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Models.Orders;

namespace VDA5050.NET.Public.Events;

public record RobotOrderRequestStateChangedEvent(
    RobotSerialNumber RobotSerialNumber,
    OrderId OrderId,
    OrderUpdateId OrderUpdateId,
    OrderRequestStatus Status,
    string? Message = null) : IRobotEvent;