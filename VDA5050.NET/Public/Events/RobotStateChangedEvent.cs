using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Models.Robots;

namespace VDA5050.NET.Public.Events;

public sealed record RobotStateChangedEvent(
    RobotSerialNumber RobotSerialNumber,
    RobotState State) : IRobotEvent;