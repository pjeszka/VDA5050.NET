using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.State;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.Connection.Enums;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.State;

namespace VDA5050.NET.Public.Models.Robots;

// TODO 
public sealed record RobotState(
    Pose Pose)
{
    internal static RobotState FromMessage(StateMessage stateMessage)
    {
        return new RobotState(new Pose(
            stateMessage.AgvPositionMessage.X,
            stateMessage.AgvPositionMessage.Y,
            0,
            stateMessage.AgvPositionMessage.Theta));
    }
}