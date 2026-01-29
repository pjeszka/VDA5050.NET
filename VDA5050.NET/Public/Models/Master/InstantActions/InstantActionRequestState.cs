using VDA5050.NET.Public.Enums.Vda5050.State;
using VDA5050.NET.Public.Models.Orders.OrderState;

namespace VDA5050.NET.Public.Models.InstantActions;

public sealed record InstantActionRequestState
{
    public InstantActionRequestState(
        RobotSerialNumber RobotSerialNumber,
        ActionId ActionId,
        ActionStatus Status)
    {
        this.RobotSerialNumber = RobotSerialNumber;
        this.ActionId = ActionId;
        this.Status = Status;
    }

    public void UpdateStatus(ActionStatus status)
    {
        Status = status;
    }
    
    public RobotSerialNumber RobotSerialNumber { get; }
    public ActionId ActionId { get; }
    public ActionStatus Status { get; private set; }
}