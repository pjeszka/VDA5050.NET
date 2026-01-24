using VDA5050.NET.Public.Enums.Domain;
using VDA5050.NET.Public.Events;
using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Models.Errors;
using VDA5050.NET.Public.Models.InstantActions;
using VDA5050.NET.Public.Models.Orders;
using VDA5050.NET.Public.Models.Orders.OrderRequesting;
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
    Task<ActionId> CancelRobotOrder(RobotSerialNumber robotSerialNumber, OrderId orderId);
    void AddRobotOrderStateChangeHandler(EventHandler<RobotOrderStateChangedEvent> robotOrderStateChangedHandler);
    void AddRobotOrderRequestStateChangeHandler(EventHandler<RobotOrderRequestStateChanged> robotOrderRequestStateChangedHandler);
    
    // instantActions
    Task<ICollection<ActionId>> RequestInstantAction(RobotInstantActionRequest request);
    void AddInstantActionStateChangedHandler(EventHandler<RobotInstantActionStateChanged> robotOrderStateChangedHandler);
    
    // errors 
    Task<ICollection<ErrorSpecifics>?> GetRobotErrors(RobotSerialNumber robotSerialNumber);
    Task<ICollection<ErrorSpecifics>?> GetRobotErrorsFor(RobotSerialNumber robotSerialNumber, ErrorReferenceType errorReferenceType, string referenceId);
    
    // generics
    void AddRobotEventHandler<T>(EventHandler<T> robotEventChangeHandler) where T : IRobotEvent;
}