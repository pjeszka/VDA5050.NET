using VDA5050.NET.Internal.VdaDomain.Robots;
using VDA5050.NET.Public.Events;
using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Models.InstantActions;
using VDA5050.NET.Public.Models.Orders;
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
    void AddRobotStateChangeHandler(EventHandler<RobotStateChangedEvent> robotStateChangedHandler);
    void AddRobotPositionChangedHandler(EventHandler<RobotPositionChangedEvent> robotPositionChangedHandler);
    
    // ordering
    Task<OrderId> SendRobotOrder(RobotOrder robotOrder);
    Task UpdateRobotOrder(RobotOrderUpdate robotOrderUpdate);
    Task CancelRobotOrder(RobotSerialNumber robotSerialNumber, OrderId orderId);
    event EventHandler<RobotStateChangedEvent> RobotOrderStateChanged;
    
    // instantActions
    Task RequestInstantAction(RobotInstantActionRequest request) ;
}