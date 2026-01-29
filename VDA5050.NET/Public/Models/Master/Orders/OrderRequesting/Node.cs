using Action = VDA5050.NET.Public.Models.InstantActions.Action;

namespace VDA5050.NET.Public.Models.Orders.OrderRequesting;

public sealed record Node(
    string Id,
    bool Released,
    NodePose NodePose,
    List<Action> Actions,
    string? NodeDescription = null);