using VDA5050.NET.Internal.VdaDomain.Robots;
using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Services;

namespace VDA5050.NET.Internal.VdaDomain.Master;

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