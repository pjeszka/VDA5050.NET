using System.Collections.Concurrent;
using VDA5050.NET.Public.Models;

namespace VDA5050.NET.Internal.VdaDomain.Robots;

public sealed class ConnectedRobotRepository : IConnectedRobotRepository
{
    ConcurrentDictionary<RobotSerialNumber, ConnectedRobot> _robots = new();

    public ICollection<RobotSerialNumber> GetRobotNames()
    {
        return _robots.Keys;
    }

    public ICollection<string> GetTopicsForRobot(RobotSerialNumber robotSerialNumber)
    {
        return new List<string>();
    }

    public RobotSerialNumber GetRobotForTopic(string topic)
    {
        throw new NotImplementedException();
    }
}