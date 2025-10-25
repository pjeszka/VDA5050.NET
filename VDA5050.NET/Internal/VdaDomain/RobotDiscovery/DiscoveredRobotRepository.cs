using System.Collections.Concurrent;
using VDA5050.NET.Public.Models;

namespace VDA5050.NET.Internal.VdaDomain.RobotDiscovery;

public sealed class DiscoveredRobotRepository : IDiscoveredRobotRepository
{
    private readonly ConcurrentDictionary<RobotSerialNumber, DiscoveredRobot> _robots;

    public ICollection<RobotSerialNumber> GetDiscoveredRobots()
    {
        return _robots.Keys;
    }

    public void AddRobot(DiscoveredRobot robot)
    {
        _robots.AddOrUpdate(
            robot.SerialNumber,
            robot,
            (_, _) => robot);
    }
}