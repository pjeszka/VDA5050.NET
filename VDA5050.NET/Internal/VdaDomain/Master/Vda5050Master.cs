using Microsoft.Extensions.Logging;
using VDA5050.NET.Internal.MQTT;
using VDA5050.NET.Internal.VdaDomain.Master.InstantActions;
using VDA5050.NET.Internal.VdaDomain.Master.Robots;
using VDA5050.NET.Internal.VdaDomain.RobotDiscovery;
using VDA5050.NET.Internal.VdaDomain.RobotOrders.OrderRequesting;
using VDA5050.NET.Internal.VdaDomain.Robots;
using VDA5050.NET.Public.Enums.Domain;
using VDA5050.NET.Public.Enums.Vda5050.State;
using VDA5050.NET.Public.Events;
using VDA5050.NET.Public.Exceptions;
using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Models.Errors;
using VDA5050.NET.Public.Models.InstantActions;
using VDA5050.NET.Public.Models.Orders;
using VDA5050.NET.Public.Models.Orders.OrderRequesting;
using VDA5050.NET.Public.Models.Robots;
using VDA5050.NET.Public.Services;
using VDA5050.NET.Public.Services.Master;
using Action = VDA5050.NET.Public.Models.InstantActions.Action;

namespace VDA5050.NET.Internal.VdaDomain.Master;

internal sealed class Vda5050Master : IVda5050Master
{
    private readonly IDiscoveredRobotRepository _discoveredRobotRepository;
    private readonly IOperationalRobotRepository _operationalRobotRepository;
    private readonly IMqttConnection _mqttConnection;
    
    private readonly IRobotOrderSender _robotOrderSender;
    private readonly IRobotOrderRequestStateRepository _robotOrderRequestStateRepository;
    
    private readonly IRobotInstantActionsSender _robotInstantActionsSender;
    private readonly IInstantActionRequestRepository _instantActionRequestRepository;

    private readonly ILoggerFactory _loggerFactory;
    private readonly ISystemClock _systemClock;
    private readonly ILogger<Vda5050Master> _logger;

    public Vda5050Master(
        IDiscoveredRobotRepository discoveredRobotRepository,
        IOperationalRobotRepository operationalRobotRepository,
        IMqttConnection mqttConnection,
        IRobotOrderSender robotOrderSender,
        IRobotOrderRequestStateRepository robotOrderRequestStateRepository,
        IRobotInstantActionsSender robotInstantActionsSender,
        IInstantActionRequestRepository instantActionRequestRepository,
        ILogger<Vda5050Master> logger,
        ILoggerFactory loggerFactory,
        ISystemClock systemClock)
    {
        _discoveredRobotRepository = discoveredRobotRepository;
        _operationalRobotRepository = operationalRobotRepository;
        _robotOrderSender = robotOrderSender;
        _robotInstantActionsSender = robotInstantActionsSender;
        _mqttConnection = mqttConnection;
        _logger = logger;
        _loggerFactory = loggerFactory;
        _robotOrderRequestStateRepository = robotOrderRequestStateRepository;
        _systemClock = systemClock;
        _instantActionRequestRepository = instantActionRequestRepository;
    }

    public Task<ICollection<OperationalRobotDetails>> GetOperationalRobots(CancellationToken cancellationToken)
    {
        return Task.FromResult<ICollection<OperationalRobotDetails>>(
            _operationalRobotRepository.GetRobots().Select(OperationalRobotDetails.FromEntity).ToList());
    }

    public Task<DiscoveredRobotDetails?> GetAccessibleRobot(RobotSerialNumber robotSerialNumber, CancellationToken cancellationToken)
    {
        return Task.FromResult(DiscoveredRobotDetails.Create(_discoveredRobotRepository.GetRobot(robotSerialNumber)));
    }

    public Task<ICollection<DiscoveredRobotDetails>> GetAccessibleRobots(CancellationToken cancellationToken)
    {
        var robots = _discoveredRobotRepository
            .GetDiscoveredRobots()
            .Select(x => DiscoveredRobotDetails.Create(x)!)
            .ToList();
        return Task.FromResult<ICollection<DiscoveredRobotDetails>>(robots);
    }

