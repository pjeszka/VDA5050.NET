using Microsoft.Extensions.Logging;
using VDA5050.NET.Internal.MQTT;
using VDA5050.NET.Internal.VdaDomain.InstantActions;
using VDA5050.NET.Internal.VdaDomain.RobotDiscovery;
using VDA5050.NET.Internal.VdaDomain.RobotOrders;
using VDA5050.NET.Internal.VdaDomain.RobotOrders.OrderRequesting;
using VDA5050.NET.Internal.VdaDomain.Robots;
using VDA5050.NET.Public.Enums.Vda5050.State;
using VDA5050.NET.Public.Events;
using VDA5050.NET.Public.Exceptions;
using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Models.InstantActions;
using VDA5050.NET.Public.Models.Orders;
using VDA5050.NET.Public.Models.Robots;
using VDA5050.NET.Public.Services;
using Action = VDA5050.NET.Public.Models.InstantActions.Action;

namespace VDA5050.NET.Internal.VdaDomain.Master;

internal sealed class Vda5050Master : IVda5050Master
{
    private readonly IDiscoveredRobotRepository _discoveredRobotRepository;
    private readonly IOperationalRobotRepository _operationalRobotRepository;
    private readonly IMqttConnection _mqttConnection;
    private readonly IRobotOrderSender _robotOrderSender;
    private readonly IRobotInstantActionsSender _robotInstantActionsSender;
    private readonly IRobotOrderRequestStateRepository _robotOrderRequestStateRepository;
    private readonly ILoggerFactory _loggerFactory;
    private readonly ISystemClock _systemClock;
    private readonly ILogger<Vda5050Master> _logger;

    public Vda5050Master(
        IDiscoveredRobotRepository discoveredRobotRepository,
        IOperationalRobotRepository operationalRobotRepository,
        IMqttConnection mqttConnection,
        IRobotOrderSender robotOrderSender,
        IRobotInstantActionsSender robotInstantActionsSender,
        ILogger<Vda5050Master> logger,
        ILoggerFactory loggerFactory,
        IRobotOrderRequestStateRepository robotOrderRequestStateRepository,
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
    }

    public Task<ICollection<OperationalRobotDetails>> GetOperationalRobots()
    {
        return Task.FromResult<ICollection<OperationalRobotDetails>>(
            _operationalRobotRepository.GetRobots().Select(OperationalRobotDetails.FromEntity).ToList());
    }

    public Task<DiscoveredRobotDetails?> GetAccessibleRobot(RobotSerialNumber robotSerialNumber)
    {
        return Task.FromResult(DiscoveredRobotDetails.Create(_discoveredRobotRepository.GetRobot(robotSerialNumber)));
    }

    public Task<ICollection<DiscoveredRobotDetails>> GetAccessibleRobots()
    {
        var robots = _discoveredRobotRepository
            .GetDiscoveredRobots()
            .Select(x => DiscoveredRobotDetails.Create(x)!)
            .ToList();
        return Task.FromResult<ICollection<DiscoveredRobotDetails>>(robots);
    }

    public async Task StartRobotOperation(RobotSettings robotSettings)
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

    public async Task StopRobotOperation(RobotSerialNumber robotSerialNumber)
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
    
    private event EventHandler<RobotConnectionStateChangedEvent>? RobotConnectionStateChanged;
    
    private event EventHandler<RobotStateChangedEvent>? RobotStateChanged;
    
    public void AddRobotPositionChangedHandler(EventHandler<RobotPositionChangedEvent> robotPositionChangedHandler)
    {
        RobotPositionChanged += robotPositionChangedHandler;
    }

