using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.State;
using VDA5050.NET.Public.Enums.Vda5050.State;

namespace VDA5050.NET.Public.Models.Orders.OrderState;

public sealed record ActionState(string Id, ActionStatus Status, BlockingType? BlockingType, string? Result)
{
    internal static ActionState FromMessage(ActionStateMessage actionStateMessage)
    {
        return new ActionState(
            actionStateMessage.ActionId,
            actionStateMessage.ActionStatus,
            actionStateMessage.BlockingType,
            actionStateMessage.ResultDescription);
    }
}