    public async Task StartRobotOperation(RobotSettings robotSettings, CancellationToken cancellationToken)
    {
        if (_operationalRobotRepository.IsRobotOperational(robotSettings.RobotSerialNumber))
        {
            _logger.LogWarning("Robot {robotSerialNumber} operation is already started.", robotSettings.RobotSerialNumber.Value);
            return;
        }

        var robotLogger = _loggerFactory.CreateLogger($"Robot-{robotSettings.RobotSerialNumber.Value}");
        var connectedRobot = new OperationalRobot(robotSettings, robotLogger);
        connectedRobot.AddConnectionStateChangeHandler(OnRobotConnectionStateChanged);
        connectedRobot.AddStateChangeHandler(OnRobotStateChanged);
        connectedRobot.AddPositionChangeHandler(OnRobotPositionChanged);
        connectedRobot.AddOrderStateChangeHandler(OnRobotOrderStateChanged);
        _operationalRobotRepository.AddRobot(connectedRobot);

        foreach (var topic in connectedRobot.ObservedTopics)
        {
            await _mqttConnection.AddSubscription(topic);
        }
        
        _logger.LogInformation("Robot {robotSerialNumber} was added to operation.", robotSettings.RobotSerialNumber.Value);
        
    }

    public async Task StopRobotOperation(RobotSerialNumber robotSerialNumber, CancellationToken cancellationToken)
    {
        var robot = _operationalRobotRepository.GetRobot(robotSerialNumber);

        if (robot is null)
        {
            _logger.LogWarning("Robot {robotSerialNumber} operation is already stopped.", robotSerialNumber.Value);
            return;
        }

        foreach (var topic in robot.ObservedTopics)
        {
            await _mqttConnection.RemoveSubscription(topic);
        }

        _operationalRobotRepository.RemoveRobot(robotSerialNumber);
        _logger.LogInformation("Robot {robotSerialNumber} was removed from operation.", robotSerialNumber.Value);
    }

    public void AddRobotConnectionStateChangeHandler(EventHandler<RobotConnectionStateChangedEvent> robotConnectionStateChangedHandler)
    {
        RobotConnectionStateChanged += robotConnectionStateChangedHandler;
    }

    public void AddRobotStateChangeHandler(EventHandler<RobotStateChangedEvent> robotStateChangedHandler)
    {
        RobotStateChanged += robotStateChangedHandler;
    }
    
    public void AddRobotPositionChangedHandler(EventHandler<RobotPositionChangedEvent> robotPositionChangedHandler)
    {
        RobotPositionChanged += robotPositionChangedHandler;
    }

