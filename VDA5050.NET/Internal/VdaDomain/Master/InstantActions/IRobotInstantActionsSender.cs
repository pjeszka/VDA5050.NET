using VDA5050.NET.Internal.VdaDomain.Master.Robots;
using VDA5050.NET.Internal.VdaDomain.Robots;
using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Models.InstantActions;

namespace VDA5050.NET.Internal.VdaDomain.Master.InstantActions;

internal interface IRobotInstantActionsSender
{
    Task<ICollection<ActionId>> SendInstantAction(OperationalRobot robot, RobotInstantActionRequest request);
}