using VDA5050.NET.Public.Enums.Vda5050.Connection;

namespace VDA5050.NET.Public.Models.RobotDiscovery;

public record DiscoveredRobot(
    RobotSerialNumber SerialNumber,
    string Version,
    string RobotTopicPrefix,
    ConnectionState ConnectionState);