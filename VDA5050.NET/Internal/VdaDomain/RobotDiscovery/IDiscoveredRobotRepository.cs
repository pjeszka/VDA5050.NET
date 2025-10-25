using VDA5050.NET.Public.Models;

namespace VDA5050.NET.Internal.VdaDomain.RobotDiscovery;

public interface IDiscoveredRobotRepository
{
    ICollection<RobotSerialNumber> GetDiscoveredRobots();
    void AddRobot(DiscoveredRobot robot);
}