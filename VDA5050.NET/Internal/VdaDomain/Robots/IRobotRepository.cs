using VDA5050.NET.Public.Models;

namespace VDA5050.NET.Internal.VdaDomain.Robots;

public interface IRobotRepository
{
    ICollection<RobotNetworkName> GetRobotNames();
    ICollection<string> GetTopicsForRobot(RobotNetworkName robotNetworkName);
}