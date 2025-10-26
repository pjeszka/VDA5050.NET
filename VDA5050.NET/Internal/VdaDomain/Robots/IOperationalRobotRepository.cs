using VDA5050.NET.Public.Models;

namespace VDA5050.NET.Internal.VdaDomain.Robots;

public interface IOperationalRobotRepository
{
    Task<bool> IsRobotOperational(RobotSerialNumber robotSerialNumber);
    Task AddRobot(OperationalRobot robot);
    Task RemoveRobot(RobotSerialNumber robotSerialNumber);
    Task<ICollection<OperationalRobot>> GetRobots();
    Task<ICollection<string>> GetTopicsForRobot(RobotSerialNumber robotSerialNumber);
    Task<OperationalRobot?> GetRobotForTopic(string topic);
}