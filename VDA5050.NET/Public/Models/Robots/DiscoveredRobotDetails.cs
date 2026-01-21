using VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.Connection.Enums;
using VDA5050.NET.Public.Models.RobotDiscovery;

namespace VDA5050.NET.Public.Models.Robots;

public sealed record DiscoveredRobotDetails(
    RobotSerialNumber SerialNumber,
    string Version,
    ConnectionState ConnectionState)
{
    internal static DiscoveredRobotDetails Create(DiscoveredRobot robot)
    {
        return new DiscoveredRobotDetails(robot.SerialNumber, robot.Version, robot.ConnectionState);
    }
}