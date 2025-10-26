using VDA5050.NET.Internal.VdaDomain.RobotDiscovery;
using VDA5050.NET.Internal.VdaDomain.Robots;
using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Services;

namespace VDA5050.NET.Internal.VdaDomain.Master;

public sealed class Vda5050Master : IVda5050Master
{
    private readonly IDiscoveredRobotRepository _discoveredRobotRepository;

    public Vda5050Master(IDiscoveredRobotRepository discoveredRobotRepository)
    {
        _discoveredRobotRepository = discoveredRobotRepository;
    }

    public Task<ICollection<RobotSerialNumber>> GetConnectedRobots()
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<DiscoveredRobot>> GetAccessibleRobots()
    {
        return Task.FromResult(_discoveredRobotRepository.GetDiscoveredRobots());
    }

    public Task ConnectRobot(RobotSettings robotSettings)
    {
        throw new NotImplementedException();
    }

    public Task DisconnectRobot(RobotSerialNumber robotSerialNumber)
    {
        throw new NotImplementedException();
    }

    public event EventHandler<RobotStateChangedEvent>? RobotStateChanged;
    public Task SendRobotOrder(RobotOrder robotOrder)
    {
        throw new NotImplementedException();
    }

    public Task UpdateRobotOrder()
    {
        throw new NotImplementedException();
    }

    public event EventHandler<RobotStateChangedEvent>? RobotOrderStateChanged;

    public Task RequestInstantAction()
    {
        throw new NotImplementedException();
    }
}