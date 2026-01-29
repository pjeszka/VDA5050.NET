using VDA5050.NET.Public.Enums.Vda5050.State;
using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Models.InstantActions;

namespace VDA5050.NET.Internal.VdaDomain.Master.InstantActions;

public interface IInstantActionRequestRepository
{
    ICollection<InstantActionRequestState> GetAllForRobot(RobotSerialNumber robotSerialNumber);
    void AddInstantActionRequest(InstantActionRequestState instantActionRequestState);
    bool TryUpdateInstantActionRequestStatus(ActionId actionId, ActionStatus status);
    void Clear();
}