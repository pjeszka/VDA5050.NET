using System.Collections.Concurrent;
using VDA5050.NET.Internal.MQTT.Topics;
using VDA5050.NET.Internal.VdaDomain.Master.Robots;
using VDA5050.NET.Public.Models;

namespace VDA5050.NET.Internal.VdaDomain.Robots;

internal sealed class OperationalRobotRepository : IOperationalRobotRepository, ISubscribedTopicsProvider
{
    private readonly ConcurrentDictionary<RobotSerialNumber, OperationalRobot> _robots = new();

    public bool IsRobotOperational(RobotSerialNumber robotSerialNumber)
    {
        _robots.TryGetValue(robotSerialNumber, out var robot);
        
        return robot is not null;
    }

    public void AddRobot(OperationalRobot robot)
    {
        _robots.AddOrUpdate(robot.SerialNumber, robot, (_, _) => robot);
    }

    public void RemoveRobot(RobotSerialNumber robotSerialNumber)
    {
        _robots.TryRemove(robotSerialNumber, out _);
    }

    public OperationalRobot? GetRobot(RobotSerialNumber robotSerialNumber)
    {
        _robots.TryGetValue(robotSerialNumber, out var robot);
        
        return robot;
    }

    public ICollection<OperationalRobot> GetRobots()
    {
        return _robots.Values;
    }

    public ICollection<string> GetTopicsForRobot(RobotSerialNumber robotSerialNumber)
    {
        _robots.TryGetValue(robotSerialNumber, out var robot);

        return robot is null ?
            new List<string>() :
            robot.ObservedTopics;
    }

    public OperationalRobot? GetRobotForTopic(string topic)
    {
        var robot = _robots
            .Select(x => x.Value)
            .FirstOrDefault(x => x.ObservedTopics.Contains(topic));
        
        return robot;
    }

    public ICollection<string> GetTopicsToSubscribe()
    {
        var robots = GetRobots();
        var topics = robots
            .SelectMany(x => x.ObservedTopics)
            .ToList();
        
        return topics;
    }
}