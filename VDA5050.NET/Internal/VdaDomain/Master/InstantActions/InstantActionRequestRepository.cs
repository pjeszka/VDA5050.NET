using System.Collections.Concurrent;
using VDA5050.NET.Public.Enums.Vda5050.State;
using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Models.InstantActions;

namespace VDA5050.NET.Internal.VdaDomain.Master.InstantActions;

public class InstantActionRequestRepository : IInstantActionRequestRepository
{
    private readonly ConcurrentDictionary<ActionId, InstantActionRequestState> _instantActionRequests = new ();
    public ICollection<InstantActionRequestState> GetAllForRobot(RobotSerialNumber robotSerialNumber)
    {
        return _instantActionRequests.Values.Where(request => request.RobotSerialNumber == robotSerialNumber).ToList();
    }

    public void AddInstantActionRequest(InstantActionRequestState instantActionRequestState)
    {
        _instantActionRequests.TryAdd(instantActionRequestState.ActionId, instantActionRequestState);
    }

    public bool TryUpdateInstantActionRequestStatus(ActionId actionId, ActionStatus status)
    {
        if (_instantActionRequests.TryGetValue(actionId, out var instantActionRequestState) && instantActionRequestState.Status != status)
        {
            instantActionRequestState!.UpdateStatus(status);
            _instantActionRequests[actionId] = instantActionRequestState;
            return true;
        }
        
        return false;
        
    }

    public void Clear()
    {
        foreach (var request in _instantActionRequests.Values.Where(request => request.Status is ActionStatus.FINISHED or ActionStatus.FAILED))
        {
            _instantActionRequests.TryRemove(request.ActionId, out _);
        }
    }
}