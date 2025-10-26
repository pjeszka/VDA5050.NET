using System.Collections.Concurrent;
using VDA5050.NET.Internal.MQTT.Topics;
using VDA5050.NET.Public.Models;

namespace VDA5050.NET.Internal.VdaDomain.Robots;

public sealed class OperationalRobotRepository : IOperationalRobotRepository, ISubscribedTopicsProvider
{
    private readonly ConcurrentDictionary<RobotSerialNumber, OperationalRobot> _robots = new();

    public Task<bool> IsRobotOperational(RobotSerialNumber robotSerialNumber)
    {
        _robots.TryGetValue(robotSerialNumber, out var robot);
        
        return Task.FromResult(robot is not null);
    }

    public Task AddRobot(OperationalRobot robot)
    {
        _robots.AddOrUpdate(robot.SerialNumber, robot, (_, _) => robot);
        return Task.CompletedTask;
    }

    public Task RemoveRobot(RobotSerialNumber robotSerialNumber)
    {
        _robots.TryRemove(robotSerialNumber, out _);
        return Task.CompletedTask;
    }

    public Task<OperationalRobot?> GetRobot(RobotSerialNumber robotSerialNumber)
    {
        _robots.TryGetValue(robotSerialNumber, out var robot);
        
        return Task.FromResult(robot);
    }

    public Task<ICollection<OperationalRobot>> GetRobots()
    {
        return Task.FromResult(_robots.Values);
    }

    public Task<ICollection<string>> GetTopicsForRobot(RobotSerialNumber robotSerialNumber)
    {
        _robots.TryGetValue(robotSerialNumber, out var robot);

        return Task.FromResult(
            robot is null ?
            new List<string>() :
            robot.ObservedTopics);
    }

    public Task<OperationalRobot?> GetRobotForTopic(string topic)
    {
        var robot = _robots
            .Select(x => x.Value)
            .FirstOrDefault(x => x.ObservedTopics.Contains(topic));
        
        return Task.FromResult(robot);
    }

    public async Task<ICollection<string>> GetTopicsToSubscribe()
    {
        var robots = await GetRobots();
        var topics = robots
            .SelectMany(x => x.ObservedTopics)
            .ToList();
        
        return topics;
    }
}