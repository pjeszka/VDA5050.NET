using VDA5050.NET.API.Models;
using VDA5050.NET.VdaDomain.Robots;

namespace VDA5050.NET.API.Services;

public interface IVda5050Master
{
    Task AddRobot(RobotSettings robotSettings);
    Task RemoveRobot(RobotNetworkName robotNetworkName);
    event EventHandler<RobotStateChangedEvent> RobotStateChanged;

    Task SendRobotOrder(RobotOrder robotOrder);
    Task UpdateRobotOrder();
}