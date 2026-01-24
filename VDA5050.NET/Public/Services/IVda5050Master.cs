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
    Task<DiscoveredRobotDetails?> GetAccessibleRobot(RobotSerialNumber robotSerialNumber, CancellationToken cancellationToken);
    Task<ICollection<DiscoveredRobotDetails>> GetAccessibleRobots(CancellationToken cancellationToken);
    // TODO event about discovering robot
    
    // robot management
    Task<ICollection<OperationalRobotDetails>> GetOperationalRobots(CancellationToken cancellationToken);
    Task StartRobotOperation(RobotSettings robotSettings, CancellationToken cancellationToken);
    Task StopRobotOperation(RobotSerialNumber robotSerialNumber, CancellationToken cancellationToken);
    
    // robot state observing
    void AddRobotConnectionStateChangeHandler(EventHandler<RobotConnectionStateChangedEvent> robotConnectionStateChangedHandler);
    void AddRobotStateChangeHandler(EventHandler<RobotStateChangedEvent> robotStateChangedHandler);
    void AddRobotPositionChangedHandler(EventHandler<RobotPositionChangedEvent> robotPositionChangedHandler);
    
    // ordering
    Task<OrderId> RequestRobotOrder(RobotOrderRequest robotOrderRequest, CancellationToken cancellationToken);
    Task<OrderUpdateId> RequestRobotOrderUpdate(RobotOrderUpdateRequest robotOrderUpdateRequest, CancellationToken cancellationToken);
    Task<ActionId> CancelRobotOrder(RobotSerialNumber robotSerialNumber, OrderId orderId, CancellationToken cancellationToken);
    void AddRobotOrderStateChangeHandler(EventHandler<RobotOrderStateChangedEvent> robotOrderStateChangedHandler);
    void AddRobotOrderRequestStateChangeHandler(EventHandler<RobotOrderRequestStateChangedEvent> robotOrderRequestStateChangedHandler);
    
    // instantActions
    Task<ICollection<ActionId>> RequestInstantAction(RobotInstantActionRequest request, CancellationToken cancellationToken);
    void AddInstantActionStateChangedHandler(EventHandler<RobotInstantActionStateChangedEvent> robotOrderStateChangedHandler);
    
    // errors 
    Task<ICollection<ErrorSpecifics>?> GetRobotErrors(RobotSerialNumber robotSerialNumber, CancellationToken cancellationToken);
    Task<ICollection<ErrorSpecifics>?> GetRobotErrorsFor(
        RobotSerialNumber robotSerialNumber,
        ErrorReferenceType errorReferenceType,
        string referenceId,
        CancellationToken cancellationToken);
    
    // generics
    void AddRobotEventHandler<T>(EventHandler<T> robotEventChangeHandler) where T : IRobotEvent;
}