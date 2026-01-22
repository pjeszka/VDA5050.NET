using VDA5050.NET.Public.Models;

namespace VDA5050.NET.Public.Events;

public record RobotInstantActionStateChanged(
    RobotSerialNumber RobotSerialNumber,
    );