using VDA5050.NET.Internal.VdaDomain.Master.Robots;
using VDA5050.NET.Internal.VdaDomain.Robots;
using VDA5050.NET.Public.Enums.Vda5050.Connection;

namespace VDA5050.NET.Public.Models.Robots;

public record OperationalRobotDetails(
    RobotSerialNumber SerialNumber,
    ConnectionState ConnectionState,
    RobotState? State,
    FactsheetInfo? Factsheet)
{
    internal static OperationalRobotDetails FromEntity(OperationalRobot entity)
    {
        return new OperationalRobotDetails(
            entity.SerialNumber,
            entity.ConnectionState,
            entity.State,
            entity.Factsheet);
    }
}