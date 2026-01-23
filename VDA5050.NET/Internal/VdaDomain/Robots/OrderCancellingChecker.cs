using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.State;
using VDA5050.NET.Public.Enums.Vda5050.State;
using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Models.Orders;

namespace VDA5050.NET.Internal.VdaDomain.Robots;

internal class OrderCancellingChecker
{
    public OrderCancellingChecker(ActionId cancelActionId, OrderId orderId)
    {
        CancelActionId = cancelActionId;
        OrderId = orderId;
    }

    public bool HasOrderBeenCanceled(StateMessage stateMessage)
    {
        var cancelAction = stateMessage.ActionStates.FirstOrDefault(x => x.ActionId == CancelActionId.Value);
        if (cancelAction == null)
        {
            return false;
        }

        return stateMessage.OrderId == OrderId.Value && cancelAction.ActionStatus == ActionStatus.FINISHED;
    }

    public ActionId CancelActionId { get; }
    public OrderId OrderId { get; }
}