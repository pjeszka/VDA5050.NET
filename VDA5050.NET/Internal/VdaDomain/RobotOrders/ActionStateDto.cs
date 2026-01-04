using VDA5050.NET.Public.Messages.State;
using VDA5050.NET.Public.Messages.State.Enums;

namespace VDA5050.NET.Internal.VdaDomain.RobotOrders;

public sealed record ActionStateDto(string Id, ActionStatus Status, BlockingType? BlockingType, string? Result)
{
    public static ActionStateDto FromMessage(ActionState actionState)
    {
        return new ActionStateDto(
            actionState.ActionId,
            actionState.ActionStatus,
            actionState.BlockingType,
            actionState.ResultDescription);
    }
}