    public async Task<OrderId> RequestRobotOrder(RobotOrderRequest robotOrderRequest, CancellationToken cancellationToken)
    {
        var robot = _operationalRobotRepository.GetRobot(robotOrderRequest.RobotSerialNumber);
        if (robot is null)
        {
            throw new RobotNotOperationalException(robotOrderRequest.RobotSerialNumber);
        }
        var orderId = new OrderId(Guid.NewGuid().ToString());
        SetOrderRequestStatus(orderId, robotOrderRequest, OrderRequestStatus.Requested);

        try
        {
            await _robotOrderSender.SendOrder(robot, robotOrderRequest, orderId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while sending order");
            SetOrderRequestStatus(orderId, robotOrderRequest, OrderRequestStatus.Invalid, e.Message);
            throw;
        }
        
        SetOrderRequestStatus(orderId, robotOrderRequest, OrderRequestStatus.Sent);
        
        return orderId;
    }

    public async Task<OrderUpdateId> RequestRobotOrderUpdate(RobotOrderUpdateRequest robotOrderUpdateRequest, CancellationToken cancellationToken)
    {
        var robot = _operationalRobotRepository.GetRobot(robotOrderUpdateRequest.RobotSerialNumber);
        if (robot is null)
        {
            throw new RobotNotOperationalException(robotOrderUpdateRequest.RobotSerialNumber);
        }
        
        var currentOrderUpdateId = robot.OrderState?.OrderUpdateId ?? new OrderUpdateId(0);

        var newOrderUpdateId = new OrderUpdateId(currentOrderUpdateId.Value + 1);
        SetOrderUpdateRequestStatus(newOrderUpdateId, robotOrderUpdateRequest, OrderRequestStatus.Requested);

        if (robot.OrderState?.OrderId != robotOrderUpdateRequest.OrderId)
        {
            const string messageFormat = "Cannot update order. Robot {0} is not assigned to order {1}. Its current order id is {2}.";
            var message = string.Format(messageFormat, robot.SerialNumber.Value, robotOrderUpdateRequest.OrderId, robot.OrderState?.OrderId);
            SetOrderUpdateRequestStatus(newOrderUpdateId, robotOrderUpdateRequest, OrderRequestStatus.Invalid, message);
            throw new InvalidOperationException(message);
        }
        
        if (robot.OrderState?.OrderId is null)
        {
            const string messageFormat = "Cannot update order. Robot {0} has no active order.";
            var message = string.Format(messageFormat, robot.SerialNumber.Value);
            SetOrderUpdateRequestStatus(newOrderUpdateId, robotOrderUpdateRequest, OrderRequestStatus.Invalid, message);
            throw new InvalidOperationException(message);
        }

        try
        {
            await _robotOrderSender.SendOrderUpdate(robot, robotOrderUpdateRequest, newOrderUpdateId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while sending order update");
            SetOrderUpdateRequestStatus(newOrderUpdateId, robotOrderUpdateRequest, OrderRequestStatus.Invalid, e.Message);
            throw;
        }
        
        SetOrderUpdateRequestStatus(newOrderUpdateId, robotOrderUpdateRequest, OrderRequestStatus.Sent);
        return newOrderUpdateId;
    }

    public async Task<ActionId> CancelRobotOrder(RobotSerialNumber robotSerialNumber, OrderId orderId, CancellationToken cancellationToken)
    {
        var robot = _operationalRobotRepository.GetRobot(robotSerialNumber);
        if (robot is null)
        {
            throw new RobotNotOperationalException(robotSerialNumber);
        }

        var cancelActionId = ActionId.New();
        var actionList = new List<Action>
        {
            new(ActionId.New(), SpecificInstantActionTypes.CancelOrder, BlockingType.NONE, new List<Parameter>())
        };
        await _robotInstantActionsSender.SendInstantAction(robot,
            new RobotInstantActionRequest(robotSerialNumber, actionList));
        
        robot.InitOrderCancellation(orderId, cancelActionId);
        
        return cancelActionId;
    }

    public void AddRobotOrderStateChangeHandler(EventHandler<RobotOrderStateChangedEvent> robotOrderStateChangedHandler)
    {
        RobotOrderStateChanged += robotOrderStateChangedHandler;
    }

    public void AddRobotOrderRequestStateChangeHandler(EventHandler<RobotOrderRequestStateChangedEvent> robotOrderRequestStateChangedHandler)
    {
        RobotOrderRequestStateChanged += robotOrderRequestStateChangedHandler;
    }
    
    public async Task<ICollection<ActionId>> RequestInstantAction(RobotInstantActionRequest request, CancellationToken cancellationToken)
    {
        var robot = _operationalRobotRepository.GetRobot(request.RobotSerialNumber);
        if (robot is null)
        {
            throw new RobotNotOperationalException(request.RobotSerialNumber);
        }
        
        var actionIds = await _robotInstantActionsSender.SendInstantAction(robot, request);

        foreach (var actionId in actionIds)
        {
            var actionState = new InstantActionRequestState(request.RobotSerialNumber, actionId, ActionStatus.WAITING);
            _instantActionRequestRepository.AddInstantActionRequest(actionState);
        }

        return actionIds;
    }

    public void AddInstantActionStateChangedHandler(EventHandler<RobotInstantActionStateChangedEvent> robotOrderStateChangedHandler)
    {
        RobotInstantActionStateChanged += robotOrderStateChangedHandler;
    }

    public Task<ICollection<ErrorSpecifics>?> GetRobotErrors(RobotSerialNumber robotSerialNumber, CancellationToken cancellationToken)
    {
        var robot = _operationalRobotRepository.GetRobot(robotSerialNumber);
        if (robot is null)
        {
            throw new RobotNotOperationalException(robotSerialNumber);
        }
        
        return Task.FromResult(robot.Errors);
    }

    public Task<ICollection<ErrorSpecifics>?> GetRobotErrorsFor(RobotSerialNumber robotSerialNumber, ErrorReferenceType errorReferenceType, string referenceId, CancellationToken cancellationToken)
    {
        var robot = _operationalRobotRepository.GetRobot(robotSerialNumber);
        if (robot is null)
        {
            throw new RobotNotOperationalException(robotSerialNumber);
        }
        
        var errors = robot.Errors?
            .Where(x => 
                x.ErrorReferences != null &&
                x.ErrorReferences
                    .Any(e => e.Type == errorReferenceType && e.Id == referenceId))
            .ToList();

        return Task.FromResult<ICollection<ErrorSpecifics>?>(errors);
    }

    public void AddRobotEventHandler<T>(EventHandler<T> robotEventChangeHandler) where T : IRobotEvent
    {
        if (robotEventChangeHandler is EventHandler<RobotConnectionStateChangedEvent> robotConnectionStateChangedEventHandler)
        {
            RobotConnectionStateChanged += robotConnectionStateChangedEventHandler;
        }
        
        if (robotEventChangeHandler is EventHandler<RobotStateChangedEvent> robotStateChangedEventHandler)
        {
            RobotStateChanged += robotStateChangedEventHandler;
        }
        
        if (robotEventChangeHandler is EventHandler<RobotPositionChangedEvent> robotPositionChangedEventHandler)
        {
            RobotPositionChanged += robotPositionChangedEventHandler;
        }
        
        if (robotEventChangeHandler is EventHandler<RobotOrderStateChangedEvent> robotOrderStateChangedEventHandler)
        {
            RobotOrderStateChanged += robotOrderStateChangedEventHandler;
        }
        
        if (robotEventChangeHandler is EventHandler<RobotOrderRequestStateChangedEvent> robotOrderRequestStateChangedEventHandler)
        {
            RobotOrderRequestStateChanged += robotOrderRequestStateChangedEventHandler;
        }
        
        if (robotEventChangeHandler is EventHandler<RobotInstantActionStateChangedEvent> robotInstantActionStateChangedEventHandler)
        {
            RobotInstantActionStateChanged += robotInstantActionStateChangedEventHandler;
        }
    }
    
    private event EventHandler<RobotConnectionStateChangedEvent>? RobotConnectionStateChanged;
    private event EventHandler<RobotStateChangedEvent>? RobotStateChanged;
    private event EventHandler<RobotPositionChangedEvent>? RobotPositionChanged;
    private event EventHandler<RobotOrderStateChangedEvent>? RobotOrderStateChanged;
    private event EventHandler<RobotOrderRequestStateChangedEvent>? RobotOrderRequestStateChanged;
    private event EventHandler<RobotInstantActionStateChangedEvent>? RobotInstantActionStateChanged;
    
    private void OnRobotOrderStateChanged(object? sender, RobotOrderStateChangedEvent e)
    {
        RobotOrderStateChanged?.Invoke(sender, e);
        
        // checking order requests
        _robotOrderRequestStateRepository.Clear();
        var waitingOrderRequestStates = _robotOrderRequestStateRepository
            .GetAllForRobot(e.RobotSerialNumber)
            .Where(x => x.Status == OrderRequestStatus.Sent)
            .ToList();
 
        foreach (var waitingOrderRequestState in waitingOrderRequestStates)
        {
            if (waitingOrderRequestState.Id.OrderId == e.OrderState.OrderId &&
                waitingOrderRequestState.Id.OrderUpdateId == e.OrderState.OrderUpdateId)
            {
                RobotOrderRequestStateChanged?.Invoke(
                    this,
                    new RobotOrderRequestStateChangedEvent(
                        e.RobotSerialNumber,
                        waitingOrderRequestState.Id.OrderId,
                        waitingOrderRequestState.Id.OrderUpdateId,
                        OrderRequestStatus.Accepted));
                _robotOrderRequestStateRepository.UpdateOrderRequestStatus(
                    waitingOrderRequestState.Id.OrderId,
                    waitingOrderRequestState.Id.OrderUpdateId,
                    OrderRequestStatus.Accepted);
            }
            else if(_systemClock.Now - waitingOrderRequestState.SentAt > TimeSpan.FromSeconds(10))
            {
                const string message = "Timeout while waiting for order confirmation";
                RobotOrderRequestStateChanged?.Invoke(
                    this,
                    new RobotOrderRequestStateChangedEvent(
                        e.RobotSerialNumber,
                        waitingOrderRequestState.Id.OrderId,
                        waitingOrderRequestState.Id.OrderUpdateId,
                        OrderRequestStatus.Rejected,
                        message));
                _robotOrderRequestStateRepository.UpdateOrderRequestStatus(
                    waitingOrderRequestState.Id.OrderId,
                    waitingOrderRequestState.Id.OrderUpdateId,
                    OrderRequestStatus.Rejected,
                    message);
            }
        }
        
        // checking instant actions
        _instantActionRequestRepository.Clear();
        var instantActionRequestStates = _instantActionRequestRepository.GetAllForRobot(e.RobotSerialNumber);
        foreach(var instantActionRequestState in instantActionRequestStates)
        {
            var actionState = e.OrderState.ActionStates
                    .FirstOrDefault(x => x.Id == instantActionRequestState.ActionId.Value);
            if (actionState is null)
            {
                _logger.LogWarning(
                    "Instant action {actionId} not found in order state for robot {serialNumber}",
                    instantActionRequestState.ActionId,
                    e.RobotSerialNumber);    
            }
            else
            {
                if (_instantActionRequestRepository.TryUpdateInstantActionRequestStatus(
                        instantActionRequestState.ActionId, actionState.Status))
                {
                    RobotInstantActionStateChanged?.Invoke(
                        this,
                        new RobotInstantActionStateChangedEvent(
                            e.RobotSerialNumber,
                            instantActionRequestState.ActionId,
                            actionState.Status));
                }
            }
        }
        
    }

    private void OnRobotPositionChanged(object? sender, RobotPositionChangedEvent e)
    {
        RobotPositionChanged?.Invoke(sender, e);
    }
    
    private void OnRobotStateChanged(object? sender, RobotStateChangedEvent e)
    {
        _logger.LogDebug(
            "Robot {robotSerialNumber} state changed to {state}",
            e.RobotSerialNumber.Value,
            e.State.ToString());
        RobotStateChanged?.Invoke(sender, e);
    }

    private void OnRobotConnectionStateChanged(object? sender, RobotConnectionStateChangedEvent e)
    {
        _logger.LogWarning(
            "Robot {robotSerialNumber} connection state changed from {previousConnectionState} to {newConnectionState}",
            e.RobotSerialNumber.Value,
            e.PreviousConnectionState,
            e.NewConnectionState);
        RobotConnectionStateChanged?.Invoke(sender, e);
    }
    
    private void SetOrderRequestStatus(
        OrderId orderId,
        RobotOrderRequest robotOrderRequest,
        OrderRequestStatus status,
        string? message = null)
    {
        if (status == OrderRequestStatus.Requested)
        {
            var orderRequestState = new OrderRequestState(orderId, robotOrderRequest);
            _robotOrderRequestStateRepository.AddOrderRequest(orderRequestState);
        }
        else
        {
            var sentTimeStamp = status == OrderRequestStatus.Sent ? _systemClock.Now : (DateTime?)null;
            var orderUpdateId = new OrderUpdateId(0);
            _robotOrderRequestStateRepository.UpdateOrderRequestStatus(orderId, orderUpdateId, status, message, sentTimeStamp);
            RobotOrderRequestStateChanged?.Invoke(this, new RobotOrderRequestStateChangedEvent(
                robotOrderRequest.RobotSerialNumber,
                orderId,
                orderUpdateId,
                status,
                message));
        }
    }
    
    private void SetOrderUpdateRequestStatus(
        OrderUpdateId orderUpdateId,
        RobotOrderUpdateRequest robotOrderUpdateRequest,
        OrderRequestStatus status,
        string? message = null)
    {
        if (status == OrderRequestStatus.Requested)
        {
            var orderRequestState = new OrderRequestState(orderUpdateId, robotOrderUpdateRequest);
            _robotOrderRequestStateRepository.AddOrderRequest(orderRequestState);
        }
        else
        {
            var sentTimeStamp = status == OrderRequestStatus.Sent ? _systemClock.Now : (DateTime?)null;
            _robotOrderRequestStateRepository.UpdateOrderRequestStatus(
                robotOrderUpdateRequest.OrderId!,
                orderUpdateId,
                status,
                message,
                sentTimeStamp);
            RobotOrderRequestStateChanged?.Invoke(this, new RobotOrderRequestStateChangedEvent(
                robotOrderUpdateRequest.RobotSerialNumber,
                robotOrderUpdateRequest.OrderId!,
                orderUpdateId,
                status,
                message));
        }
    }
}