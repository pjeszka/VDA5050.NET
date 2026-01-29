using VDA5050.NET.Public.Models;

namespace VDA5050.NET.Public.Events;

public sealed record RobotPositionChangedEvent(
    RobotSerialNumber RobotSerialNumber,
    double X,
    double Y,
    double Theta) : IRobotEvent;