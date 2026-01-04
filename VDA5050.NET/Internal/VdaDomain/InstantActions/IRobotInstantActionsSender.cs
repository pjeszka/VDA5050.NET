using VDA5050.NET.Public.Models.InstantActions;

namespace VDA5050.NET.Internal.VdaDomain.InstantActions;

public interface IRobotInstantActionsSender
{
    Task SendInstantAction(RobotInstantActionRequest request);
}