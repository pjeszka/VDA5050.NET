using VDA5050.NET.Internal.VdaDomain.Master.Robots;
using VDA5050.NET.Public.Models;

namespace VDA5050.NET.Internal.VdaDomain.Robots;

internal interface IOperationalRobotRepository
{
    OperationalRobot? GetRobot(RobotSerialNumber robotSerialNumber);
    ICollection<OperationalRobot> GetRobots();
    bool IsRobotOperational(RobotSerialNumber robotSerialNumber);
    void AddRobot(OperationalRobot robot);
    void RemoveRobot(RobotSerialNumber robotSerialNumber);
    ICollection<string> GetTopicsForRobot(RobotSerialNumber robotSerialNumber);
    OperationalRobot? GetRobotForTopic(string topic);
}