using Microsoft.Extensions.Logging;
using VDA5050.NET.Internal.MQTT;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts;
using VDA5050.NET.Internal.VdaDomain.RobotDiscovery;
using VDA5050.NET.Internal.VdaDomain.RobotOrders;
using VDA5050.NET.Internal.VdaDomain.RobotOrders.OrderRequesting;
using VDA5050.NET.Internal.VdaDomain.Robots;
using VDA5050.NET.Public.Events;
using VDA5050.NET.Public.Exceptions;
using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Models.InstantActions;
using VDA5050.NET.Public.Models.Orders;
using VDA5050.NET.Public.Models.Robots;
using VDA5050.NET.Public.Services;

namespace VDA5050.NET.Internal.VdaDomain.Master;

internal sealed class Vda5050Master : IVda5050Master
{
    private readonly IDiscoveredRobotRepository _discoveredRobotRepository;
    private readonly IOperationalRobotRepository _operationalRobotRepository;
    private readonly IMqttConnection _mqttConnection;
    private readonly IRobotOrderSender _robotOrderSender;
    private readonly ILogger<Vda5050Master> _logger;

    public Vda5050Master(
        IDiscoveredRobotRepository discoveredRobotRepository,
        IOperationalRobotRepository operationalRobotRepository,
        IMqttConnection mqttConnection,
        ILogger<Vda5050Master> logger,
        IRobotOrderSender robotOrderSender)
    {
        _discoveredRobotRepository = discoveredRobotRepository;
        _operationalRobotRepository = operationalRobotRepository;
        _logger = logger;
        _robotOrderSender = robotOrderSender;
        _mqttConnection = mqttConnection;
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

        var connectedRobot = new OperationalRobot(robotSettings);
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
        ValidateRobotIsOperational(robotOrderRequest.RobotSerialNumber);
        
        var robot = _operationalRobotRepository.GetRobot(robotOrderRequest.RobotSerialNumber);
        if (robot is null)
        {
            throw new RobotNotOperationalException(robotOrderRequest.RobotSerialNumber);
        }
        
        var orderId = await _robotOrderSender.SendOrder(robot, robotOrderRequest);
        
        return orderId;
    }

    public async Task<OrderUpdateId> UpdateRobotOrder(RobotOrderUpdateRequest robotOrderUpdateRequest)
    {
        ValidateRobotIsOperational(robotOrderUpdateRequest.RobotSerialNumber);
        
        var robot = _operationalRobotRepository.GetRobot(robotOrderUpdateRequest.RobotSerialNumber);
        if (robot is null)
        {
            throw new RobotNotOperationalException(robotOrderUpdateRequest.RobotSerialNumber);
        }
        
        var orderUpdateId = await _robotOrderSender.SendOrderUpdate(robot, robotOrderUpdateRequest);
        
        return orderUpdateId;
    }

    public async Task<ActionId> CancelRobotOrder(RobotSerialNumber robotSerialNumber, OrderId orderId)
    {
        ValidateRobotIsOperational(robotSerialNumber);
        
        // TODO send instant action to cancel order
        
        re
    }

    public void AddRobotOrderStateChangeHandler(EventHandler<RobotOrderStateChangedEvent> robotOrderStateChangedHandler)
    {
        RobotOrderStateChanged += robotOrderStateChangedHandler;
    }

    public void AddRobotOrderRequestStateChangeHandler(EventHandler<RobotOrderRequestStateChanged> robotOrderRequestStateChangedHandler)
    {
        RobotOrderRequestStateChanged += robotOrderRequestStateChangedHandler;
    }
    
    public async Task<ActionId> RequestInstantAction(RobotInstantActionRequest request)
    {
        ValidateRobotIsOperational(request.RobotSerialNumber);
        
        // TODO send instant action
    }

    public void AddInstantActionStateChangedHandler(EventHandler<RobotOrderStateChangedEvent> robotOrderStateChangedHandler)
    {
        throw new NotImplementedException();
    }

    private event EventHandler<RobotPositionChangedEvent>? RobotPositionChanged;
    private event EventHandler<RobotOrderStateChangedEvent>? RobotOrderStateChanged;
    private event EventHandler<RobotOrderRequestStateChanged> RobotOrderRequestStateChanged;

    private void ValidateRobotIsOperational(RobotSerialNumber robotSerialNumber)
    {
        if (_operationalRobotRepository.IsRobotOperational(robotSerialNumber) is false)
        {
            _logger.LogWarning("Robot {robotSerialNumber} operation is already started.", robotSerialNumber);
            throw new RobotNotOperationalException(robotSerialNumber);
        }
    }
    
    private void OnRobotOrderStateChanged(object? sender, RobotOrderStateChangedEvent e)
    {
        // TODO here check order request - if for any there should be some change of status
        RobotOrderStateChanged?.Invoke(sender, e);
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