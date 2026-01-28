using System.Collections.Concurrent;
using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Models.RobotDiscovery;

namespace VDA5050.NET.Internal.VdaDomain.RobotDiscovery;

internal sealed class DiscoveredRobotRepository : IDiscoveredRobotRepository
{
    private readonly ConcurrentDictionary<RobotSerialNumber, DiscoveredRobot> _robots = new ();

    public DiscoveredRobot? GetRobot(RobotSerialNumber robotSerialNumber)
    {
        _robots.TryGetValue(robotSerialNumber, out var robot);
        
        return robot;
    }

    public ICollection<DiscoveredRobot> GetDiscoveredRobots()
    {
        return _robots.Values;
    }

    public void AddOrUpdateRobot(DiscoveredRobot robot)
    {
        if (_robots.ContainsKey(robot.SerialNumber))
        {
            
        }

        _robots.AddOrUpdate(
            robot.SerialNumber,
            robot,
            (_, _) => robot);
    }
}