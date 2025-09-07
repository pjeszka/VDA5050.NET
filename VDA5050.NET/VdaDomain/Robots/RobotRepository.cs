using System.Collections.Concurrent;

namespace VDA5050.NET.VdaDomain.Robots;

public sealed class RobotRepository : IRobotRepository
{
    ConcurrentDictionary<RobotNetworkName, Robot> _robots = new();
    public ICollection<RobotNetworkName> GetRobotNames()
    {
        return _robots.Keys;
    }

    public ICollection<string> GetTopicsForRobot(RobotNetworkName robotNetworkName)
    {
    }