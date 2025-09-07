namespace VDA5050.NET.VdaDomain.Robots;

public interface IRobotRepository
{
    ICollection<RobotNetworkName> GetRobotNames();
    ICollection<string> GetTopicsForRobot(RobotNetworkName robotNetworkName);
}