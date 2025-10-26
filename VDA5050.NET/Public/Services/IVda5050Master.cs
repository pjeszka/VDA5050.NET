using VDA5050.NET.Internal.VdaDomain.Robots;
using VDA5050.NET.Public.Events;
using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Models.RobotDiscovery;

namespace VDA5050.NET.Public.Services;

public interface IVda5050Master
{
    // robot discovery
    Task<DiscoveredRobot?> GetAccessibleRobot(RobotSerialNumber robotSerialNumber);
    Task<ICollection<DiscoveredRobot>> GetAccessibleRobots();
    
    // robot management
    Task<ICollection<OperationalRobot>> GetOperationalRobots();
    Task StartRobotOperation(RobotSettings robotSettings);
    Task StopRobotOperation(RobotSerialNumber robotSerialNumber);
    
    // robot state observing
    void AddRobotConnectionStateChangeHandler(EventHandler<RobotConnectionStateChangedEvent> robotConnectionStateChangedHandler);
    event EventHandler<RobotConnectionStateChangedEvent> RobotConnectionStateChanged;
    void AddRobotStateChangeHandler(EventHandler<RobotStateChangedEvent> robotStateChangedHandler);
    event EventHandler<RobotStateChangedEvent> RobotStateChanged;
    
    // robot visualization
    // TODO: implement
    
    // ordering
    // TODO implement
    // Task SendRobotOrder(RobotOrder robotOrder);
    // Task UpdateRobotOrder();
    // event EventHandler<RobotStateChangedEvent> RobotOrderStateChanged;
    
    // instantActions
    // TODO implement
    // Task RequestInstantAction();
}