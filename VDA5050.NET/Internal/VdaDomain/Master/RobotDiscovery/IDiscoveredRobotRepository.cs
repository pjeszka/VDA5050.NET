using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Models.RobotDiscovery;

namespace VDA5050.NET.Internal.VdaDomain.RobotDiscovery;

public interface IDiscoveredRobotRepository
{
    DiscoveredRobot? GetRobot(RobotSerialNumber robotSerialNumber);
    ICollection<DiscoveredRobot> GetDiscoveredRobots();
    void AddOrUpdateRobot(DiscoveredRobot robot);
}