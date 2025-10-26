using VDA5050.NET.Public.Messages.Connection.Enums;

namespace VDA5050.NET.Public.Models.RobotDiscovery;

public record DiscoveredRobot(RobotSerialNumber SerialNumber, string Version, string RobotTopicPrefix, ConnectionState ConnectionState);