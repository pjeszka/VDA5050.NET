using System.Collections.Concurrent;
using VDA5050.NET.Public.Models;

namespace VDA5050.NET.Internal.VdaDomain.Robots;

public sealed class RobotRepository : IRobotRepository
{
    ConcurrentDictionary<RobotNetworkName, Robot> _robots = new();

    public ICollection<RobotNetworkName> GetRobotNames()
    {
        return _robots.Keys;
    }

    public ICollection<string> GetTopicsForRobot(RobotNetworkName robotNetworkName)
    {
        return new List<string>();
    }
}