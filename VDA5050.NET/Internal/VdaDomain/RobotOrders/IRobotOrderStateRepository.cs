using VDA5050.NET.Internal.VdaDomain.RobotOrders.OrderRequesting;
using VDA5050.NET.Public.Models;

namespace VDA5050.NET.Internal.VdaDomain.RobotOrders;

public interface IRobotOrderStateRepository
{
    ICollection<CurrentRobotOrderState> GetAll();
    CurrentRobotOrderState GetForRobot(RobotSerialNumber robotSerialNumber);
    void AddRobotOrderState(CurrentRobotOrderState robotOrderState);
    void UpdateRobotOrderState(CurrentRobotOrderState robotOrderState);
}