using VDA5050.NET.Public.Models;

namespace VDA5050.NET.Internal.VdaDomain.Robots;

public interface IConnectedRobotRepository
{
    ICollection<RobotSerialNumber> GetRobotNames();
    ICollection<string> GetTopicsForRobot(RobotSerialNumber robotSerialNumber);
    RobotSerialNumber GetRobotForTopic(string topic);
}