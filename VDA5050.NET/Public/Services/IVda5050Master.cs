using VDA5050.NET.Internal.VdaDomain.Robots;
using VDA5050.NET.Public.Models;

namespace VDA5050.NET.Public.Services;

public interface IVda5050Master
{
    Task AddRobot(RobotSettings robotSettings);
    Task RemoveRobot(RobotNetworkName robotNetworkName);
    event EventHandler<RobotStateChangedEvent> RobotStateChanged;

    Task SendRobotOrder(RobotOrder robotOrder);
    Task UpdateRobotOrder();
}