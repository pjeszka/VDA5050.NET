using VDA5050.NET.Internal.VdaDomain.Robots;

namespace VDA5050.NET.Public.Models;

public sealed record RobotStateChangedEvent(RobotNetworkName RobotNetworkName);