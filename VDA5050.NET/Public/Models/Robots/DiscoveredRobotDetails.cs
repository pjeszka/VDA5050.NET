using VDA5050.NET.Public.Enums.Vda5050.Connection;
using VDA5050.NET.Public.Models.RobotDiscovery;

namespace VDA5050.NET.Public.Models.Robots;

public sealed record DiscoveredRobotDetails(
    RobotSerialNumber SerialNumber,
    string Version,
    string RobotTopicPrefix,
    ConnectionState ConnectionState)
{
    internal static DiscoveredRobotDetails? Create(DiscoveredRobot? robot)
    {
        if (robot is null)
        {
           return null;
        }
        
        return new DiscoveredRobotDetails(robot.SerialNumber, robot.Version, robot.RobotTopicPrefix, robot.ConnectionState);
    }
}