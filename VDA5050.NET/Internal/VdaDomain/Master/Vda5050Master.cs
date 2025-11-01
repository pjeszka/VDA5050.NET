using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VDA5050.NET.Internal.MQTT;
using VDA5050.NET.Internal.VdaDomain.RobotDiscovery;
using VDA5050.NET.Internal.VdaDomain.Robots;
using VDA5050.NET.Public.Events;
using VDA5050.NET.Public.Messages;
using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Models.RobotDiscovery;
using VDA5050.NET.Public.Services;

namespace VDA5050.NET.Internal.VdaDomain.Master;

public sealed class Vda5050Master : IVda5050Master
{
    private readonly IDiscoveredRobotRepository _discoveredRobotRepository;
    private readonly IOperationalRobotRepository _operationalRobotRepository;
    private readonly IMqttConnection _mqttConnection;
    private readonly ILogger<Vda5050Master> _logger;

    public Vda5050Master(
        IDiscoveredRobotRepository discoveredRobotRepository,
        IOperationalRobotRepository operationalRobotRepository,
        IMqttConnection mqttConnection,
        ILogger<Vda5050Master> logger)
    {
        _discoveredRobotRepository = discoveredRobotRepository;
        _operationalRobotRepository = operationalRobotRepository;
        _logger = logger;
        _mqttConnection = mqttConnection;
    }

    public Task<ICollection<OperationalRobot>> GetOperationalRobots()
    {
        return _operationalRobotRepository.GetRobots();
    }

    public Task<DiscoveredRobot?> GetAccessibleRobot(RobotSerialNumber robotSerialNumber)
    {
        return Task.FromResult(_discoveredRobotRepository.GetRobot(robotSerialNumber));
    }

    public Task<ICollection<DiscoveredRobot>> GetAccessibleRobots()
    {
        return Task.FromResult(_discoveredRobotRepository.GetDiscoveredRobots());
    }

    public async Task StartRobotOperation(RobotSettings robotSettings)
    {
        if (await _operationalRobotRepository.IsRobotOperational(robotSettings.RobotSerialNumber))
        {
            _logger.LogWarning("Robot {robotSerialNumber} operation is already started.", robotSettings.RobotSerialNumber.Value);
            return;
        }

        var connectedRobot = new OperationalRobot(robotSettings);
        connectedRobot.AddConnectionStateChangeHandler(OnRobotConnectionStateChanged);
        connectedRobot.AddStateChangeHandler(OnRobotStateChanged);
        connectedRobot.AddPositionChangeHandler(OnRobotPositionChanged);
        await _operationalRobotRepository.AddRobot(connectedRobot);

        foreach (var topic in connectedRobot.ObservedTopics)
        {
            await _mqttConnection.AddSubscription(topic);
        }
        
        _logger.LogInformation("Robot {robotSerialNumber} was added to operation.", robotSettings.RobotSerialNumber.Value);
        
    }

    public async Task StopRobotOperation(RobotSerialNumber robotSerialNumber)
    {
        var robot = await _operationalRobotRepository.GetRobot(robotSerialNumber);

        if (robot is null)
        {
            _logger.LogWarning("Robot {robotSerialNumber} operation is already stopped.", robotSerialNumber.Value);
            return;
        }

        foreach (var topic in robot.ObservedTopics)
        {
            await _mqttConnection.RemoveSubscription(topic);
        }

        await _operationalRobotRepository.RemoveRobot(robotSerialNumber);
        _logger.LogInformation("Robot {robotSerialNumber} was removed from operation.", robotSerialNumber.Value);
    }

    public void AddRobotConnectionStateChangeHandler(EventHandler<RobotConnectionStateChangedEvent> robotConnectionStateChangedHandler)
    {
        RobotConnectionStateChanged += robotConnectionStateChangedHandler;
    }

    public event EventHandler<RobotConnectionStateChangedEvent>? RobotConnectionStateChanged;

    public void AddRobotStateChangeHandler(EventHandler<RobotStateChangedEvent> robotStateChangedHandler)
    {
        RobotStateChanged += robotStateChangedHandler;
    }
    
    public event EventHandler<RobotStateChangedEvent>? RobotStateChanged;
    
    public void AddRobotPositionChangedHandler(EventHandler<RobotPositionChangedEvent> robotPositionChangedHandler)
    {
        RobotPositionChanged += robotPositionChangedHandler;
    }

    public event EventHandler<RobotPositionChangedEvent>? RobotPositionChanged;
    // public Task SendRobotOrder(RobotOrder robotOrder)
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public Task UpdateRobotOrder()
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public event EventHandler<RobotStateChangedEvent>? RobotOrderStateChanged;
    //
    // public Task RequestInstantAction()
    // {
    //     throw new NotImplementedException();
    // }
    
    private void OnRobotPositionChanged(object? sender, RobotPositionChangedEvent e)
    {
        RobotPositionChanged?.Invoke(sender, e);
    }
    
    private void OnRobotStateChanged(object? sender, RobotStateChangedEvent e)
    {
        _logger.LogDebug(
            "Robot {robotSerialNumber} state changed to {state}",
            e.RobotSerialNumber.Value,
            e.State.ToJson());
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