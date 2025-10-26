using VDA5050.NET.Internal.VdaDomain.RobotDiscovery;
using VDA5050.NET.Internal.VdaDomain.Robots;
using VDA5050.NET.Public.Models;

namespace VDA5050.NET.Public.Services;

public interface IVda5050Master
{
    // robot discovery
    Task<ICollection<DiscoveredRobot>> GetAccessibleRobots();
    
    // robot management
    Task<ICollection<RobotSerialNumber>> GetConnectedRobots();
    Task ConnectRobot(RobotSettings robotSettings);
    Task DisconnectRobot(RobotSerialNumber robotSerialNumber);
    
    // robot state observing
    event EventHandler<RobotStateChangedEvent> RobotStateChanged;
    // robot visualization
    // TODO: implement
    
    // ordering
    Task SendRobotOrder(RobotOrder robotOrder);
    Task UpdateRobotOrder();
    event EventHandler<RobotStateChangedEvent> RobotOrderStateChanged;
    
    // instantActions
    Task RequestInstantAction();
}