using VDA5050.NET.API.Models;
using VDA5050.NET.API.Services;
using VDA5050.NET.VdaDomain.Robots;

namespace VDA5050.NET.VdaDomain.Master;

public sealed class Vda5050Master : IVda5050Master
{
    public Task AddRobot(RobotSettings robotSettings)
    {
        throw new NotImplementedException();
    }

    public Task RemoveRobot(RobotNetworkName robotNetworkName)
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
}