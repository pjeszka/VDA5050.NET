using VDA5050.NET.Public.Enums.Vda5050.State;
using VDA5050.NET.Public.Models;

namespace VDA5050.NET.Public.Events;

public record RobotInstantActionStateChanged(
    RobotSerialNumber RobotSerialNumber,
    ActionId ActionId,
    ActionStatus Status);