    public async Task<OrderId> SendRobotOrder(RobotOrderRequest robotOrderRequest)
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
            RobotOrderRequestStateChanged.Invoke(this, new RobotOrderRequestStateChanged(
                robotOrderRequest.RobotSerialNumber,
                orderId,
                orderUpdateId,
                status,
                message));
        }
    }

    public async Task<OrderUpdateId> UpdateRobotOrder(RobotOrderUpdateRequest robotOrderUpdateRequest)
    {
        var robot = _operationalRobotRepository.GetRobot(robotOrderUpdateRequest.RobotSerialNumber);
        if (robot is null)
        {
            throw new RobotNotOperationalException(robotOrderUpdateRequest.RobotSerialNumber);
        }
        
        var currentOrderUpdateId = robot.OrderState?.OrderUpdateId ?? new OrderUpdateId(0);

        var newOrderUpdateId = new OrderUpdateId(currentOrderUpdateId.Value + 1);
        SetOrderUpdateRequestStatus(newOrderUpdateId, robotOrderUpdateRequest, OrderRequestStatus.Requested);

        if (robot.OrderState?.OrderId != robotOrderUpdateRequest.Request.OrderId)
        {
            const string messageFormat = "Cannot update order. Robot {0} is not assigned to order {1}. Its current order id is {2}.";
            var message = string.Format(messageFormat, robot.SerialNumber.Value, robotOrderUpdateRequest.Request.OrderId, robot.OrderState?.OrderId);
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
                robotOrderUpdateRequest.Request.OrderId!,
                orderUpdateId,
                status,
                message,
                sentTimeStamp);
            RobotOrderRequestStateChanged.Invoke(this, new RobotOrderRequestStateChanged(
                robotOrderUpdateRequest.RobotSerialNumber,
                robotOrderUpdateRequest.Request.OrderId!,
                orderUpdateId,
                status,
                message));
        }
    }

    public async Task<ActionId> CancelRobotOrder(RobotSerialNumber robotSerialNumber, OrderId orderId)
    {
        var robot = _operationalRobotRepository.GetRobot(robotSerialNumber);
        if (robot is null)
        {
            throw new RobotNotOperationalException(robotSerialNumber);
        }

        var cancelActionId = ActionId.New();
        var actionList = new List<Action>()
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

    public void AddRobotOrderRequestStateChangeHandler(EventHandler<RobotOrderRequestStateChanged> robotOrderRequestStateChangedHandler)
    {
        RobotOrderRequestStateChanged += robotOrderRequestStateChangedHandler;
    }
    
    public async Task<ICollection<ActionId>> RequestInstantAction(RobotInstantActionRequest request)
    {
        var robot = _operationalRobotRepository.GetRobot(request.RobotSerialNumber);
        if (robot is null)
        {
            throw new RobotNotOperationalException(request.RobotSerialNumber);
        }
        
        var actionIds = await _robotInstantActionsSender.SendInstantAction(robot, request);
        
        return actionIds;
    }

    public void AddInstantActionStateChangedHandler(EventHandler<RobotInstantActionStateChanged> robotOrderStateChangedHandler)
    {
        RobotInstantActionStateChanged += robotOrderStateChangedHandler;
    }

    private event EventHandler<RobotPositionChangedEvent>? RobotPositionChanged;
    private event EventHandler<RobotOrderStateChangedEvent>? RobotOrderStateChanged;
    private event EventHandler<RobotOrderRequestStateChanged> RobotOrderRequestStateChanged;
    private event EventHandler<RobotInstantActionStateChanged> RobotInstantActionStateChanged;
    
    private void OnRobotOrderStateChanged(object? sender, RobotOrderStateChangedEvent e)
    {
        RobotOrderStateChanged?.Invoke(sender, e);
        
        // checking order requests
        _robotOrderRequestStateRepository.Clear();
        var waitingOrderRequestStates = _robotOrderRequestStateRepository
            .GetAllForRobot(e.SerialNumber)
            .Where(x => x.Status == OrderRequestStatus.Sent)
            .ToList();
 
        foreach (var waitingOrderRequestState in waitingOrderRequestStates)
        {
            if (waitingOrderRequestState.Id.OrderId == e.OrderState.OrderId &&
                waitingOrderRequestState.Id.OrderUpdateId == e.OrderState.OrderUpdateId)
            {
                RobotOrderRequestStateChanged.Invoke(
                    this,
                    new RobotOrderRequestStateChanged(
                        e.SerialNumber,
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
                RobotOrderRequestStateChanged.Invoke(
                    this,
                    new RobotOrderRequestStateChanged(
                        e.SerialNumber,
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
        
        // TODO checking instantAction requests
        // 1. keep track of instantAction requests
        // 2. check if instantAction status has changed
        
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
}