using VDA5050.NET.Internal.Messages.Connection.Enums;
using VDA5050.NET.Public.Models;

namespace VDA5050.NET.Internal.VdaDomain.RobotDiscovery;

public record DiscoveredRobot(RobotSerialNumber SerialNumber, string Version, ConnectionState ConnectionState);