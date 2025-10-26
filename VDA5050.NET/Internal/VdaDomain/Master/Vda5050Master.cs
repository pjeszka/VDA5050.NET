using Microsoft.Extensions.Logging;
using VDA5050.NET.Internal.VdaDomain.RobotDiscovery;
using VDA5050.NET.Internal.VdaDomain.Robots;
using VDA5050.NET.Public.Events;
using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Models.RobotDiscovery;
using VDA5050.NET.Public.Services;

namespace VDA5050.NET.Internal.VdaDomain.Master;

public sealed class Vda5050Master : IVda5050Master
{
    private readonly IDiscoveredRobotRepository _discoveredRobotRepository;
    private readonly IOperationalRobotRepository _operationalRobotRepository;
    private readonly ILogger<Vda5050Master> _logger;

    public Vda5050Master(
        IDiscoveredRobotRepository discoveredRobotRepository,
        IOperationalRobotRepository operationalRobotRepository,
        ILogger<Vda5050Master> logger)
    {
        _discoveredRobotRepository = discoveredRobotRepository;
        _operationalRobotRepository = operationalRobotRepository;
        _logger = logger;
    }

    public Task<ICollection<OperationalRobot>> GetOperationalRobots()
    {
        return _operationalRobotRepository.GetRobots();
    }

    public Task<ICollection<DiscoveredRobot>> GetAccessibleRobots()
    {
        return Task.FromResult(_discoveredRobotRepository.GetDiscoveredRobots());
    }

    public async Task StartRobotOperation(RobotSettings robotSettings)
    {
        if (await _operationalRobotRepository.IsRobotOperational(robotSettings.RobotSerialNumber))
        {
            _logger.LogWarning("Robot {robotSerialNumber} is already connected.", robotSettings.RobotSerialNumber);
            return;
        }

        var connectedRobot = new OperationalRobot(robotSettings, 
            OnRobotConnectionStateChanged,
            OnRobotStateChanged);
        await _operationalRobotRepository.AddRobot(connectedRobot);
    }

    public async Task StopRobotOperation(RobotSerialNumber robotSerialNumber)
    {
        if (await _operationalRobotRepository.IsRobotOperational(robotSerialNumber) is false)
        {
            _logger.LogWarning("Robot {robotSerialNumber} is already disconnected.", robotSerialNumber);
            return;
        }
        
        await _operationalRobotRepository.RemoveRobot(robotSerialNumber);
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
    
    private void OnRobotStateChanged(object? sender, RobotStateChangedEvent e)
    {
        _logger.LogInformation(
            "Robot {robotSerialNumber} state changed to {state}",
            e.RobotSerialNumber,
            e.State);
        RobotStateChanged?.Invoke(sender, e);
    }

    private void OnRobotConnectionStateChanged(object? sender, RobotConnectionStateChangedEvent e)
    {
        _logger.LogInformation(
            "Robot {robotSerialNumber} connection state changed from {previousConnectionState} to {newConnectionState}",
            e.RobotSerialNumber,
            e.PreviousConnectionState,
            e.NewConnectionState);
        RobotConnectionStateChanged?.Invoke(sender, e);
    }
}