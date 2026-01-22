using VDA5050.NET.Public.Events;
using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Models.InstantActions;
using VDA5050.NET.Public.Models.Orders;
using VDA5050.NET.Public.Models.Robots;

namespace VDA5050.NET.Public.Services;

public interface IVda5050Master
{
    // robot discovery
    Task<DiscoveredRobotDetails?> GetAccessibleRobot(RobotSerialNumber robotSerialNumber);
    Task<ICollection<DiscoveredRobotDetails>> GetAccessibleRobots();
    // TODO event about discovering robot
    
    // robot management
    Task<ICollection<OperationalRobotDetails>> GetOperationalRobots();
    Task StartRobotOperation(RobotSettings robotSettings);
    Task StopRobotOperation(RobotSerialNumber robotSerialNumber);
    
    // robot state observing
    void AddRobotConnectionStateChangeHandler(EventHandler<RobotConnectionStateChangedEvent> robotConnectionStateChangedHandler);
    void AddRobotStateChangeHandler(EventHandler<RobotStateChangedEvent> robotStateChangedHandler);
    void AddRobotPositionChangedHandler(EventHandler<RobotPositionChangedEvent> robotPositionChangedHandler);
    
    // ordering
    Task<OrderId> SendRobotOrder(RobotOrderRequest robotOrderRequest);
    Task<OrderUpdateId> UpdateRobotOrder(RobotOrderUpdateRequest robotOrderUpdateRequest);
    Task CancelRobotOrder(RobotSerialNumber robotSerialNumber, OrderId orderId);
    event EventHandler<RobotStateChangedEvent> RobotOrderStateChanged;
    event EventHandler<RobotOrderRequestStateChanged> RobotOrderRequestStateChanged;
    
    // instantActions
    Task<ActionId> RequestInstantAction(RobotInstantActionRequest request);
}