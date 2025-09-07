using VDA5050.NET.VdaDomain.Robots;

namespace VDA5050.NET.API.Models;

public sealed record RobotStateChangedEvent(RobotNetworkName RobotNetworkName);