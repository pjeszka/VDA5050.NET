using VDA5050.NET.Public.Messages.Order;

namespace VDA5050.NET.Public.Models.InstantActions;

public sealed record RobotInstantActionRequest(RobotSerialNumber RobotSerialNumber, List<ActionItem> Actions);